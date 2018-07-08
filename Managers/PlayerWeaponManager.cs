using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{

    public static PlayerWeaponManager Instance;
    public bool isInit;

    public string currentWeaponTag;

    public List<GameObject> weaponsInHand;
    public List<GameObject> weaponsInBack;
    public Collider frontTrigger;
    public XftWeapon.XWeaponTrail[] frontTrails;
    public Collider backTrigger;
    public XftWeapon.XWeaponTrail[] backTrails;

    //[HideInInspector]
    public MyEnums.WeaponType weaponOneType;
    //[HideInInspector]
    public MyEnums.WeaponType weaponTwoType;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    public void Init()
    {
        isInit = true;
        PlayerObjectAgent playerObjectAgent = PlayerInfoManager.Instance.Player.GetComponent<PlayerObjectAgent>();
        weaponsInHand = playerObjectAgent.weaponsInHand;
        weaponsInBack = playerObjectAgent.weaponsInBack;
        frontTrigger = playerObjectAgent.frontTrigger;
        frontTrails = playerObjectAgent.frontTrails;
        backTrigger = playerObjectAgent.backTrigger;
        backTrails = playerObjectAgent.backTrails;
        CheckWeaponTypeForCharactor();
        foreach (GameObject weapon in weaponsInHand)
            MyTools.SetActive(weapon, false);
        foreach (GameObject weapon in weaponsInBack)
            MyTools.SetActive(weapon, false);
        if (frontTrigger)
        {
            frontTrigger.enabled = false;
            frontTrigger.isTrigger = true;
            frontTrigger.tag = "PlayerWeapon";
        }
        if(backTrigger)
        {
            backTrigger.enabled = false;
            backTrigger.isTrigger = true;
            backTrigger.tag = "PlayerWeapon";
        }
        foreach (XftWeapon.XWeaponTrail wt in frontTrails)
        {
            wt.Init();
            wt.Deactivate();
        }
        foreach (XftWeapon.XWeaponTrail wt in backTrails)
        {
            wt.Init();
            wt.Deactivate();
        }
    }

    public void SetFrontTrigger(bool state)
    {
        if (!isInit) return;
        if (!frontTrigger) return;
        frontTrigger.enabled = state;
    }
    public void SetFrontTrail(bool state, float time = 0, bool smooth = false)
    {
        if (!isInit) return;
        if (frontTrails.Length == 0) return;
        if (currentWeaponTag == weaponOneType.ToString())
        {    /*    MyTools.SetActive(frontTrails[0].gameObject, state);
            else
            {
                if (state) MyTools.SetActive(frontTrails[1].gameObject, state);
            }*/
            if (!frontTrails[0].gameObject.activeSelf && state) frontTrails[0].Activate();
            else if (frontTrails[0].gameObject.activeSelf && !state)
            {
                if (smooth && time > 0 && !frontTrails[0].GetIsFading()) frontTrails[0].StopSmoothly(time);
                else
                {
                    frontTrails[0].Deactivate();
                }
            }
        }
        else
        {
            if (!frontTrails[1].gameObject.activeSelf && state) frontTrails[1].Activate();
            else if (frontTrails[1].gameObject.activeSelf && !state)
            {
                if (smooth && time > 0 && !frontTrails[1].GetIsFading()) frontTrails[1].StopSmoothly(time);
                else
                {
                    frontTrails[1].Deactivate();
                }
            }
        }
    }

    public void SetBackTrigger(bool state)
    {
        if (!isInit) return;
        if (!backTrigger) return;
        backTrigger.enabled = state;
    }
    public void SetBackTrail(bool state, float time = 0, bool smooth = false)
    {
        if (!isInit) return;
        if (backTrails.Length == 0) return;
        if (currentWeaponTag == weaponOneType.ToString())
        {
            if (!backTrails[0].gameObject.activeSelf && state) backTrails[0].Activate();
            else if (backTrails[0].gameObject.activeSelf && !state)
            {
                if (smooth && time > 0 && !backTrails[0].GetIsFading()) backTrails[0].StopSmoothly(time);
                else
                {
                    backTrails[0].Deactivate();
                }
            }
        }
        else
        {
            if (!backTrails[1].gameObject.activeSelf && state) backTrails[1].Activate();
            else if (backTrails[1].gameObject.activeSelf && !state)
            {
                if (smooth && time > 0 && !backTrails[1].GetIsFading()) backTrails[1].StopSmoothly(time);
                else
                {
                    backTrails[1].Deactivate();

                }
            }
        }
    }
    public void SwapWeaponStatu(bool toFight)
    {
        if (!isInit) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsAlive || PlayerInfoManager.Instance.PlayerInfo.IsMounting || currentWeaponTag == string.Empty) return;
        if (toFight)
        {
            //Debug.Log("swapToFight");
            MyTools.SetActive(weaponsInHand.Find(g => g.tag == currentWeaponTag), true);
            MyTools.SetActive(weaponsInBack.Find(g => g.tag == currentWeaponTag), false);
        }
        else
        {
            //Debug.Log("swapToNormal");
            MyTools.SetActive(weaponsInHand.Find(g => g.tag == currentWeaponTag), false);
            MyTools.SetActive(weaponsInBack.Find(g => g.tag == currentWeaponTag), true);
        }
    }

    public void EquipWeapon()
    {
        //Debug.Log("EquipWeapon");
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.equipments.weapon == null) return;
        currentWeaponTag = PlayerInfoManager.Instance.PlayerInfo.equipments.weapon.weaponType.ToString();
        foreach (GameObject weapon in weaponsInBack.FindAll(g => g.tag == currentWeaponTag))
            MyTools.SetActive(weapon, true);
        foreach (GameObject weapon in weaponsInHand.FindAll(g => g.tag == currentWeaponTag))
            MyTools.SetActive(weapon, false);
        if (currentWeaponTag == "Sword")
            foreach (GameObject sheath in weaponsInBack.FindAll(g => g.tag == "Sheath"))
                MyTools.SetActive(sheath, true);
        if (currentWeaponTag == weaponOneType.ToString())
        {
            PlayerSkillManager.Instance.usingWeaponOne = true;
        }
        else
        {
            PlayerSkillManager.Instance.usingWeaponOne = false;
        }
        PlayerSkillManager.Instance.GetSkills();
        PlayerSkillManager.Instance.StoreSkillToUse();
    }

    public void NoWeapon()
    {
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.equipments.weapon != null) return;
        foreach (GameObject weapon in weaponsInBack.FindAll(g => g.tag == currentWeaponTag))
            MyTools.SetActive(weapon, false);
        foreach (GameObject weapon in weaponsInHand.FindAll(g => g.tag == currentWeaponTag))
            MyTools.SetActive(weapon, false);
        if (currentWeaponTag == "Sword")
            foreach (GameObject sheath in weaponsInBack.FindAll(g => g.tag == "Sheath"))
                MyTools.SetActive(sheath, false);
        currentWeaponTag = string.Empty;
        if (PlayerInfoManager.Instance.PlayerInfo.IsFighting)
            PlayerInfoManager.Instance.PlayerInfo.IsFighting = false;
    }

    public void CheckWeaponTypeForCharactor()
    {
        switch (PlayerInfoManager.Instance.characterType)
        {
            case MyEnums.PlayerCharacterType.Boy:
                weaponOneType = MyEnums.WeaponType.Halberd;
                weaponTwoType = MyEnums.WeaponType.Blade;
                break;
            case MyEnums.PlayerCharacterType.Girl:
                weaponOneType = MyEnums.WeaponType.Sword;
                weaponTwoType = MyEnums.WeaponType.Blade;
                break;
            case MyEnums.PlayerCharacterType.LittleGirl:
                weaponOneType = MyEnums.WeaponType.Spear;
                weaponTwoType = MyEnums.WeaponType.Sword;
                break;
        }
        PlayerSkillManager.Instance.GetSkills();
    }
}
