using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.IO;

public class WarehouseInfo{

    public readonly static string DataName = "/WarehouseInfoData.zetan";

    public int MaxSize;
    private int current_Size;
    public int Current_Size
    {
        get
        {
            return current_Size;
        }
        set
        {
            if (value > MaxSize) current_Size = MaxSize;
            else if (value < 0) current_Size = 0;
            else current_Size = value;
        }
    }
    public bool IsMax;

    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            if (value < 0) money = 0;
            else money = value;
        }
    }

    public List<ItemInfo> itemList;

    public WarehouseInfo(int max_size)
    {
        MaxSize = max_size;
        Current_Size = 0;
        IsMax = false;
        itemList = new List<ItemInfo>();
    }

    public void StoreItem(ItemBase item, BagInfo bagInfo, int store_num)
    {
        if (item == null || store_num <= 0) return;
        ItemInfo itemInBag = bagInfo.itemList.Find(i => i.Item == item);
        if (itemInBag == null) throw new System.Exception("背包中不存在该物品");
        int finallyStore = store_num;
        ItemInfo find = itemList.Find(i => i.ItemID == item.ID);
        //Debug.Log(find);
        if (find != null && item.StackAble)
        {
            if (finallyStore != 0)
            {
                find.Quantity += finallyStore;
            }
        }
        else if (!item.StackAble)
        {
            finallyStore = MaxSize - Current_Size > store_num ? store_num : MaxSize - Current_Size;
            if (!IsMax)
            {
                for (int i = 0; i < finallyStore; i++)
                {
                    Current_Size++;
                    ItemInfo info = new ItemInfo(item.Clone())
                    {
                        Quantity = 1
                    };
                    itemList.Add(info);
                }
            }
            else throw new System.Exception("仓库已满");
        }
        else
        {
            finallyStore = item.MaxCount > store_num ? store_num : item.MaxCount;
            if (!IsMax)
            {
                Current_Size++;
                ItemInfo info = new ItemInfo(item.Clone());
                info.Quantity += finallyStore;
                if (info.Quantity >= info.MaxCount) info.IsMax = true;
                itemList.Add(info);
            }
            else throw new System.Exception("仓库已满");
        }
        bagInfo.LoseItem(item, finallyStore);
    }

    public int TakeOutItem(ItemInfo item, BagInfo bagInfo, int take_num)
    {
        if (item == null || take_num <= 0) return 0;
        if (Current_Size <= 0) throw new System.Exception("仓库为空");
        ItemInfo tempitem = itemList.Find(i => i.Item == item.Item);
        if (tempitem == null) throw new System.Exception("该物品未在仓库中");
        if (tempitem.Quantity <= 0) throw new System.Exception("该物品为空");
        int finallyTake = tempitem.StackAble ? tempitem.Quantity - take_num > 0 ? take_num : tempitem.Quantity : 1;
        ItemInfo itemInBag = bagInfo.itemList.Find(i => i.ItemID ==item.ItemID);
        int temp = 0;
        if (itemInBag != null) temp = itemInBag.Quantity;
        bagInfo.GetItem(item.Item, finallyTake);
        itemInBag = bagInfo.itemList.Find(i => i.ItemID == item.ItemID);
        temp = itemInBag.Quantity - temp;
        finallyTake = finallyTake > temp ? temp : finallyTake;
        tempitem.Quantity -= finallyTake;
        if (tempitem.Quantity <= 0)
        {
            Current_Size -= 1;
            itemList.Remove(tempitem);
        }
        if (Current_Size < MaxSize) IsMax = false;
        return finallyTake;
    }

    /*public int CheckFinallyTake(BagInfo bag, int buy_num)
    {
        if (buy_num <= 0) return 0;
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
    }*/

    public void StoreMoney(int money)
    {
        if (money <= 0) return;
        Money += money;
    }

    public void WithdrawMoney(int money)
    {
        if (money <= 0) return;
        if (Money < money) throw new System.Exception("金钱不足");
        Money -= money;
    }

    public void Sort()
    {
        itemList.Sort((i1, i2) =>
        {
            string type1 = i1.ItemID.Substring(0, 2);
            string type2 = i2.ItemID.Substring(0, 2);
            if (type1.Equals(type2))
            {
                int id1 = int.Parse(i1.ItemID.Substring(2));
                int id2 = int.Parse(i2.ItemID.Substring(2));
                if (id1 > id2) return 1;
                else if (id1 < id2) return -1;
                else return 0;
            }
            else
            {
                if (type1 == "WP") return -1;
                else if (type2 == "WP") return 1;
                else if (type1 == "AR") return -1;
                else if (type2 == "AR") return 1;
                else if (type1 == "JW") return -1;
                else if (type2 == "JW") return 1;
                else if (type1 == "ME") return -1;
                else if (type2 == "ME") return 1;
                else if (type1 == "MA") return -1;
                else if (type2 == "MA") return 1;
                else return 0;
            }
        });
    }

    public void Save(string path, string key = "", bool encrypt = false)
    {
        List<string> SaveStrings = new List<string>();
        foreach (ItemInfo info in itemList)
            info.Save(SaveStrings);
        List<string> EncryptStings = new List<string>();
        foreach (string s in SaveStrings)
        {
            if (encrypt && key.Length == 32) EncryptStings.Add(Encryption.Encrypt(s, key));
            else EncryptStings.Add(s);
        }
        File.WriteAllLines(path + "/WarehouseInfoData.zetan", EncryptStings.ToArray(), System.Text.Encoding.UTF8);
    }

    public void Load(string path, string key = "", bool dencrypt = false)
    {
        string[] encrypts = File.ReadAllLines(path + "/WarehouseInfoData.zetan", System.Text.Encoding.UTF8);
        List<string> LoadStrings = new List<string>();
        foreach (string s in encrypts)
        {
            if (dencrypt && key.Length == 32) LoadStrings.Add(Encryption.Dencrypt(s, key));
            else LoadStrings.Add(s);
        }
        string[] jsons = LoadStrings.ToArray();
        for (int i = 0; i < itemList.Count; i++)
            itemList[i].Load(jsons[i]);
    }
}
