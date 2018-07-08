using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoShower : MonoBehaviour
{
    public Text info;
    public Image overweight;
    public Image maxWeight;

    // Update is called once per frame
    void Update()
    {
        if (PlayerInfoManager.Instance != null) ShowInfo();
    }

    public void ShowInfo()
    {
        info.text =
             "<size=40>" + PlayerInfoManager.Instance.PlayerInfo.CharacterName + "</size>\n"
             + PlayerInfoManager.Instance.PlayerInfo.Level + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Current_Exp + "/" + PlayerInfoManager.Instance.PlayerInfo.NextExp + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Li + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Ti + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Qi + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Ji + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Min + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Current_HP + "/" + PlayerInfoManager.Instance.PlayerInfo.HP + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Current_MP + "/" + PlayerInfoManager.Instance.PlayerInfo.MP + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Current_Endurance + "/" + PlayerInfoManager.Instance.PlayerInfo.Endurance + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Neili + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.ATK + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.DEF + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Hit.ToString("F2") + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Dodge.ToString("F2") + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Crit.ToString("F2") + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Res_Rigidity.ToString("F2") + "\n"
             //+ PlayerInfoManager.Self.PlayerInfo.Res_Repulsed.ToString("F2") + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Res_Stuned.ToString("F2") + "\n"
             //+ PlayerInfoManager.Self.PlayerInfo.Res_Floated.ToString("F2") + "\n"
             //+ PlayerInfoManager.Self.PlayerInfo.Res_Blowed.ToString("F2") + "\n"
             + PlayerInfoManager.Instance.PlayerInfo.Res_Falled.ToString("F2") + "\n";
        /*+ "是否装备了戒指1\t" + PlayerInfoManager.Self.playerInfo.equipments.IsRgEquip_1 + "\n"
        + "是否装备了戒指2\t" + PlayerInfoManager.Self.playerInfo.equipments.IsRgEquip_2 + "\n"
        + "行囊中物品的总重量\t" + PlayerInfoManager.Self.playerInfo.bag.Current_Weight + "\n"
        + "行囊中不同物品的数量\t" + PlayerInfoManager.Self.playerInfo.bag.Current_Size + "\n"
        + "行囊中不同种类的数量\t" + PlayerInfoManager.Self.playerInfo.bag.itemList.Count + "\n"
        + "行囊是否过重\t" + PlayerInfoManager.Self.playerInfo.bag.IsOverweight + "\n"
        + "行囊是否超重\t" + PlayerInfoManager.Self.playerInfo.bag.IsMaxWeight + "\n";*/
    }
}
