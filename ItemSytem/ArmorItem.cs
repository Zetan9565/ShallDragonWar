using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.Text;

namespace MyEnums
{
    public enum ArmorType
    {
        Clothes,
        Helmet,//头饰
        WristBand,//护腕
        Shoes
    }
}
public class ArmorItem : ItemBase
{
    public ArmorType armorType;
    public string type_name;
    public int DEF;
    public bool IsEqu;
    public Enchant enchant;
    public SuitEffectInfo suitEffect;
    public ArmorItem(string id, string name, string description, string icon, int max_c, float weight, int bprice, int sprice, bool sellable, bool usable, ArmorType type, int def, SuitEffectInfo set) :
        base(id, name, description, icon, max_c, weight, bprice, sprice, sellable, usable)
    {
        /*if(icon.Contains("Icon/Item/Armor/Clothes/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Armor/Clothes/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Armor/Helmet/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Armor/Helmet/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Armor/WristBand/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Armor/WristBand/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Armor/Shoes/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Armor/Shoes/", "");
            icon = sb.ToString();
        }*/
        switch (type)
        {
            case ArmorType.Clothes: type_name = "衣服"; /*Icon = "Icon/Item/Armor/Clothes/" + icon*/; break;
            case ArmorType.Helmet: type_name = "头饰"; /*Icon = "Icon/Item/Armor/Helmet/" + icon*/; break;
            case ArmorType.WristBand: type_name = "护腕"; /*Icon = "Icon/Item/Armor/WristBand/" + icon*/; break;
            case ArmorType.Shoes: type_name = "鞋履"; /*Icon = "Icon/Item/Armor/Shoes/" + icon*/; break;
        }
        this.armorType = type;
        DEF = def;
        ItemType = ItemType.Armor;
        ItemTypeName = "防具";
        IsEqu = false;
        enchant = null;
        StackAble = false;
        suitEffect = set;
    }
    public override void OnUsed(PlayerInfo playerInfo)
    {
        Equip(playerInfo);
    }

    public void Equip(PlayerInfo playerInfo)
    {
        //Debug.Log("调用了防具装备函数");
        if (IsEqu) throw new System.Exception("已经装备了该物品"); 
        IsEqu = true;
        playerInfo.DEF += DEF;
        switch(armorType)
        {
            case ArmorType.Clothes:
                if (playerInfo.equipments.IsClEquip)
                {
                    //Debug.Log("已经装备了衣服，因此");
                    playerInfo.equipments.clothes.Unequip(playerInfo);
                }
                playerInfo.equipments.IsClEquip=true;
                playerInfo.equipments.clothes = this;
                //Debug.Log("装备了衣服");
                SystemMessages.AddMessage("装备了衣服");
                break;
            case ArmorType.Helmet:
                if (playerInfo.equipments.IsHmEquip)
                {
                    //Debug.Log("已经装备了头饰，因此");
                    playerInfo.equipments.helmet.Unequip(playerInfo);
                }
                playerInfo.equipments.IsHmEquip = true;
                playerInfo.equipments.helmet = this;
                //Debug.Log("装备了头饰");
                SystemMessages.AddMessage("装备了头饰");
                break;
            case ArmorType.WristBand:
                if (playerInfo.equipments.IsWBEquip)
                {
                    //Debug.Log("已经装备了护腕，因此");
                    playerInfo.equipments.wristband.Unequip(playerInfo);
                }
                playerInfo.equipments.IsWBEquip = true;
                playerInfo.equipments.wristband = this;
                //Debug.Log("装备了护腕");
                SystemMessages.AddMessage("装备了护腕");
                break;
            case ArmorType.Shoes:
                if (playerInfo.equipments.IsShEquip)
                {
                    //Debug.Log("已经装备了鞋履，因此");
                    playerInfo.equipments.shoes.Unequip(playerInfo);
                }
                playerInfo.equipments.IsShEquip = true;
                playerInfo.equipments.shoes = this;
                //Debug.Log("装备了鞋履");
                SystemMessages.AddMessage("装备了鞋履");
                break;
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
        //Debug.Log("调用了防具卸装函数");
        if (!IsEqu) throw new System.Exception("尚未装备该物品");
        IsEqu = false;
        playerInfo.DEF -= DEF;
        switch (armorType)
        {
            case ArmorType.Clothes:
                playerInfo.equipments.IsClEquip = false;
                playerInfo.equipments.clothes = null;
                //Debug.Log("卸下了衣服");
                SystemMessages.AddMessage("卸下了衣服");
                break;
            case ArmorType.Helmet:
                playerInfo.equipments.IsHmEquip = false;
                playerInfo.equipments.helmet = null;
                //Debug.Log("卸下了头饰");
                SystemMessages.AddMessage("卸下了头饰");
                break;
            case ArmorType.WristBand:
                playerInfo.equipments.IsWBEquip = false;
                playerInfo.equipments.wristband = null;
                //Debug.Log("卸下了护腕");
                SystemMessages.AddMessage("卸下了护腕");
                break;
            case ArmorType.Shoes:
                playerInfo.equipments.IsShEquip = false;
                playerInfo.equipments.shoes = null;
                //Debug.Log("卸下了鞋履");
                SystemMessages.AddMessage("卸下了鞋履");
                break;
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

    public override ItemBase Clone()
    {
        return new ArmorItem(this.ID, this.Name, this.Description, this.Icon, this.MaxCount, this.Weight,
            this.BuyPrice, this.SellPrice, this.SellAble, this.Usable, this.armorType, this.DEF, this.suitEffect)
        {
            type_name = type_name,
            enchant = enchant,
            MaterialsList = MaterialsList,
            StackAble = StackAble
        };
    }
}