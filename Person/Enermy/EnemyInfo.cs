using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
    public int ID;
    public string Icon;
    public EnermyTypeInfo TypeInfo;
    public float Li;
    public float Ti;
    public float Qi;
    public float Ji;
    public float Min;
    public float Strength;
    private float current_Strength;
    public float Current_Strength
    {
        get { return current_Strength; }
        set
        {
            if (value > Strength) current_Strength = Strength;
            else if (value < 0) current_Strength = 0;
            else current_Strength = value;
        }
    }
    public float Energy;
    public float Hits;
    public float Dodge;
    public float Crit;
    public float Res_Rigidity;
    public float Res_Repulsed;
    public float Res_Stuned;
    public float Res_Floated;
    public float Res_Blowed;
    public float Res_Falled;

    public bool IsAlive;
    public bool IsPatroling;
    public bool IsFighting;

    public bool IsDown;
    public bool IsDiggy;
    public bool IsFloat;

    int level;
    public int Level
    {
        get { return level; }
        set
        {
            if (value >= 40) level = 40;
            else if (level < 1) level = 1;
            else level = value;
        }
    }

    public string dropItemsInput;
}