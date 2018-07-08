using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;

namespace MyEnums
{
    public enum DamageType
    {
        Hit,
        Miss,
        Block,
        BlockBroken,
        HurtBackward,
        Crit,
        Invalid
    }
}

public class MyTools
{
    public static bool Probability(float probability)
    {
        if (probability < 0) return false;
        return Random.Range(100, 10001) / 100.0f <= probability;
    }

    public static int GetDamaged(bool forPlayer, PlayerInfo player, EnemyInfoAgent enemy, SkillInfoAgent skillInfo, out DamageType damageType, out StatuInfo statuInfo, bool hurtForward)
    {
        if (!enemy || !skillInfo)
        {
            Debug.Log(skillInfo);
            damageType = DamageType.Miss;
            statuInfo = new StatuInfo(Statu.None, 0);
            return 0;
        }
        statuInfo = new StatuInfo(Statu.None, 0);
        if (!forPlayer)
        {
            if (!Probability(80 + player.Hit - enemy.Dodge - player.Level - enemy.Level)) { damageType = DamageType.Miss; return 0; }
            damageType = DamageType.Hit;
            float finallyValue = (player.ATK - enemy.DEF * (skillInfo.isCD ? skillInfo.attackMultiple / 100 : (skillInfo.attackMultiple * (1 - skillInfo.subMultiple * 0.01f) / 100)));
            //Debug.Log(skillInfo.attackMultiple * (1 - skillInfo.subMultiple * 0.01f) / 100);
            finallyValue = finallyValue > 1 ? finallyValue : 1;
            if (enemy.IsDown /*|| player.IsFloat*/ || !hurtForward)
            {
                finallyValue *= 2;
                if (!hurtForward)
                    damageType = DamageType.HurtBackward;
            }
            else if (enemy.IsGiddy) finallyValue = finallyValue * 1.5f;
            if (Probability((20 + enemy.Crit) < 60 ? (20 + enemy.Crit) : 60)) { finallyValue *= 2; damageType = DamageType.Crit; }
            if (enemy.IsBlocking && hurtForward)
            {
                damageType = DamageType.Block;
                enemy.Current_BlockAmount -= System.Convert.ToInt32(finallyValue);
                if (enemy.Current_BlockAmount == 0)
                    damageType = DamageType.BlockBroken;
                return 0;
            }
            if (skillInfo.statuEffcted && skillInfo.isCDWhenUse)
            {
                statuInfo.Statu = skillInfo.attachStatu;
                statuInfo.Duration = skillInfo.statuDuration;
            }
            if (skillInfo.isCDWhenUse)
            {
                player.Current_HP += skillInfo.recHPWhenHit;
                player.Current_MP += skillInfo.recMPWhenHit;
            }
            return System.Convert.ToInt32(finallyValue);
        }
        else
        {
            if (!Probability(80 + enemy.Hit - player.Dodge - enemy.Level - player.Level)) { damageType = DamageType.Miss; return 0; }
            damageType = DamageType.Hit;
            float finallyValue = (enemy.ATK - player.DEF * (skillInfo.isCD ? skillInfo.attackMultiple / 100 : (skillInfo.attackMultiple * (1 - skillInfo.subMultiple * 0.01f) / 100)));
            finallyValue = finallyValue > 1 ? finallyValue : 1;
            if (player.IsDown /*|| player.IsFloat*/ || !hurtForward)
            {
                finallyValue *= 2;
                if (!hurtForward)
                    damageType = DamageType.HurtBackward;
            }
            else if (player.IsGiddy) finallyValue = finallyValue * 1.5f;
            if (Probability((20 + player.Crit) < 60 ? (20 + player.Crit) : 60)) { finallyValue *= 2; damageType = DamageType.Crit; }
            if (player.IsBlocking && hurtForward)
            {
                damageType = DamageType.Block;
                player.Current_MP += System.Convert.ToInt32(player.MP * 0.05f);
                player.Current_BlockAmount -= System.Convert.ToInt32(finallyValue);
                if (player.Current_BlockAmount == 0)
                    damageType = DamageType.BlockBroken;
                return 0;
            }
            if (skillInfo.statuEffcted)
            {
                statuInfo.Statu = skillInfo.attachStatu;
                statuInfo.Duration = skillInfo.statuDuration;
            }
            return System.Convert.ToInt32(finallyValue);
        }
    }

    public static void GetNextEXP(PlayerInfo playerInfo)
    {
        int nextEXP = 0;
        if (playerInfo.Level - 1 <= 20)
            nextEXP = playerInfo.NextExp + 1000 * (playerInfo.Level);
        else
        {
            nextEXP = playerInfo.NextExp * 2;
        }
        playerInfo.NextExp = nextEXP;
    }

    public static void SetActive(GameObject gameObject, bool value)
    {
        if (!gameObject) return;
        if(gameObject.activeSelf != value)
        {
            gameObject.SetActive(value);
        }
    }

    public static string GetMD5(string filename)
    {
        try
        {
            System.IO.FileStream file = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(file);
            file.Close();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
            return string.Empty;
        }
    }

    public static bool CompareMD5(string filename, string md5hash)
    {
        try
        {
            System.IO.FileStream file = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(file);
            file.Close();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString() == md5hash;
        }
        catch
        {
            return false;
        }
    }
}