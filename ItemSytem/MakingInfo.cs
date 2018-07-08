using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingInfo {
    public ItemBase Item;
    public int Cost;
    public bool MakeAble;
    public List<string> infos;
    public int MaxMake;

    public MakingInfo(ItemBase item,int cost)
    {
        Item = item;
        MakeAble = false;
        Cost = cost;
    }

    public bool Check(BagInfo bag, int num)
    {
        MakeAble = Item.CheckMaterials(bag, num, out infos);
        return MakeAble;
    }

    public int CheckMaxMake(BagInfo bag)
    {
        List<int> nums = new List<int>();
        foreach (MaterialInfo minfo in Item.MaterialsList)
        {
            ItemInfo find = bag.itemList.Find(i => i.ItemID == minfo.Material.ID);
            if (find != null)
            {
                nums.Add(find.Quantity / minfo.Required);
            }
            else nums.Add(0);
        }
        nums.Sort((x, y) => {
            if (x < y) return 1;
            else if (x > y) return -1;
            else return 0;
        });
        MaxMake = nums[0];
        ItemInfo find2 = bag.itemList.Find(i => i.ItemID == Item.ID);
        if (find2 != null)
        {
            if (find2.IsMax) MaxMake = 0;
            if (MaxMake <= 0) return 0;
            MaxMake = find2.MaxCount - find2.Quantity > MaxMake ? find2.StackAble ? MaxMake : find2.MaxCount - find2.Quantity : 1;
        }
        else MaxMake = Item.MaxCount > MaxMake ? MaxMake : Item.MaxCount;
        return MaxMake;
    }

    public void Make(BagInfo bag, int num)
    {
        if (num <= 0) return;
        CheckMaxMake(bag);
        if (num > MaxMake) throw new System.Exception("无法制作该数量的该物品");
        if (bag.Money < Cost * num) throw new System.Exception("制作费用不足");
        if (bag.IsMax) throw new System.Exception("行囊空间不足");
        if (bag.IsMaxWeight) throw new System.Exception("超重状态无法制作");
        if ((bag.Current_Weight + Item.Weight * num) / bag.MaxWeight >= 1.5f) throw new System.Exception("该物品太重了");
        Check(bag, num);
        if (!MakeAble) throw new System.Exception("材料不足，无法制作");
        foreach (MaterialInfo minfo in Item.MaterialsList)
        {
            ItemBase find = bag.itemList.Find(m => m.ItemID == minfo.Material.ID).Item;
            if (find == null) throw new System.Exception("找不到该材料");
            bag.LoseItem(find, minfo.Required * num);
        }
        bag.GetItem(Item, num);
        bag.LoseMoney(Cost * num);
    }
}
