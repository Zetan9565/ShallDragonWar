using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountInfo{
    public float Speed;//奔跑速度
    public float TurnPower;//回旋力

    public float Strength;
    float current_Strength;
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
    float current_Energy;
    public float Current_Energy
    {
        get { return current_Energy; }
        set
        {
            if (value > Energy) current_Energy = Energy;
            else if (value < 0) current_Energy = 0;
            else current_Energy = value;
        }
    }
    public bool IsMount;
    public bool IsTired;
    public bool IsOverWeight;
    public bool IsAlive;
    public MountInfo(float speed, float turnpower, float strength, float energy)
    {
        Speed = speed;
        TurnPower = turnpower;
        Strength = strength;
        Energy = energy;
        Current_Energy = Energy;
        Current_Strength = Strength;
        IsMount = false;
        IsTired = false;
    }

    public void OnMount(PlayerInfo playerInfo)
    {
        if (!IsAlive)
        {
            //throw new System.Exception("坐骑已经死亡");
            return;
        }
        if (IsMount)
            return;
        IsMount = true;
        playerInfo.IsMounting = true;
        //playerInfo.MoveSpeed += Speed;
    }

    public void OnDismount(PlayerInfo playerInfo)
    {
        if (!IsMount)
            return;
        IsMount = false;
        playerInfo.IsMounting = false;
        //playerInfo.MoveSpeed -= Speed;
    }

    public void OnRunning()
    {
        Current_Energy -= Time.deltaTime;
        if (Current_Energy <= 0) IsTired = true;
    }

    public void GetHurt(float hurt_value)
    {
        Current_Strength -= hurt_value;
        if (Current_Strength <= 0) IsAlive = false;
    }

    public void Eating(float str_rec,float ene_rev)
    {
        if (!IsAlive) return;
        Current_Strength += str_rec;
        Current_Energy += ene_rev;
        if (Current_Energy > 0) IsTired = false;
    }

    public void Relive()
    {
        if (IsAlive) return;
        IsAlive = true;
        IsTired = false;
        Current_Strength = Strength;
        Current_Energy = Energy;
    }
}
