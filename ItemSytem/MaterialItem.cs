using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.Text;

public class MaterialItem :ItemBase {
    public MaterialItem(string id, string name, string description, string icon, int max_c, float weight, int bprice, int sprice, bool sellable, bool usable) :
        base(id, name, description, icon, max_c, weight, bprice, sprice, sellable, usable)
    {
        /*if (icon.Contains("Icon/Item/Material/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Material/", "");
            icon = sb.ToString();
        }
        Icon = "Icon/Item/Material/" + icon;*/
        ItemType = ItemType.Material;
        ItemTypeName = "材料";
        StackAble = true;
    }

    public override ItemBase Clone()
    {
        return new MaterialItem(this.ID, this.Name, this.Description, this.Icon, this.MaxCount, this.Weight,
    this.BuyPrice, this.SellPrice, this.SellAble, this.Usable)
        {
            MaterialsList = this.MaterialsList,
            StackAble = this.StackAble
        };
    }
}
