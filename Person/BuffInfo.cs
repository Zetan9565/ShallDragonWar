using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo {
    public int ID;
    public bool isDebuff;
    public string Name;
    public string Description;
    public float duration;
    public float Duration
    {
        get { return duration; }
        set
        {
            if (value >= 0) duration = value;
            else duration = 0;
        }
    }

    public PowerUps powerUps;

    public BuffInfo(int id,bool debuff,string name,string descripton,float duration,PowerUps ups)
    {
        ID = id;
        isDebuff = debuff;
        Name = name;
        Description = descripton;
        Duration = duration;
        powerUps = ups;
    }

    public BuffInfo(BuffInfo info,float duration)
    {
        ID = info.ID;
        isDebuff = info.isDebuff;
        Name = info.Name;
        Description = info.Description;
        powerUps = info.powerUps;
        Duration = duration;
    }

    public void OnBegined(PlayerInfo playerInfo)
    {
        playerInfo.HP += powerUps.HP_Up;
        playerInfo.MP += powerUps.MP_Up;
        playerInfo.Endurance += powerUps.Endurance_Up;
        playerInfo.ATK += powerUps.ATK_Up;
        playerInfo.DEF += powerUps.DEF_Up;
        playerInfo.Hit += powerUps.Hit_Up;
        playerInfo.Dodge += powerUps.Dodge_Up;
        playerInfo.Crit += powerUps.Crit_Up;
    }

    public void OnEnd(PlayerInfo playerInfo)
    {
        playerInfo.HP -= powerUps.HP_Up;
        playerInfo.MP -= powerUps.MP_Up;
        playerInfo.Endurance -= powerUps.Endurance_Up;
        playerInfo.ATK -= powerUps.ATK_Up;
        playerInfo.DEF -= powerUps.DEF_Up;
        playerInfo.Hit -= powerUps.Hit_Up;
        playerInfo.Dodge -= powerUps.Dodge_Up;
        playerInfo.Crit -= powerUps.Crit_Up;
    }

    public void SubTime(int time)
    {
        Duration -= time;
    }

    public bool IsEnd()
    {
        return Duration <= 0;
    }
}
