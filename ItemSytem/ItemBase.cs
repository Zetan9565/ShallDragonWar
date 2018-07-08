using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System;

namespace MyEnums
{
    public enum ItemType
    {
        Others,
        Weapon,
        Armor,
        Jewelry,//首饰
        Medicine,//恢复剂
        Quests,//任务道具
        Material,//材料
        Mount//坐骑
        //等等
    }
}

public class ItemBase
{
    public string ID;
    public string Name;
    public string Description;
    public string Icon;
    public int MaxCount;
    public float Weight;
    public ItemType ItemType;
    public string ItemTypeName;
    
    public int BuyPrice;
    public int SellPrice;
    public bool SellAble;

    public bool Usable;
    public bool StackAble;
    public List<MaterialInfo> MaterialsList;
    public string MaterialsListInput;

    public ItemBase(string id, string name, string description, string icon, int max_c, float weight, int bprice, int sprice, bool sellable, bool usable)
    {
        ID = id;
        Name = name;
        Description = description;
        Icon = icon;
        MaxCount = max_c;
        Weight = weight;
        BuyPrice = bprice;
        SellPrice = sprice;
        SellAble = sellable;
        ItemType = ItemType.Others;
        ItemTypeName = "其他";
        Usable = usable;
        StackAble = true;
        MaterialsList = null;
        MaterialsListInput = string.Empty;
    }

    public virtual void OnUsed(PlayerInfo playerInfo)
    {
        if (playerInfo == null) return;
    }

    public virtual void OnUsed(GameObject acted_go)
    {
        if (!Usable) return;
    }

    public virtual int OnBuy(BagInfo bag, int buy_num)
    {
        if (buy_num <= 0) return 0;
        if (bag.Money < BuyPrice * buy_num) throw new System.Exception("金钱不足");
        int finallyBuy = 0;
        if (StackAble)
        {
            ItemInfo itemInBag = bag.itemList.Find(i => i.ItemID == ID);
            try
            {
                if (itemInBag != null)
                {
                    finallyBuy = itemInBag.Quantity;
                    bag.GetItem(this, buy_num);
                    itemInBag = bag.itemList.Find(i => i.ItemID == ID);
                    finallyBuy = Mathf.Abs(itemInBag.Quantity - finallyBuy);
                }
                else
                {
                    //finallyBuy = MaxCount > buy_num ? buy_num : MaxCount;
                    bag.GetItem(this, buy_num);
                    itemInBag = bag.itemList.Find(i => i.ItemID == ID);
                    if (itemInBag != null) finallyBuy = itemInBag.Quantity;
                }
                bag.LoseMoney(BuyPrice * finallyBuy);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        else
        {
            try
            {
                finallyBuy = bag.itemList.FindAll(i => i.ItemID == ID).Count;
                bag.GetItem(this, buy_num);
                finallyBuy = bag.itemList.FindAll(i => i.ItemID == ID).Count - finallyBuy;
                bag.LoseMoney(BuyPrice * finallyBuy);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        return finallyBuy;
    }

    public virtual void OnSell(BagInfo bag, int sell_num)
    {
        if (sell_num <= 0) return;
        if (SellAble)
        {
            ItemInfo itemInBag = bag.itemList.Find(i => i.Item == this);
            if (itemInBag == null) throw new System.Exception("行囊中不存在该物品");
            int finallySell = itemInBag.Quantity > sell_num ? sell_num : itemInBag.Quantity;
            bag.GetMoney(SellPrice * finallySell);
            bag.LoseItem(itemInBag.Item, finallySell);           
        }
        else throw new System.Exception("该物品不可贩卖");
    }

    ///   <summary>   
    ///   检查材料收集进度   
    ///   </summary> 
    ///   <param name="bag">指定要产生影响的背包数据</param>
    ///   <param name="num">指定数量</param>
    ///   <param name="info">传出材料收集进度及信息以便显示在UI中</param> 
    /// <returns>材料收集足够，可以制作时返回True</returns>
    public bool CheckMaterials(BagInfo bag, int num, out List<string> info)
    {
        info = new List<string>();
        if (MaterialsList == null) return false;
        List<bool> checklist = new List<bool>();
        foreach (MaterialInfo minfo in MaterialsList)
        {
            minfo.Check(bag, num);
            checklist.Add(minfo.IsEnough);
            minfo.IsEnough = false;
            info.Add(minfo.State);
        }
        bool result = true;
        foreach (bool check in checklist)
        {
            result = result && check;
        }
        return result;
    }

    public void AddMaterial(MaterialInfo material)
    {
        if (MaterialsList == null) MaterialsList = new List<MaterialInfo>();
        MaterialsList.Add(material);
    }

    public void SetMaterials(DataBase dataBase)
    {
        if (string.IsNullOrEmpty(MaterialsListInput)) return;
        string[] temps;
        temps = MaterialsListInput.Split(';');
        foreach(string temp in temps)
        {
            string[] info = temp.Split(',');
            /*Debug.Log(info[0]);
            Debug.Log(info[1]);*/
            AddMaterial(new MaterialInfo(dataBase.GetItem(info[0]), int.Parse(info[1])));
        }
    }

    /// <summary>
    /// 克隆相同信息的物品。
    /// 因为类是引用类型，为了避免引用的原始信息被更改，不妨克隆一个新的对象
    /// </summary>
    /// <returns></returns>
    public virtual ItemBase Clone()
    {
        return new ItemBase(ID, Name, Description, Icon, MaxCount, Weight, BuyPrice, SellPrice, SellAble, Usable)
        {
            MaterialsList = this.MaterialsList,
            MaterialsListInput = this.MaterialsListInput,
            StackAble = this.StackAble
        };
    }
}
