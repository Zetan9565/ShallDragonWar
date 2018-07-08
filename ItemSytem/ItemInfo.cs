using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyEnums;

public class ItemInfo
{
    public ItemBase Item;
    public string ItemID;
    public string Icon;
    public int MaxCount;
    private int quantity;
    public int Quantity//持有数
    {
        get { return quantity; }
        set
        {
            if (value > MaxCount) quantity = MaxCount;
            else if (value < 0) quantity = 0;
            else quantity = value;
        }
    }
    public bool IsMax;
    public bool StackAble;

    public ItemInfo()
    {
        ItemID = string.Empty;
        Icon = string.Empty;
        MaxCount = 0;
        Quantity = 0;
        IsMax = false;
        StackAble = false;
    }

    public ItemInfo(ItemBase info)
    {
        Item = info;
        ItemID = info.ID;
        Icon = info.Icon;
        MaxCount = info.MaxCount;
        Quantity = 0;
        IsMax = false;
        StackAble = info.StackAble;
    }

    public void OnDiscard(int discard_num, BagInfo bag)
    {
        if (discard_num <= 0) return;
        bool isequip = false;
        switch (Item.ItemType)
        {
            case ItemType.Weapon:
                WeaponItem weapon = Item as WeaponItem;
                if (weapon == null) break;
                isequip = weapon.IsEqu;
                break;
            case ItemType.Armor:
                ArmorItem armor = Item as ArmorItem;
                if (armor == null) break;
                isequip = armor.IsEqu;
                break;
            case ItemType.Jewelry:
                JewelryItem jewelry = Item as JewelryItem;
                if (jewelry == null) break;
                isequip = jewelry.IsEqu;
                break;
        }
        if (isequip) throw new System.Exception("装备中的物品不能丢弃");
        if (Quantity <= 0) throw new System.Exception("该物品为空");
        if (!bag.itemList.Exists(i => i.Item == Item)) throw new System.Exception("该物品未在行囊里");
        int finallyDiscard = StackAble ? Quantity - discard_num > 0 ? discard_num : Quantity : 1;
        //Debug.Log("最终丢弃数为" + finallyDiscard);
        Quantity -= finallyDiscard;
        bag.Current_Weight -= Item.Weight * finallyDiscard;
        if (Quantity <= 0)
        {
            bag.Current_Size -= 1;
            bag.itemList.Remove(this);
        }
        bag.CheckSizeAndWeight();
    }

    public void OnDiscardAll(ItemBase item, BagInfo bag)
    {
        if (Quantity <= 0) throw new System.Exception("该物品为空");
        if (!bag.itemList.Exists(i => i.Item == Item)) throw new System.Exception("该物品未在行囊里");
        if (!StackAble) throw new System.Exception("该物品不可全数丢弃");
        bag.Current_Weight -= item.Weight * Quantity;
        Quantity = 0;
        bag.itemList.RemoveAll(i => i.Item == item);
        bag.Current_Size--;
        bag.CheckSizeAndWeight();
    }

    public void Save(List<string> list)
    {
        list.Add(JsonConvert.SerializeObject(Item));
    }

    public void Load(string info)
    {
        JToken obj = (JToken)JsonConvert.DeserializeObject(info);
        switch (obj["ItemType"].ToObject<ItemType>())
        {
            case ItemType.Weapon:
                Item = obj.ToObject<WeaponItem>();
                break;
            case ItemType.Armor:
                Item = obj.ToObject<ArmorItem>();
                break;
            case ItemType.Jewelry:
                Item = obj.ToObject<JewelryItem>();
                break;
            case ItemType.Medicine:
                Item = obj.ToObject<MedicineItem>();
                break;
            case ItemType.Material:
                Item = obj.ToObject<MaterialItem>();
                break;
            case ItemType.Mount:
                Item = obj.ToObject<MountItem>();
                break;
            case ItemType.Others:
            //case ItemType.Quests:
            default:
                Item = obj.ToObject<ItemBase>();
                break;
        }
        Item.Icon = obj["Icon"].ToString();
        //Debug.Log(Item.Icon);
    }

}