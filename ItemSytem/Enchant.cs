using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enchant
{

    public PowerUps powerUps;
    public float Res_Rig_Add;
    public float Res_Req_Add;
    public float Res_Stu_Add;
    public float Res_Flo_Add;
    public float Res_Blo_Add;
    public float Res_Fal_Add;
    public bool IsOperant;

    public Enchant()
    {
        powerUps = new PowerUps();
        Res_Rig_Add = 0;
        Res_Req_Add = 0;
        Res_Stu_Add = 0;
        Res_Flo_Add = 0;
        Res_Blo_Add = 0;
        Res_Fal_Add = 0;
        IsOperant = false;
    }

    #region 单独设置
    public void SetATK_Add(float probability,int min,int max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.ATK_Up = Random.Range(min, max);
    }

    public void SetDEF_Add(float probability, int min, int max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.DEF_Up = Random.Range(min, max);
    }

    public void SetStrength_Add(float probability, int min, int max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.HP_Up = Random.Range(min, max);
    }

    public void SetEnergy_Add(float probability, int min, int max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.MP_Up= Random.Range(min, max);
    }

    public void SetEndurance_Add(float probability, int min, int max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.Endurance_Up = Random.Range(min, max);
    }

    public void SetHit_Add(float probability, int min, int max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.Hit_Up = Random.Range(min, max);
    }

    public void SetDodge_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.Dodge_Up = Random.Range(min, max);
    }

    public void SetCrit_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            powerUps.Crit_Up = Random.Range(min, max);
    }

    public void SetRes_Rig_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            Res_Rig_Add = Random.Range(min, max);
    }

    public void SetRes_Req_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            Res_Req_Add = Random.Range(min, max);
    }

    public void SetRes_Stu_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            Res_Stu_Add = Random.Range(min, max);
    }

    public void SetRes_Flo_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            Res_Flo_Add = Random.Range(min, max);
    }

    public void SetRes_Blo_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            Res_Blo_Add = Random.Range(min, max);
    }

    public void SetRes_Fal_Add(float probability, float min, float max)
    {
        if (MyTools.Probability(probability) && min <= max)
            Res_Fal_Add = Random.Range(min, max);
    }
    #endregion

    /// <summary>
    /// 通过三维数组分别设置出现该属性的概率、该属性的最小值、最大值
    /// </summary>
    /// <param name="pro_min_max">属性的出现的概率、最小值、最大值</param>
    public void SetByArray(Vector3[] pro_min_max)
    {
        if (pro_min_max.Length < 14) return;
        SetATK_Add(pro_min_max[0].x, (int)pro_min_max[0].y, (int)pro_min_max[0].z);
        SetDEF_Add(pro_min_max[1].x, (int)pro_min_max[1].y, (int)pro_min_max[1].z);
        SetStrength_Add(pro_min_max[2].x, (int)pro_min_max[2].y, (int)pro_min_max[2].z);
        SetEnergy_Add(pro_min_max[3].x, (int)pro_min_max[3].y, (int)pro_min_max[3].z);
        SetEndurance_Add(pro_min_max[4].x, (int)pro_min_max[4].y, (int)pro_min_max[4].z);
        SetHit_Add(pro_min_max[5].x, (int)pro_min_max[5].y, (int)pro_min_max[5].z);
        SetDodge_Add(pro_min_max[6].x, pro_min_max[6].y, pro_min_max[6].z);
        SetCrit_Add(pro_min_max[7].x, pro_min_max[7].y, pro_min_max[7].z);
        SetRes_Rig_Add(pro_min_max[8].x, pro_min_max[8].y, pro_min_max[8].z);
        SetRes_Req_Add(pro_min_max[9].x, pro_min_max[9].y, pro_min_max[9].z);
        SetRes_Stu_Add(pro_min_max[10].x, pro_min_max[10].y, pro_min_max[10].z);
        SetRes_Flo_Add(pro_min_max[11].x, pro_min_max[11].y, pro_min_max[11].z);
        SetRes_Blo_Add(pro_min_max[12].x, pro_min_max[12].y, pro_min_max[12].z);
        SetRes_Fal_Add(pro_min_max[13].x, pro_min_max[13].y, pro_min_max[13].z);
    }

    public void SetByArraySimple(float[] pro)
    {
        if (pro.Length < 14) return;
        SetATK_Add(pro[0], 10, 20);
        SetDEF_Add(pro[1], 10, 20);
        SetStrength_Add(pro[2], 30, 40);
        SetEnergy_Add(pro[3],20, 30);
        SetEndurance_Add(pro[4], 15, 25);
        SetHit_Add(pro[5], 5, 10);
        SetDodge_Add(pro[6], 5, 10);
        SetCrit_Add(pro[7], 2, 5);
        SetRes_Rig_Add(pro[8], 5, 10);
        SetRes_Req_Add(pro[9], 5, 10);
        SetRes_Stu_Add(pro[10], 5, 10);
        SetRes_Flo_Add(pro[11], 5, 10);
        SetRes_Blo_Add(pro[12], 5, 10);
        SetRes_Fal_Add(pro[13], 5, 10);
    }

    public Enchant GetByArray(Vector3[] pro_min_max)
    {
        SetByArray(pro_min_max);
        return this;
    }

    public void Enchanting(PlayerInfo playerInfo)
    {
        if (IsOperant) return;
        IsOperant = true;
        powerUps.TryPowerUp(playerInfo);
        playerInfo.Res_Rigidity += Res_Rig_Add;
        playerInfo.Res_Repulsed += Res_Req_Add;
        playerInfo.Res_Stuned += Res_Stu_Add;
        playerInfo.Res_Floated += Res_Flo_Add;
        playerInfo.Res_Blowed += Res_Blo_Add;
        playerInfo.Res_Falled += Res_Fal_Add;
    }

    public void Unenchant(PlayerInfo playerInfo)
    {
        if (!IsOperant) return;
        IsOperant = false;
        powerUps.TryPowerDown(playerInfo);
        playerInfo.Res_Rigidity -= Res_Rig_Add;
        playerInfo.Res_Repulsed -= Res_Req_Add;
        playerInfo.Res_Stuned -= Res_Stu_Add;
        playerInfo.Res_Floated -= Res_Flo_Add;
        playerInfo.Res_Blowed -= Res_Blo_Add;
        playerInfo.Res_Falled -= Res_Fal_Add;
    }

    /*public override bool Equals(object obj)
    {
        Enchant enchant = obj as Enchant;
        if ((object)enchant == null) return false;
        return base.Equals(obj) && powerUps.Equals(enchant.powerUps);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }*/
}