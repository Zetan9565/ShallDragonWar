using System.Collections.Generic;
using System.IO;
using MyEnums;

public class BagInfo {
    public int MaxSize;
    private int current_Size;
    public int Current_Size
    {
        get { return current_Size; }
        set
        {
            if (value > MaxSize) current_Size = MaxSize;
            else if (value < 0) current_Size = 0;
            else current_Size = value;
        }
    }
    public float MaxWeight;
    private float current_Weight;
    public float Current_Weight
    {
        get { return current_Weight; }
        set
        {
            if (value < 0) current_Weight = 0;
            else current_Weight = value;
        }
    }
    private int money;
    public int Money
    {
        get { return money; }
        set
        {
            if (value >= 0) money = value;
            else money = 0;
        }
    }

    public List<ItemInfo> itemList;

    public bool IsMax;
    public bool IsOverweight;
    public bool IsMaxWeight;

    public readonly static string DataName = "/BagInfoData.zetan";

    public BagInfo(int maxsize, float maxweight)
    {
        MaxSize = maxsize;
        Current_Size = 0;
        MaxWeight = maxweight;
        Current_Weight = 0;
        Money = 0;
        itemList = new List<ItemInfo>();
    }

    public void GetItem(ItemBase item, int get_num)
    {
        if (item == null || get_num <= 0) return;
        int finallyGet = 0;
        ItemInfo find = itemList.Find(i => i.ItemID == item.ID);
        //Debug.Log(find);
        if (find != null && item.StackAble)
        {
            finallyGet = find.MaxCount - find.Quantity > get_num ? get_num : find.MaxCount - find.Quantity;
            if (finallyGet != 0 || !find.IsMax)
            {
                if (!IsMaxWeight)
                {
                    if (!((Current_Weight + item.Weight * finallyGet) / MaxWeight > 1.5f))
                    {
                        find.Quantity += finallyGet;
                        if (find.Quantity >= find.MaxCount)
                            find.IsMax = true;
                        Current_Weight += item.Weight * finallyGet;
                        CheckSizeAndWeight();
                    }
                    else throw new System.Exception("该物品太重了");
                }
                else throw new System.Exception("已经超重了");
            }
            else throw new System.Exception("该物品可持有量已达最大");
        }
        else if (!item.StackAble)
        {
            finallyGet = MaxSize - Current_Size > get_num ? get_num : MaxSize - Current_Size;
            if (!IsMaxWeight)
            {
                if (!((Current_Weight + item.Weight * finallyGet) / MaxWeight > 1.5f))
                {
                    if (!IsMax)
                    {
                        for (int i = 0; i < finallyGet; i++)
                        {
                            Current_Size++;
                            Current_Weight += item.Weight;
                            ItemInfo info = new ItemInfo(item.Clone())
                            {
                                Quantity = 1
                            };
                            itemList.Add(info);
                            CheckSizeAndWeight();
                        }
                    }
                    else throw new System.Exception("行囊已满");
                }
                else throw new System.Exception("该物品太重了");
            }
            else throw new System.Exception("已经超重了");
        }
        else
        {
            finallyGet = item.MaxCount > get_num ? get_num : item.MaxCount;
            if(!IsMaxWeight)
            {
                if (!((Current_Weight + item.Weight * finallyGet) / MaxWeight > 1.5f))
                {
                    if(!IsMax)
                    {
                        Current_Size++;
                        Current_Weight += item.Weight * finallyGet;
                        ItemInfo info = new ItemInfo(item.Clone());
                        info.Quantity += finallyGet;
                        if (info.Quantity >= info.MaxCount) info.IsMax = true;
                        itemList.Add(info);
                        CheckSizeAndWeight();
                    }
                    else throw new System.Exception("行囊已满");
                }
                else throw new System.Exception("该物品太重了");
            }
            else throw new System.Exception("已经超重了");
        }
    }

    public void LoseItem(ItemBase item, int lose_num)
    {
        if (item == null || lose_num <= 0) return;
        if (Current_Size <= 0) throw new System.Exception("背包为空");
        ItemInfo tempitem = itemList.Find(i => i.Item == item);
        if (tempitem == null) throw new System.Exception("该物品未在行囊中");
        bool isequip = false;
        switch (tempitem.Item.ItemType)
        {
            case ItemType.Weapon:
                WeaponItem weapon = tempitem.Item as WeaponItem;
                if (weapon == null) break;
                isequip = weapon.IsEqu;
                break;
            case ItemType.Armor:
                ArmorItem armor = tempitem.Item as ArmorItem;
                if (armor == null) break;
                isequip = armor.IsEqu;
                break;
            case ItemType.Jewelry:
                JewelryItem jewelry = tempitem.Item as JewelryItem;
                if (jewelry == null) break;
                isequip = jewelry.IsEqu;
                break;
            case ItemType.Mount:
                MountItem mount = tempitem.Item as MountItem;
                if (mount == null) break;
                isequip = mount.IsEqu;
                break;
        }
        if (isequip) throw new System.Exception("该物品已装备");
        if (tempitem.Quantity <= 0) throw new System.Exception("该物品为空");
        int finallyDiscard = tempitem.StackAble ? tempitem.Quantity - lose_num > 0 ? lose_num : tempitem.Quantity : 1;
        tempitem.Quantity -= finallyDiscard;
        Current_Weight -= tempitem.Item.Weight * finallyDiscard;
        if (tempitem.Quantity <= 0)
        {
            Current_Size -= 1;
            itemList.Remove(tempitem);
        }
        CheckSizeAndWeight();
    }

    public void GetMoney(int money)
    {
        if (money <= 0) return;
        Current_Weight += money * 0.0001f;
        Money += money;
        CheckSizeAndWeight();
    }

    public void LoseMoney(int money)
    {
        if (money <= 0) return;
        if (Money < money) throw new System.Exception("金钱不足");
        Current_Weight -= money * 0.0001f;
        Money -= money;
        CheckSizeAndWeight();
    }

    public void CheckSizeAndWeight()
    {
        IsMax = Current_Size >= MaxSize;
        IsOverweight = Current_Weight / MaxWeight > 1.0f;
        IsMaxWeight = Current_Weight / MaxWeight > 1.5f;
    }

    /*public void GetItemByList(List<ItemInfo> itemslist)
    {
        if (itemslist == null) return;
        foreach (ItemInfo item in itemslist)
            if (item != null)
                GetItem(item.Item, item.Quantity);
    }*/

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
        File.WriteAllLines(path + "/BagInfoData.zetan", EncryptStings.ToArray(), System.Text.Encoding.UTF8);
    }

    public void Load(string path, string key = "", bool dencrypt = false)
    {
        string[] encrypts = File.ReadAllLines(path + "/BagInfoData.zetan", System.Text.Encoding.UTF8);
        List<string> LoadStrings = new List<string>();
        foreach (string s in encrypts)
        {
            if (dencrypt && key.Length == 32) LoadStrings.Add(Encryption.Dencrypt(s, key));
            else LoadStrings.Add(s);
            //UnityEngine.Debug.Log(s);
        }
        string[] jsons = LoadStrings.ToArray();
        for (int i = 0; i < itemList.Count; i++)
            itemList[i].Load(jsons[i]);
    }

}
