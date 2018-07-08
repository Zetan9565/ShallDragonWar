using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using MalbersAnimations.HAP;

public class MountInfoManager : MonoBehaviour {

    public MountInfo mountInfo;
    Animal animal;
    Mountable mountable;
    bool isInit;

    /*private void Awake()
    {

    }*/

    // Use this for initialization
    void Init () {
        mountInfo = PlayerInfoManager.Instance.PlayerInfo.mount;
        animal = GetComponent<Animal>();
        mountable = GetComponent<Mountable>();
        UpdateMountInfo();
        isInit = true;
    }

    // Update is called once per frame
    void Update () {
        if (!isInit) return;
        if(mountInfo != null) SubEnergy();
        CheckTired();
        //if (animal.Swim) Debug.Log("Swim");
	}

    void CheckTired()
    {
        if (mountInfo.IsAlive && mountInfo.IsTired) animal.Speed1 = true;
    }

    void CheckOverWeight()
    {
        if (mountInfo.IsAlive && mountInfo.IsOverWeight && !mountInfo.IsTired) animal.Speed2 = true;
    }

    void SubEnergy()
    {
        if (animal.Anim.GetFloat("Vertical") > 0.2f)
        {
            mountInfo.Current_Energy -= Time.deltaTime;
            //Debug.Log("AnimalRunning" + mountInfo.Current_Energy);
            if (mountInfo.Current_Energy <= 0) mountInfo.IsTired = true;
        }
    }

    public void OnJump()
    {
        //Debug.Log("AnimalJump");
    }

    public void OnHurt()
    {
        Debug.Log("AnimalHurt");
    }

    public void OnEat()
    {
        mountInfo.Eating(10, 10);
        CheckTired();
    }

    public void OnRelive()
    {
        Debug.Log("AnimalRelive");
    }

    public void OnDeath()
    {
        Debug.Log("AnimalDeath");
        mountable.active = false;
    }

    public void UpdateMountInfo()
    {
        animal.trotSpeed.position = mountInfo.Speed * 0.01f * 2;
        animal.runSpeed.position = mountInfo.Speed * 0.01f * 4;
        animal.TurnMultiplier = mountInfo.TurnPower * 0.01f * 10;
    }
}
