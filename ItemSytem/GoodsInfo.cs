using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsInfo {
    public ItemBase Item;
    public int MaxNum;
    int numForSell;
    public int NumForSell
    {
        get { return numForSell; }
        set
        {
            if (value < 0) numForSell = 0;
            else if (value > MaxNum) numForSell = MaxNum;
            else numForSell = value;
        }
    }
    public bool isSoldOut;
    public bool sellOutAble;
    public int Price;

    public GoodsInfo(ItemBase item, int max, bool sellOut_able, int num_for_sell)
    {
        Item = item.Clone();
        MaxNum = max;
        sellOutAble = sellOut_able;
        NumForSell = num_for_sell;
        Price = item.BuyPrice;
    }

    public void OnSell(int sell_num,BagInfo bag)
    {
        if (sell_num <= 0) return;
        if (isSoldOut) throw new System.Exception("已售罄");
        if (sellOutAble)
        {
            int finallySell = NumForSell > sell_num ? sell_num : NumForSell;
            NumForSell -= Item.OnBuy(bag, finallySell);
            //Debug.Log(NumForSell);
            if (NumForSell <= 0) isSoldOut = true;
        }
        else
        {
            Item.OnBuy(bag, sell_num);
        }
    }

    public void Replenish(int rep_num)
    {
        if (rep_num <= 0 || NumForSell >= MaxNum || !sellOutAble) return;
        NumForSell += rep_num;
        if (NumForSell > 0) isSoldOut = false;
    }
}
