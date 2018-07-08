using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine
{
    public enum Statu
    {
        None,
        Rigidity,//硬直
        Repulsed,//击退，不写入状态列表
        Giddy,//气绝
        Floated,//浮空
        Blowed,//击飞，不写入状态列表
        Falled,//倒地
        Poisoning,//中毒
        BlockBroken
    }
}
public class StatuInfo
{
    public int ID;
    public Statu Statu;
    public string statu_name;
    private float duration;
    public float Duration
    {
        get { return duration; }
        set
        {
            if (value >= 0.0f) duration = value;
            else duration = 0.0f;
        }
    }
    public StatuInfo(Statu statu, float duration)
    {
        this.Statu = statu;
        Duration = duration;
        switch (statu)
        {
            case Statu.Rigidity: ID = 90000; statu_name = "硬直"; break;
            case Statu.Repulsed: ID = 90001; statu_name = "击退"; break;
            case Statu.Giddy: ID = 90002; statu_name = "气绝"; break;
            case Statu.Floated: ID = 90003; statu_name = "浮空"; break;
            case Statu.Blowed: ID = 90004; statu_name = "击飞"; break;
            case Statu.Falled: ID = 90005; statu_name = "倒地"; break;
            case Statu.Poisoning: ID = 90006; statu_name = "中毒"; break;
        }
    }
    public void SubTime(float time)
    {
        Duration -= time;
    }
    public bool IsEnd()
    {
        return Duration <= 0;
    }

    public static string GetStatuName(Statu statu)
    {
        string statu_name = string.Empty;
        switch (statu)
        {
            case Statu.Rigidity: statu_name = "僵直"; break;
            case Statu.Repulsed: statu_name = "击退"; break;
            case Statu.Giddy: statu_name = "气绝"; break;
            case Statu.Floated: statu_name = "浮空"; break;
            case Statu.Blowed: statu_name = "击飞"; break;
            case Statu.Falled: statu_name = "倒地"; break;
            case Statu.Poisoning: statu_name = "中毒"; break;
        }
        return statu_name;
    }
}