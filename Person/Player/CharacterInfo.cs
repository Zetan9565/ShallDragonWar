using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CharacterInfo
{
    public int ID;
    public string Name;
    public string Description;
    public string HeadIcon;

    public int Add_Li;
    public int Add_Ti;
    public int Add_Qi;
    public int Add_Ji;
    public int Add_Min;

    public int Add_HP;
    public int Add_MP;
    public int Add_Endurance;
    public int Add_Neili;

    public CharacterInfo(int id, string name, string decription, string icon, int add_li, int add_ti, int add_qi, int add_ji, int add_min, int add_hp, int add_mp, int add_end, int add_nei)
    {
        ID = id;
        Name = name;
        Description = decription;
        HeadIcon = icon;
        Add_Li = add_li;
        Add_Ti = add_ti;
        Add_Qi = add_qi;
        Add_Ji = add_ji;
        Add_Min = add_min;
        Add_HP = add_hp;
        Add_MP = add_mp;
        Add_Endurance = add_end;
        Add_Neili = add_nei;
    }

    public CharacterInfo()
    {
        ID = 0;
        Name = string.Empty;
        Description = string.Empty;
        HeadIcon = string.Empty;
        Add_Li = 0;
        Add_Ti = 0;
        Add_Qi = 0;
        Add_Ji = 0;
        Add_Min = 0;
        Add_HP = 0;
        Add_MP = 0;
        Add_Endurance = 0;
        Add_Neili = 0;
    }

    public void LoadFromFile(string path)
    {
        CharacterInfo input = new CharacterInfo();
        if (File.Exists(path))
            input = JsonUtility.FromJson<CharacterInfo>(File.ReadAllText(path)) ?? input;
        ID = input.ID;
        Name = input.Name;
        Description = input.Description;
        HeadIcon = input.HeadIcon;
        Add_Li = input.Add_Li;
        Add_Ti = input.Add_Ti;
        Add_Qi = input.Add_Qi;
        Add_Ji = input.Add_Ji;
        Add_Min = input.Add_Min;
        Add_HP = input.Add_HP;
        Add_MP = input.Add_MP;
        Add_Endurance = input.Add_Endurance;
        Add_Neili = input.Add_Neili;
    }
}