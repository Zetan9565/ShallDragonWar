using MyEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemListAgent : MonoBehaviour
{
    public List<DropItemInfo> dropItemList;

    public void GetDropItems(string[] dropItemsInput)
    {
        if (dropItemsInput == null) return;
        dropItemList = new List<DropItemInfo>();
        foreach (string dropitem in dropItemsInput)
        {
            string[] dropinfo = dropitem.Split(',');
            float rate = 0, tempf;
            if (float.TryParse(dropinfo[2], out tempf)) rate = tempf;
            if (MyTools.Probability(rate))
            {
                ItemBase item = DataBase.Instance.GetItem(dropinfo[0]);
                if (item == null) return;
                //Debug.Log(item.Icon);
                int maxcount = 1, tempi;
                if (int.TryParse(dropinfo[1], out tempi)) maxcount = tempi;
                if (item.StackAble)
                {
                    dropItemList.Add(new DropItemInfo(item, tempi));
                }
                else
                {
                    for (int i = 0; i < maxcount; i++)
                        dropItemList.Add(new DropItemInfo(item, 1));
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (PickUpManager.Instance && PickUpManager.Instance.dropItemListAgent && PickUpManager.Instance.dropItemListAgent == this)
            PickUpManager.Instance.CantPick();
    }
}
