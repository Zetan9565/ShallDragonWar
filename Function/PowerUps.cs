using UnityEngine;

public class PowerUps {
    public int ATK_Up;
    public int DEF_Up;
    public int HP_Up;
    public int MP_Up;
    public int Endurance_Up;
    public float Hit_Up;
    public float Dodge_Up;
    public float Crit_Up;
    public bool IsOperant;

    public PowerUps()
    {
        ATK_Up = 0;
        DEF_Up = 0;
        HP_Up = 0;
        MP_Up = 0;
        Endurance_Up = 0;
        Hit_Up = 0;
        Dodge_Up = 0;
        Crit_Up = 0;
        IsOperant = false;
    }

    public PowerUps(string input)
    {
        string[] infos = input.Split(',');
        if (infos.Length < 8)
        {
            ATK_Up = 0;
            DEF_Up = 0;
            HP_Up = 0;
            MP_Up = 0;
            Endurance_Up = 0;
            Hit_Up = 0;
            Dodge_Up = 0;
            Crit_Up = 0;
            IsOperant = false;
        }
        else
        {
            int atk_up, def_up, str_up, ene_up, end_up;
            int.TryParse(infos[0], out atk_up);
            int.TryParse(infos[1], out def_up);
            int.TryParse(infos[2], out str_up);
            int.TryParse(infos[3], out ene_up);
            int.TryParse(infos[4], out end_up);
            float hit_up, dod_up, cri_up;
            float.TryParse(infos[5], out hit_up);
            float.TryParse(infos[6], out dod_up);
            float.TryParse(infos[7], out cri_up);
            ATK_Up = atk_up;
            DEF_Up = def_up;
            HP_Up = str_up;
            MP_Up = ene_up;
            Endurance_Up = end_up;
            Hit_Up = hit_up;
            Dodge_Up = dod_up;
            Crit_Up = cri_up;
            IsOperant = false;
        }
    }

    public PowerUps(int atk_up, int def_up, int hp_up, int mp_up, int end_up, float hit_up, float dod_up, float cri_up)
    {
        ATK_Up = atk_up;
        DEF_Up = def_up;
        HP_Up = hp_up;
        MP_Up = mp_up;
        Endurance_Up = end_up;
        Hit_Up = hit_up;
        Dodge_Up = dod_up;
        Crit_Up = cri_up;
        IsOperant = false;
    }

    public void TryPowerUp(PlayerInfo playerInfo)
    {
        if (IsOperant) return;
        IsOperant = true;
        playerInfo.ATK += ATK_Up;
        playerInfo.DEF += DEF_Up;
        playerInfo.HP += HP_Up;
        playerInfo.MP += MP_Up;
        playerInfo.Endurance += Endurance_Up;
        playerInfo.Hit += Hit_Up;
        playerInfo.Dodge += Dodge_Up;
        playerInfo.Crit += Crit_Up;
    }

    public void TryPowerDown(PlayerInfo playerInfo)
    {
        if (!IsOperant) return;
        IsOperant = false;
        playerInfo.ATK -= ATK_Up;
        playerInfo.DEF -= DEF_Up;
        playerInfo.HP -= HP_Up;
        if (playerInfo.Current_HP > playerInfo.HP) playerInfo.Current_HP = playerInfo.HP;
        playerInfo.MP -= MP_Up;
        if (playerInfo.Current_MP > playerInfo.MP) playerInfo.Current_MP = playerInfo.MP;
        playerInfo.Endurance -= Endurance_Up;
        if (playerInfo.Current_Endurance > playerInfo.Endurance) playerInfo.Current_Endurance = playerInfo.Endurance;
        playerInfo.Hit -= Hit_Up;
        playerInfo.Dodge -= Dodge_Up;
        playerInfo.Crit -= Crit_Up;
    }

    public PowerUps Clone()
    {
        return new PowerUps(ATK_Up, DEF_Up, HP_Up, MP_Up, Endurance_Up, Hit_Up, Dodge_Up, Crit_Up);
    }
}
