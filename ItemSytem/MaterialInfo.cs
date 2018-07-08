using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInfo  {
    public ItemBase Material;
    public int Required;
    public bool IsEnough;
    public string State;

    public MaterialInfo(ItemBase material, int required)
    {
        Material = material;
        Required = required;
        IsEnough = false;
        State = string.Empty;
    }

    public bool Check(BagInfo bag, int multiple)
    {
        if (Material == null) return false;
        ItemInfo find = bag.itemList.Find(i => i.ItemID == Material.ID);
        if (find != null)
        {
            State = Material.Name + find.Quantity + "/" + Required + "\n";
            if (find.Quantity / (Required * multiple) > 0)
            {
                IsEnough = true;
                return true;
            }
            else
            {
                IsEnough = false;
                return false;
            }
        }
        State = Material.Name + "0/" + Required + "\n";
        IsEnough = false;
        return false;
    }
}
