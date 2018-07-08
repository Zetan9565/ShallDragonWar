using UnityEngine;

public class DropItemInfo {

    /// <summary>
    /// 剩余量
    /// </summary>
    int left;
    public int Left
    {
        get
        {
            return left;
        }
        set
        {
            if (value < 0) left = 0;
            else if (value > MaxProduct) left = MaxProduct;
            else left = value;
        }
    }
    public int MaxProduct;
    public bool StackAble;
    public ItemBase Item;
    public string ItemID;

    public DropItemInfo(ItemBase item, int maxproduct)
    {
        Item = item;
        ItemID = item.ID;
        StackAble = item.StackAble;
        MaxProduct = Random.Range(1, maxproduct);
        Left = MaxProduct;
    }

    public int TryPickUp(BagInfo bag, int pick_num)
    {
        int finalltPick = CheckMaxGet(bag);
        Left -= pick_num > finalltPick ? finalltPick : pick_num;
        return pick_num > finalltPick ? finalltPick : pick_num;
    }

    public int TryPickUpAll(BagInfo bag)
    {
        int finallyPick = CheckMaxGet(bag);
        //Debug.Log(finallyPick);
        Left -= finallyPick;
        return finallyPick;
    }

    #region 检查拾取量
    /// <summary>
    /// 仅用于检查掉落物品的拾取量，
    /// 进行这一步是为了避免物品的拾取不符合背包的设计逻辑
    /// </summary>
    /// <param name="bag">用于检查的背包数据</param>
    /// <returns>最大拾取量</returns>
    public int CheckMaxGet(BagInfo bag)
    {
        ItemInfo info = bag.itemList.Find(i => i.ItemID == ItemID);
        int maxGet = 0;
        if (info == null)
        {
            maxGet = bag.IsMax ? 0 : StackAble ? Left : bag.MaxSize - bag.Current_Size > Left ? Left : bag.MaxSize - bag.Current_Size;
            if (maxGet == 0) return 0;
            if ((bag.Current_Weight + Item.Weight * maxGet) / bag.MaxWeight > 1.5f)
            {
                int i = 0;
                while (i < maxGet)
                {
                    if ((bag.Current_Weight + Item.Weight * i) / bag.MaxWeight <= 1.5f && ((bag.Current_Weight + Item.Weight * (i + 1)) / bag.MaxWeight > 1.5f))
                    {
                        maxGet = i;
                        break;
                    }
                    i++;
                }
            }
        }
        else
        {
            maxGet = StackAble ?
 ((info.MaxCount - info.Quantity > Left) ?
         Left
        : (info.MaxCount - info.Quantity))
          : bag.IsMax ? 0 : bag.MaxSize - bag.Current_Size > Left ? Left : bag.MaxSize - bag.Current_Size;
            if (maxGet == 0) return 0;
            if ((bag.Current_Weight + Item.Weight * maxGet) / bag.MaxWeight > 1.5f)
            {
                int i = 0;
                while (i < maxGet)
                {
                    if ((bag.Current_Weight + Item.Weight * i) / bag.MaxWeight <= 1.5f && ((bag.Current_Weight + Item.Weight * (i + 1)) / bag.MaxWeight > 1.5f))
                    {
                        maxGet = i;
                        break;
                    }
                    i++;
                }
            }
        }
        return maxGet;
    }
    #endregion
}
