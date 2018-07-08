using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyEnums;

public class GiveTestItems : MonoBehaviour
{
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button Button5;
    public Button Button6;
    public Button Button7;
    public Button Button8;
    public Button Button9;
    public Button Button10;
    public Button Button11;
    public Button Button12;
    //MedicineItem medicine = new MedicineItem(444, "止血草", "药铺的抢手货", "Grass", 15, 0.05f, 0, 0, true, true, 100, 100, 110);

    // Use this for initialization
    void Start()
    {
        Button1.onClick.AddListener(delegate () { GiveItems(1); });
        Button2.onClick.AddListener(delegate () { GiveItems(2); });
        Button3.onClick.AddListener(delegate () { GiveItems(3); });
        Button4.onClick.AddListener(delegate () { GiveItems(4); });
        Button5.onClick.AddListener(delegate () { GiveItems(5); });
        Button6.onClick.AddListener(delegate () { GiveItems(6); });
        Button7.onClick.AddListener(delegate () { GiveItems(7); });
        Button8.onClick.AddListener(delegate () { GiveItems(8); });
        Button9.onClick.AddListener(delegate () { GiveItems(9); });
        Button10.onClick.AddListener(delegate () { GiveItems(10); });
        Button11.onClick.AddListener(delegate () { GiveItems(11); });
        Button12.onClick.AddListener(delegate () { GiveItems(12); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GiveItems(int index)
    {
        switch (index)
        {
            case 1: BagManager.Instance.GetItem(new WeaponItem("WP0132", "铁戟", "力大之人方可随意舞弄的长杆武器", "Icon/Item/Weapon/Halberd/Fangtian", 99, 50.0f, 0, 0, false, true, WeaponType.Halberd, 250,null), 1); break;
            case 2: BagManager.Instance.GetItem(new WeaponItem("WP0133", "直枪", "以速度致胜的轻型长枪", "Icon/Item/Weapon/Spear/Spear", 99, 30.0f, 0, 0, false, true, WeaponType.Spear, 220,null), 1); break;
            case 3: BagManager.Instance.GetItem(new WeaponItem("WP0134", "匕首", "藏于袖内的预留防身武器", "Icon/Item/Weapon/Kinfe/Knife", 99, 10.0f, 0, 0, false, true, WeaponType.Knife, 50,null), 1); break;
            case 4: BagManager.Instance.GetItem(new ArmorItem("AR0223", "铁甲", "坚硬的铁制铠甲", "Icon/Item/Armor/Clothes/Clothes", 99, 15.0f, 0, 0, false, true, ArmorType.Clothes, 25, DataBase.Instance.GetSuitEffectInfo("SET001")), 1); break;
            case 5: BagManager.Instance.GetItem(new ArmorItem("AR0224", "铁冠", "坚硬的铁制束发冠", "Icon/Item/Armor/Helmet/Helmet", 99, 5.0f, 0, 0, false, true, ArmorType.Helmet, 10, DataBase.Instance.GetSuitEffectInfo("SET001")), 1); break;
            case 6: BagManager.Instance.GetItem(new ArmorItem("AR0225", "铁护腕", "坚硬的铁制护腕", "Icon/Item/Armor/WristBand/WristBand", 99, 10.0f, 0, 0, false, true, ArmorType.WristBand, 15, DataBase.Instance.GetSuitEffectInfo("SET001")), 1); break;
            case 7: BagManager.Instance.GetItem(new ArmorItem("AR0226", "铁靴", "坚硬的铁制战靴", "Icon/Item/Armor/Shoes/shan", 99, 10.0f, 0, 0, false, true, ArmorType.Shoes, 10, DataBase.Instance.GetSuitEffectInfo("SET001")), 1); break;
            case 8: BagManager.Instance.GetItem(new JewelryItem("JW0330", "项链", "集市上常见的项链", "Icon/Item/Jewelry/Necklace/Necklace", 99, 0.1f, 0, 0, false, true, JewelryType.Necklace, new PowerUps(20, 20, 20, 20, 20, 20, 20, 20), null), 1); break;
            case 9: BagManager.Instance.GetItem(new JewelryItem("JW0331", "腰饰", "集市上常见的腰饰", "Icon/Item/Jewelry/Belt/Belt", 99, 0.2f, 0, 0, false, true, JewelryType.Belt, new PowerUps(20, 20, 20, 20, 20, 20, 20, 20),null), 1); break;
            case 10: BagManager.Instance.GetItem(new JewelryItem("JW0332", "戒指", "集市上常见的戒指", "Icon/Item/Jewelry/Ring/Ring", 99, 0.1f, 0, 0, false, true, JewelryType.Ring, new PowerUps(20, 20, 20, 20, 20, 20, 20, 20),null), 2); break;
            case 11: PlayerInfoManager.Instance.PlayerInfo.bag.GetMoney(500); break;
            case 12: PlayerInfoManager.Instance.LevelUp(39); break;
        }
    }

}
