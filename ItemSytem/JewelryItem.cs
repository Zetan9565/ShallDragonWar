using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.Text;

namespace MyEnums
{
    public enum JewelryType
    {
        Necklace,
        Belt,
        Ring
    }
}


public class JewelryItem : ItemBase
{
    public JewelryType jewelry_Type;
    public string type_name;
    public PowerUps power_Add;
    public bool IsEqu;
    public Enchant enchant;
    public SuitEffectInfo suitEffect;
    public JewelryItem(string id, string name, string description, string icon, int max_c, float weight, int bprice, int sprice, bool sellable, bool usable, JewelryType type, PowerUps add, SuitEffectInfo set) :
        base(id, name, description, icon, max_c, weight, bprice, sprice, sellable, usable)
    {
        /*if(icon.Contains("Icon/Item/Jewelry/Necklace/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Jewelry/Necklace/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Jewelry/Belt/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Jewelry/Belt/", "");
            icon = sb.ToString();
        }
        else if(icon.Contains("Icon/Item/Jewelry/Ring/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Jewelry/Ring/", "");
            icon = sb.ToString();
        }*/
        switch (type)
        {
            case JewelryType.Necklace: type_name = "项链"; /*Icon = "Icon/Item/Jewelry/Necklace/" + icon;*/ break;
            case JewelryType.Belt: type_name = "腰饰"; /*Icon = "Icon/Item/Jewelry/Belt/" + icon;*/ break;
            case JewelryType.Ring: type_name = "戒指"; /*Icon = "Icon/Item/Jewelry/Ring/" + icon;*/ break;
        }
        this.jewelry_Type = type;
        if(add != null) power_Add = add.Clone();
        ItemType = ItemType.Jewelry;
        ItemTypeName = "首饰";
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
        //Debug.Log("调用了首饰装备函数");
        if (IsEqu) throw new System.Exception("已经装备了该物品");
        IsEqu = true;
        power_Add.TryPowerUp(playerInfo);
        switch (jewelry_Type)
        {
            case JewelryType.Necklace:
                if (playerInfo.equipments.IsNlEquip)
                {
                    //Debug.Log("已经装备了项链，因此");
                    playerInfo.equipments.necklace.Unequip(playerInfo, 0);
                }
                playerInfo.equipments.IsNlEquip = true;
                playerInfo.equipments.necklace = this;
                //Debug.Log("装备了项链");
                break;
            case JewelryType.Belt:
                if (playerInfo.equipments.IsBtEquip)
                {
                    //Debug.Log("已经装备了腰饰，因此");
                    playerInfo.equipments.belt.Unequip(playerInfo, 0);
                }
                playerInfo.equipments.IsBtEquip = true;
                playerInfo.equipments.belt = this;
                //Debug.Log("装备了腰饰");
                break;
            case JewelryType.Ring:
                if (playerInfo.equipments.IsRgEquip_1 && !playerInfo.equipments.IsRgEquip_2)
                {
                    playerInfo.equipments.IsRgEquip_2 = true;
                    playerInfo.equipments.ring_2 = this;
                }
                else
                {
                    if (playerInfo.equipments.IsRgEquip_1 && playerInfo.equipments.IsRgEquip_2)
                    {
                        //Debug.Log("两手都装备了戒指，因此");
                        playerInfo.equipments.ring_1.Unequip(playerInfo, 1);
                    }
                    playerInfo.equipments.IsRgEquip_1 = true;
                    playerInfo.equipments.ring_1 = this;
                }
                //Debug.Log("装备了戒指");
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

    public void Unequip(PlayerInfo playerInfo,int ring1or2)
    {
        //Debug.Log("调用了首饰卸装函数");
        if (!IsEqu) throw new System.Exception("尚未装备该物品");
        IsEqu = false;
        power_Add.TryPowerDown(playerInfo);
        switch (jewelry_Type)
        {
            case JewelryType.Necklace:
                playerInfo.equipments.IsNlEquip = false;
                playerInfo.equipments.necklace = null;
                //Debug.Log("卸下了项链");
                break;
            case JewelryType.Belt:
                playerInfo.equipments.IsBtEquip = false;
                playerInfo.equipments.belt = null;
                //Debug.Log("卸下了腰饰");
                break;
            case JewelryType.Ring:
                if (ring1or2 == 1)
                {
                    playerInfo.equipments.IsRgEquip_1 = false;
                    playerInfo.equipments.ring_1 = null;
                    //Debug.Log("卸下了戒指1");
                }
                else if (ring1or2 == 2)
                {
                    playerInfo.equipments.IsRgEquip_2 = false;
                    playerInfo.equipments.ring_2 = null;
                    //Debug.Log("卸下了戒指2");
                }
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
        return new JewelryItem(this.ID, this.Name, this.Description, this.Icon, this.MaxCount, this.Weight,
            this.BuyPrice, this.SellPrice, this.SellAble, this.Usable, this.jewelry_Type, this.power_Add, suitEffect)
        {
            type_name = this.type_name,
            enchant = this.enchant,
            MaterialsList = this.MaterialsList,
            StackAble = this.StackAble
        };
    }
}