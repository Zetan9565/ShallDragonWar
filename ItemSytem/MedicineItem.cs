using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
using System.Text;

public class MedicineItem : ItemBase
{

    public int HP_Rec;
    public int MP_Rec;
    public int Endurance_Rec;
    public MedicineItem(string id, string name, string description, string icon, int max_c, float weight, int bprice, int sprice, bool sellable, bool enable, int hp_rev, int mp_rec, int endurance_rec) :
        base(id, name, description, icon, max_c, weight, bprice, sprice, sellable, enable)
    {
        /*if(icon.Contains("Icon/Item/Medicine/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Medicine/", "");
            icon = sb.ToString();
        }
        Icon = "Icon/Item/Medicine/" + icon;*/
        HP_Rec = hp_rev;
        MP_Rec = mp_rec;
        Endurance_Rec = endurance_rec;
        ItemType = ItemType.Medicine;
        ItemTypeName = "药物";
        StackAble = true;
    }

    public override void OnUsed(PlayerInfo playerInfo)
    {
        Used(playerInfo);
    }

    public void Used(PlayerInfo playerInfo)
    {
        playerInfo.Current_HP += HP_Rec;
        playerInfo.Current_MP += MP_Rec;
        playerInfo.Current_Endurance += Endurance_Rec;
        playerInfo.bag.Current_Weight -= Weight;
        ItemInfo find = playerInfo.bag.itemList.Find(i => i.ItemID == ID);
        find.Quantity--;
        if (find.Quantity == 0)
        {
            playerInfo.bag.Current_Size--;
            playerInfo.bag.itemList.Remove(find);
        }
    }

    public override ItemBase Clone()
    {
        return new MedicineItem(this.ID, this.Name, this.Description, this.Icon, this.MaxCount, this.Weight,
    this.BuyPrice, this.SellPrice, this.SellAble, this.Usable, this.HP_Rec, this.MP_Rec, this.Endurance_Rec)
        {
            MaterialsList = this.MaterialsList,
            StackAble = this.StackAble
        };
    }
}
