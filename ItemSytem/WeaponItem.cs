using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.Text;

namespace MyEnums
{
    public enum WeaponType
    {
        Halberd,
        Spear,
        Sword,
        Blade,
        Knife
    }
}

public class WeaponItem : ItemBase
{
    public WeaponType weaponType;
    public string type_name;
    public int ATK;
    public bool IsEqu;
    public Enchant enchant;
    public SuitEffectInfo suitEffect;
    public WeaponItem(string id, string name, string description, string icon, int max_c, float weight, int bprice, int sprice, bool sellable, bool usable, WeaponType type, int atk, SuitEffectInfo set) :
        base(id, name, description, icon, max_c, weight, bprice, sprice, sellable, usable)
    {
        /*Debug.Log(icon);
        if (icon.Contains("Icon/Item/Weapon/Halberd/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Weapon/Halberd/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Weapon/Spear/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Weapon/Spear/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Weapon/Sword/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Weapon/Sword/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Weapon/Blade/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Weapon/Blade/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Weapon/Shield/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Weapon/Shield/", "");
            icon = sb.ToString();
        }
        Debug.Log(icon);*/
        switch(type)
        {
            case WeaponType.Halberd:type_name = "戟"; /*Icon = "Icon/Item/Weapon/Halberd/" + icon;*/ break;
            case WeaponType.Spear:type_name = "长枪"; /*Icon = "Icon/Item/Weapon/Spear/" + icon;*/ break;
            case WeaponType.Sword:type_name = "单剑"; /*Icon = "Icon/Item/Weapon/Sword/" + icon;*/ break;
            case WeaponType.Blade:type_name = "单刀"; /*Icon = "Icon/Item/Weapon/Blade/" + icon;*/ break;
            default:type_name="匕首"; /*Icon = "Icon/Item/Weapon/Shield/" + icon;*/ break;
        }
        weaponType = type;
        ATK = atk;
        ItemType = ItemType.Weapon;
        ItemTypeName = "武器";
        IsEqu = false;
        MaterialsList = null;
        enchant = null;
        suitEffect = set;
        StackAble = false;
    }

    public override void OnUsed(PlayerInfo playerInfo)
    {
        Equip(playerInfo);
    }

    public void Equip(PlayerInfo playerInfo)
    {
        //Debug.Log("调用了武器装备函数");
        if (IsEqu) throw new System.Exception("已经装备了该物品");
        if (!CheckEquipAble(playerInfo)) { throw new System.Exception("武器不符，无法装备"); }
        IsEqu = true;
        playerInfo.ATK += ATK;
        if (this.weaponType == WeaponType.Knife)
        {
            if (playerInfo.equipments.IsSdEquip)
            {
                //Debug.Log("已经装备了副武器，因此");
                playerInfo.equipments.shield.Unequip(playerInfo);
            }
            playerInfo.equipments.IsSdEquip = true;
            playerInfo.equipments.shield = this;
            //Debug.Log("装备了副武器");
            SystemMessages.AddMessage("装备了副武器");
        }
        else
        {
            if (playerInfo.equipments.IsWpEquip)
            {
                //Debug.Log("已经装备了主武器，因此");
                playerInfo.equipments.weapon.Unequip(playerInfo);
            }
            playerInfo.equipments.IsWpEquip = true;
            playerInfo.equipments.weapon = this;
            //Debug.Log("装备了主武器");
            SystemMessages.AddMessage("装备了主武器");
        }
        if (enchant != null) enchant.Enchanting(playerInfo);
        if (suitEffect != null)
        {
            if (playerInfo.equipments.suitEffect != null)
            {
                SuitEffectInfo sinfo = playerInfo.equipments.suitEffect.Find(s => s.SuittID == suitEffect.SuittID);
                if (sinfo != null)
                {
                    sinfo.currentNum++;
                    sinfo.TryEffect(playerInfo);
                }
                else
                {
                    suitEffect.currentNum++;
                    playerInfo.equipments.suitEffect.Add(suitEffect);
                }
            }
            else
            {
                suitEffect.currentNum++;
                playerInfo.equipments.suitEffect = new List<SuitEffectInfo>
                {
                    suitEffect
                };
            }
        }
        ItemInfo info = playerInfo.bag.itemList.Find(i => i.Item == this);
        info.Quantity--;
        playerInfo.bag.itemList.Remove(info);
        playerInfo.bag.Current_Size--;
    }

    public void Unequip(PlayerInfo playerInfo)
    {
        //Debug.Log("调用了武器卸装函数");
        if (!IsEqu) { throw new System.Exception("尚未装备该物品"); }
        /*if (!playerInfo.equipments.IsSdEquip && type == WeaponType.Shield)
        { Debug.Log("副武器为空"); return "副武器为空"; }
        if (!playerInfo.equipments.IsWpEquip && type != WeaponType.Shield)
        { Debug.Log("武器为空"); return "武器为空"; }*/
        IsEqu = false;
        playerInfo.ATK -= ATK;
        if (weaponType == WeaponType.Knife)
        {
            playerInfo.equipments.IsSdEquip = false;
            playerInfo.equipments.shield = null;
            //Debug.Log("卸下了副武器");
            SystemMessages.AddMessage("卸下了副武器");
        }
        else
        {
            playerInfo.equipments.IsWpEquip = false;
            playerInfo.equipments.weapon = null;
            //Debug.Log("卸下了主武器");
            SystemMessages.AddMessage("卸下了主武器");
        }
        if (enchant != null) enchant.Unenchant(playerInfo);
        if (suitEffect != null)
        {
            SuitEffectInfo sinfo = playerInfo.equipments.suitEffect.Find(s => s.SuittID == suitEffect.SuittID);
            if (sinfo != null)
            {
                sinfo.currentNum--;
                if (sinfo.currentNum <= 0) playerInfo.equipments.suitEffect.Remove(sinfo);
                sinfo.TryUnEffect(playerInfo);
            }
        }
        ItemInfo info = new ItemInfo(this);
        info.Quantity++;
        playerInfo.bag.itemList.Add(info);
        playerInfo.bag.Current_Size++;
    }
    ///   <summary>   
    ///   给装备淬火（附魔）  
    ///   </summary> 
    public void Enchant(Enchant enchant)
    {
        this.enchant = enchant;
    }

    ///   <summary>   
    ///   检查角色是否可以装备该种武器   
    ///   </summary> 
    public bool CheckEquipAble(PlayerInfo playerInfo)
    {
        return ((weaponType == WeaponType.Halberd || weaponType == WeaponType.Blade) && playerInfo.ID == 100001)/*男*/
            || ((weaponType == WeaponType.Spear || weaponType == WeaponType.Blade) && playerInfo.ID == 100002)/*女*/
            || ((weaponType == WeaponType.Spear || weaponType == WeaponType.Sword) && playerInfo.ID == 100003)/*女孩*/
            || (weaponType == WeaponType.Knife);
    }

    public override ItemBase Clone()
    {
        return new WeaponItem(this.ID, this.Name, this.Description, this.Icon, this.MaxCount, this.Weight,
            this.BuyPrice, this.SellPrice, this.SellAble, this.Usable, this.weaponType, this.ATK, suitEffect)
        {
            type_name = this.type_name,
            enchant = this.enchant,
            MaterialsList = this.MaterialsList,
            StackAble = this.StackAble
        };
    }
}