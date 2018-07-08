using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MountItem :ItemBase {
    public MountInfo mountInfo;
    public bool IsEqu;

    public MountItem(string id, string name, string description, string icon, int max_c, float weight, int bprice,int sprice,bool sellable, bool enable, MountInfo info) :
        base(id, name, description, icon, max_c, weight,bprice,sprice,sellable, enable)
    {
        /*if (icon.Contains("Icon/Item/Mount/"))
        {
            StringBuilder sb = new StringBuilder(icon);
            sb.Replace("Icon/Item/Mount/", "");
            icon = sb.ToString();
        }
        Icon = "Icon/Item/Mount/" + icon;*/
        mountInfo = info;
        StackAble = false;
    }

    public override void OnUsed(PlayerInfo playerInfo)
    {
        //Equip(playerInfo);
    }

    public void Unequip(GameObject acted_go)
    {
        mountInfo.OnDismount(acted_go.GetComponent<PlayerInfoManager>().PlayerInfo);
        IsEqu = false;
    }

    public override ItemBase Clone()
    {
        return new MountItem(this.ID, this.Name, this.Description, this.Icon, this.MaxCount, this.Weight,
            this.BuyPrice, this.SellPrice, this.SellAble, this.Usable, this.mountInfo)
        {
            Icon = this.Icon,
            IsEqu = this.IsEqu
        };
    }
}
