using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyTypeInfo
{
    public string ID;
    public string TypeName;
    public string Description;
    public string HeadIcon;

    public int Add_Li;
    public int Add_Ti;
    public int Add_Qi;
    public int Add_Ji;
    public int Add_Min;

    public float Add_Strength;
    public float Add_Energy;

    public string dropItemsInput;

    public EnermyTypeInfo(string id, string name, string description, string headicon, int add_li, int add_ti, int add_qi, int add_ji, int add_min, int add_str, int add_ene, string dropitems)
    {
        ID = id;
        TypeName = name;
        Description = description;
        HeadIcon = headicon;
        Add_Li = add_li;
        Add_Ti = add_ti;
        Add_Qi = add_qi;
        Add_Ji = add_ji;
        Add_Min = add_min;
        Add_Strength = add_str;
        Add_Energy = add_ene;
        dropItemsInput = dropitems;
    }
}