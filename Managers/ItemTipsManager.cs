using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyEnums;
using System;

[DisallowMultipleComponent]
public class ItemTipsManager : MonoBehaviour
{
    public static ItemTipsManager Instance;
    public GameObject UI;

    public enum ItemTipsType
    {
        BagItem,
        WerehouseItem,
        EuipmentItem,
        GoodsItem,
        BuyerItem,
        MakingItem,
        DropItem
    }
    [Space]
    public ItemTipsType TipsType;
    [Space]
    public Image Icon;
    public Text Name;
    public Text Effect;
    public Text itemType;
    public Text priceType;
    public Text Price;
    public Text Description;
    public Text Weight;
    public GameObject EnchantOrMaterialsList;
    public Text ListText;
    public GameObject SuitEffect;
    public Text SuitText;
    [Header("按钮集合")]
    public Button UsedButton;
    public Button EquipButton;
    public Button UnequipButton;
    public Button DiscardButton;
    public Button PickUpButton;
    public Button BuyButton;
    public Button SellButton;
    public Button MakingButton;
    public Button StoreButton;
    public Button TakeOutButton;
    public Button HotKeyButton;
    [Header("对象集合")]
    public ItemAgent itemAgent;
    public ItemBase equipInfo;
    public DropItemAgent dropItemAgent;
    public GoodsAgent goodsAgent;
    public BuyerItemAgent buyerItemAgent;
    public MakingAgent makingAgent;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        UsedButton.onClick.AddListener(OnUsedButtonClick);
        EquipButton.onClick.AddListener(OnEquipButtonClick);
        PickUpButton.onClick.AddListener(OnPickUpButtonClick);
        BuyButton.onClick.AddListener(OnBuyButtonClick);
        SellButton.onClick.AddListener(OnSellButtonClick);
        MakingButton.onClick.AddListener(OnMakingButtonClick);
        DiscardButton.onClick.AddListener(OnDiscardButtonClick);
        StoreButton.onClick.AddListener(OnStoreButtonClick);
        TakeOutButton.onClick.AddListener(OnTakeOutButtonClick);
        HotKeyButton.onClick.AddListener(OnHotKeyButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (!itemAgent && !dropItemAgent && equipInfo == null && !goodsAgent && !buyerItemAgent && !makingAgent) CloseUI();
    }

    public void ShowItemInfo()
    {
        CheckBottonStatu();
        ItemBase Item;
        if (TipsType == ItemTipsType.BagItem || TipsType == ItemTipsType.WerehouseItem)
        {
            Item = itemAgent.itemInfo.Item;
            Icon.overrideSprite = itemAgent.iconImage;
        }
        else if (TipsType == ItemTipsType.DropItem)
        {
            Item = dropItemAgent.dropItemInfo.Item;
            Icon.overrideSprite = dropItemAgent.iconImage;
        }
        else
        {
            Item = equipInfo;
            Icon.overrideSprite = Resources.Load(Item.Icon, typeof(Sprite)) as Sprite;
        }
        Name.text = Item.Name;
        switch (Item.ItemType)
        {
            case ItemType.Weapon:
                WeaponItem weapon = Item as WeaponItem;
                if (weapon == null) break;
                Effect.text = "攻击力" + weapon.ATK.ToString("F0");
                itemType.text = weapon.ItemTypeName + "/" + weapon.type_name;
                if (TipsType != ItemTipsType.EuipmentItem && TipsType != ItemTipsType.DropItem)
                    if (!weapon.Usable)
                    {
                        MyTools.SetActive(UsedButton.gameObject, false);
                        MyTools.SetActive(EquipButton.gameObject, false);
                        MyTools.SetActive(DiscardButton.gameObject, true);
                        MyTools.SetActive(HotKeyButton.gameObject, false);
                    }
                    else
                    {
                        MyTools.SetActive(UsedButton.gameObject, false);
                        MyTools.SetActive(EquipButton.gameObject, true);
                        MyTools.SetActive(DiscardButton.gameObject, true);
                        MyTools.SetActive(HotKeyButton.gameObject, true);
                    }
                if (weapon.enchant == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                else
                {
                    EnchantOrMaterialsList.GetComponent<Text>().text = "淬火属性";
                    ListText.text =
                        (weapon.enchant.powerUps.ATK_Up != 0 ? "攻击力" + weapon.enchant.powerUps.ATK_Up.ToString("F0") + "\n" : "")
                        + (weapon.enchant.powerUps.DEF_Up != 0 ? "防御力" + weapon.enchant.powerUps.DEF_Up.ToString("F0") + "\n" : "")
                        + (weapon.enchant.powerUps.HP_Up != 0 ? "体力" + weapon.enchant.powerUps.HP_Up.ToString("F0") + "\n" : "")
                        + (weapon.enchant.powerUps.MP_Up != 0 ? "真气" + weapon.enchant.powerUps.MP_Up.ToString("F0") + "\n" : "")
                        + (weapon.enchant.powerUps.Endurance_Up != 0 ? "耐力" + weapon.enchant.powerUps.Endurance_Up.ToString("F0") + "\n" : "")
                        + (weapon.enchant.powerUps.Hit_Up != 0 ? "命中" + weapon.enchant.powerUps.Hit_Up.ToString("F2") + "\n" : "")
                        + (weapon.enchant.powerUps.Dodge_Up != 0 ? "闪避" + weapon.enchant.powerUps.Dodge_Up.ToString("F2") + "\n" : "")
                        + (weapon.enchant.powerUps.Crit_Up != 0 ? "会心" + weapon.enchant.powerUps.Crit_Up.ToString("F2") + "\n" : "")
                        + (weapon.enchant.Res_Rig_Add != 0 ? "僵直抗性" + weapon.enchant.Res_Rig_Add.ToString("F2") + "\n" : "")
                        + (weapon.enchant.Res_Req_Add != 0 ? "击退抗性" + weapon.enchant.Res_Req_Add.ToString("F2") + "\n" : "")
                        + (weapon.enchant.Res_Stu_Add != 0 ? "气绝抗性" + weapon.enchant.Res_Stu_Add.ToString("F2") + "\n" : "")
                        + (weapon.enchant.Res_Flo_Add != 0 ? "浮空抗性" + weapon.enchant.Res_Flo_Add.ToString("F2") + "\n" : "")
                        + (weapon.enchant.Res_Blo_Add != 0 ? "击飞抗性" + weapon.enchant.Res_Blo_Add.ToString("F2") + "\n" : "")
                        + (weapon.enchant.Res_Fal_Add != 0 ? "倒地抗性" + weapon.enchant.Res_Fal_Add.ToString("F2") + "\n" : "");
                    MyTools.SetActive(EnchantOrMaterialsList, true);
                }
                MyTools.SetActive(SuitEffect, true);
                if (weapon.suitEffect == null)
                {
                    SuitText.text = "无";
                }
                else
                {
                    SuitText.text = weapon.suitEffect.SuitName + "\n" + weapon.suitEffect.suit1Num + "件:"
                        + (weapon.suitEffect.powerUps1.ATK_Up != 0 ? "攻击力" + weapon.suitEffect.powerUps1.ATK_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps1.DEF_Up != 0 ? "防御力" + weapon.suitEffect.powerUps1.DEF_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps1.HP_Up != 0 ? "体力" + weapon.suitEffect.powerUps1.HP_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps1.MP_Up != 0 ? "真气" + weapon.suitEffect.powerUps1.MP_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps1.Endurance_Up != 0 ? "耐力" + weapon.suitEffect.powerUps1.Endurance_Up.ToString() + "\n" : "")
                        + (weapon.suitEffect.powerUps1.Hit_Up != 0 ? "命中" + weapon.suitEffect.powerUps1.Hit_Up.ToString("F2") + "," : "")
                        + (weapon.suitEffect.powerUps1.Dodge_Up != 0 ? "闪避" + weapon.suitEffect.powerUps1.Dodge_Up.ToString("F2") + "," : "")
                        + (weapon.suitEffect.powerUps1.Crit_Up != 0 ? "会心" + weapon.suitEffect.powerUps1.Crit_Up.ToString("F2") + "\n" : "")
                    + weapon.suitEffect.suit2Num + "件:"
                        + (weapon.suitEffect.powerUps2.ATK_Up != 0 ? "攻击力" + weapon.suitEffect.powerUps2.ATK_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps2.DEF_Up != 0 ? "防御力" + weapon.suitEffect.powerUps2.DEF_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps2.HP_Up != 0 ? "体力" + weapon.suitEffect.powerUps2.HP_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps2.MP_Up != 0 ? "真气" + weapon.suitEffect.powerUps2.MP_Up.ToString() + "," : "")
                        + (weapon.suitEffect.powerUps2.Endurance_Up != 0 ? "耐力" + weapon.suitEffect.powerUps2.Endurance_Up.ToString() + "\n" : "")
                        + (weapon.suitEffect.powerUps2.Hit_Up != 0 ? "命中" + weapon.suitEffect.powerUps2.Hit_Up.ToString("F2") + "," : "")
                        + (weapon.suitEffect.powerUps2.Dodge_Up != 0 ? "闪避" + weapon.suitEffect.powerUps2.Dodge_Up.ToString("F2") + "," : "")
                        + (weapon.suitEffect.powerUps2.Crit_Up != 0 ? "会心" + weapon.suitEffect.powerUps2.Crit_Up.ToString("F2") + "\n" : "");
                }
                break;
            case ItemType.Armor:
                ArmorItem armor = Item as ArmorItem;
                if (armor == null) break;
                Effect.text = "防御力" + armor.DEF.ToString("F0");
                itemType.text = armor.ItemTypeName + "/" + armor.type_name;
                if (TipsType != ItemTipsType.EuipmentItem && TipsType != ItemTipsType.DropItem)
                    if (!armor.Usable)
                    {
                        MyTools.SetActive(UsedButton.gameObject, false);
                        MyTools.SetActive(EquipButton.gameObject, false);
                        MyTools.SetActive(DiscardButton.gameObject, true);
                        MyTools.SetActive(HotKeyButton.gameObject, false);
                    }
                    else
                    {
                        MyTools.SetActive(UsedButton.gameObject, false);
                        MyTools.SetActive(EquipButton.gameObject, true);
                        MyTools.SetActive(DiscardButton.gameObject, true);
                        MyTools.SetActive(HotKeyButton.gameObject, true);
                    }
                if (armor.enchant == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                else
                {
                    EnchantOrMaterialsList.GetComponent<Text>().text = "淬火属性";
                    ListText.text =
                        (armor.enchant.powerUps.ATK_Up != 0 ? "攻击力" + armor.enchant.powerUps.ATK_Up.ToString() + "\n" : "")
                        + (armor.enchant.powerUps.DEF_Up != 0 ? "防御力" + armor.enchant.powerUps.DEF_Up.ToString() + "\n" : "")
                        + (armor.enchant.powerUps.HP_Up != 0 ? "体力" + armor.enchant.powerUps.HP_Up.ToString("F0") + "\n" : "")
                        + (armor.enchant.powerUps.MP_Up != 0 ? "真气" + armor.enchant.powerUps.MP_Up.ToString() + "\n" : "")
                        + (armor.enchant.powerUps.Endurance_Up != 0 ? "耐力" + armor.enchant.powerUps.Endurance_Up.ToString() + "\n" : "")
                        + (armor.enchant.powerUps.Hit_Up != 0 ? "命中" + armor.enchant.powerUps.Hit_Up.ToString("F2") + "\n" : "")
                        + (armor.enchant.powerUps.Dodge_Up != 0 ? "闪避" + armor.enchant.powerUps.Dodge_Up.ToString("F2") + "\n" : "")
                        + (armor.enchant.powerUps.Crit_Up != 0 ? "暴击" + armor.enchant.powerUps.Crit_Up.ToString("F2") + "\n" : "")
                        + (armor.enchant.Res_Rig_Add != 0 ? "硬直抗性" + armor.enchant.Res_Rig_Add.ToString("F2") + "\n" : "")
                        + (armor.enchant.Res_Req_Add != 0 ? "击退抗性" + armor.enchant.Res_Req_Add.ToString("F2") + "\n" : "")
                        + (armor.enchant.Res_Stu_Add != 0 ? "气绝抗性" + armor.enchant.Res_Stu_Add.ToString("F2") + "\n" : "")
                        + (armor.enchant.Res_Flo_Add != 0 ? "浮空抗性" + armor.enchant.Res_Flo_Add.ToString("F2") + "\n" : "")
                        + (armor.enchant.Res_Blo_Add != 0 ? "击飞抗性" + armor.enchant.Res_Blo_Add.ToString("F2") + "\n" : "")
                        + (armor.enchant.Res_Fal_Add != 0 ? "倒地抗性" + armor.enchant.Res_Fal_Add.ToString("F2") + "\n" : "");
                    MyTools.SetActive(EnchantOrMaterialsList, true);
                }
                MyTools.SetActive(SuitEffect, true);
                if (armor.suitEffect == null)
                {
                    SuitText.text = "无";
                }
                else
                {
                    SuitText.text = armor.suitEffect.SuitName + "\n" + armor.suitEffect.suit1Num + "件:"
                        + (armor.suitEffect.powerUps1.ATK_Up != 0 ? "攻击力" + armor.suitEffect.powerUps1.ATK_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.DEF_Up != 0 ? "防御力" + armor.suitEffect.powerUps1.DEF_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.HP_Up != 0 ? "体力" + armor.suitEffect.powerUps1.HP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.MP_Up != 0 ? "真气" + armor.suitEffect.powerUps1.MP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.Endurance_Up != 0 ? "耐力" + armor.suitEffect.powerUps1.Endurance_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.Hit_Up != 0 ? "命中" + armor.suitEffect.powerUps1.Hit_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps1.Dodge_Up != 0 ? "闪避" + armor.suitEffect.powerUps1.Dodge_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps1.Crit_Up != 0 ? "会心" + armor.suitEffect.powerUps1.Crit_Up.ToString("F2") + "\n" : "\n")
                    + armor.suitEffect.suit2Num + "件:"
                        + (armor.suitEffect.powerUps2.ATK_Up != 0 ? "攻击力" + armor.suitEffect.powerUps2.ATK_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.DEF_Up != 0 ? "防御力" + armor.suitEffect.powerUps2.DEF_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.HP_Up != 0 ? "体力" + armor.suitEffect.powerUps2.HP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.MP_Up != 0 ? "真气" + armor.suitEffect.powerUps2.MP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.Endurance_Up != 0 ? "耐力" + armor.suitEffect.powerUps2.Endurance_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.Hit_Up != 0 ? "命中" + armor.suitEffect.powerUps2.Hit_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps2.Dodge_Up != 0 ? "闪避" + armor.suitEffect.powerUps2.Dodge_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps2.Crit_Up != 0 ? "会心" + armor.suitEffect.powerUps2.Crit_Up.ToString("F2") + "\n" : "\n");
                }
                break;
            case ItemType.Jewelry:
                JewelryItem jewelry = Item as JewelryItem;
                if (jewelry == null) break;
                Effect.text = (jewelry.power_Add.ATK_Up != 0 ? "攻击力" + jewelry.power_Add.ATK_Up.ToString() + "\n" : "")
                    + (jewelry.power_Add.DEF_Up != 0 ? "防御力" + jewelry.power_Add.DEF_Up.ToString() + "\n" : "")
                    + (jewelry.power_Add.HP_Up != 0 ? "体力" + jewelry.power_Add.HP_Up.ToString() : "")
                    + (jewelry.power_Add.MP_Up != 0 ? "真气" + jewelry.power_Add.MP_Up.ToString() + "\n" : "")
                    + (jewelry.power_Add.Endurance_Up != 0 ? "耐力" + jewelry.power_Add.Endurance_Up.ToString() : "")
                    + (jewelry.power_Add.Hit_Up != 0 ? "命中" + jewelry.power_Add.Hit_Up.ToString("F2") + "\n" : "")
                    + (jewelry.power_Add.Dodge_Up != 0 ? "闪避" + jewelry.power_Add.Dodge_Up.ToString("F2") : "")
                    + (jewelry.power_Add.Crit_Up != 0 ? "会心" + jewelry.power_Add.Crit_Up.ToString("F2") + "\n" : "");
                itemType.text = jewelry.ItemTypeName + "/" + jewelry.type_name;
                if (TipsType != ItemTipsType.EuipmentItem && TipsType != ItemTipsType.DropItem)
                    if (!jewelry.Usable)
                    {
                        MyTools.SetActive(UsedButton.gameObject, false);
                        MyTools.SetActive(EquipButton.gameObject, false);
                        MyTools.SetActive(DiscardButton.gameObject, true);
                        MyTools.SetActive(HotKeyButton.gameObject, false);
                    }
                    else
                    {
                        MyTools.SetActive(UsedButton.gameObject, false);
                        MyTools.SetActive(EquipButton.gameObject, true);
                        MyTools.SetActive(DiscardButton.gameObject, true);
                        MyTools.SetActive(HotKeyButton.gameObject, true);
                    }
                if (jewelry.enchant == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                else
                {
                    EnchantOrMaterialsList.GetComponent<Text>().text = "淬火属性";
                    ListText.text =
                        (jewelry.enchant.powerUps.ATK_Up != 0 ? "攻击力" + jewelry.enchant.powerUps.ATK_Up.ToString("F0") + "\n" : "")
                        + (jewelry.enchant.powerUps.DEF_Up != 0 ? "防御力" + jewelry.enchant.powerUps.DEF_Up.ToString("F0") + "\n" : "")
                        + (jewelry.enchant.powerUps.HP_Up != 0 ? "体力" + jewelry.enchant.powerUps.HP_Up.ToString("F0") + "\n" : "")
                        + (jewelry.enchant.powerUps.MP_Up != 0 ? "真气" + jewelry.enchant.powerUps.MP_Up.ToString("F0") + "\n" : "")
                        + (jewelry.enchant.powerUps.Endurance_Up != 0 ? "耐力" + jewelry.enchant.powerUps.Endurance_Up.ToString("F0") + "\n" : "")
                        + (jewelry.enchant.powerUps.Hit_Up != 0 ? "命中" + jewelry.enchant.powerUps.Hit_Up.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.powerUps.Dodge_Up != 0 ? "闪避" + jewelry.enchant.powerUps.Dodge_Up.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.powerUps.Crit_Up != 0 ? "会心" + jewelry.enchant.powerUps.Crit_Up.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.Res_Rig_Add != 0 ? "僵直抗性" + jewelry.enchant.Res_Rig_Add.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.Res_Req_Add != 0 ? "击退抗性" + jewelry.enchant.Res_Req_Add.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.Res_Stu_Add != 0 ? "气绝抗性" + jewelry.enchant.Res_Stu_Add.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.Res_Flo_Add != 0 ? "浮空抗性" + jewelry.enchant.Res_Flo_Add.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.Res_Blo_Add != 0 ? "击飞抗性" + jewelry.enchant.Res_Blo_Add.ToString("F2") + "\n" : "")
                        + (jewelry.enchant.Res_Fal_Add != 0 ? "倒地抗性" + jewelry.enchant.Res_Fal_Add.ToString("F2") + "\n" : "");
                    MyTools.SetActive(EnchantOrMaterialsList, true);
                }
                MyTools.SetActive(SuitEffect, true);
                if (jewelry.suitEffect == null)
                {
                    SuitText.text = "无";
                }
                else
                {
                    SuitText.text = jewelry.suitEffect.SuitName + "\n" + jewelry.suitEffect.suit1Num + "件:"
                        + (jewelry.suitEffect.powerUps1.ATK_Up != 0 ? "攻击力" + jewelry.suitEffect.powerUps1.ATK_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.DEF_Up != 0 ? "防御力" + jewelry.suitEffect.powerUps1.DEF_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.HP_Up != 0 ? "体力" + jewelry.suitEffect.powerUps1.HP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.MP_Up != 0 ? "真气" + jewelry.suitEffect.powerUps1.MP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.Endurance_Up != 0 ? "耐力" + jewelry.suitEffect.powerUps1.Endurance_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.Hit_Up != 0 ? "命中" + jewelry.suitEffect.powerUps1.Hit_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps1.Dodge_Up != 0 ? "闪避" + jewelry.suitEffect.powerUps1.Dodge_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps1.Crit_Up != 0 ? "会心" + jewelry.suitEffect.powerUps1.Crit_Up.ToString("F2") + "\n" : "\n")
                    + jewelry.suitEffect.suit2Num + "件:"
                        + (jewelry.suitEffect.powerUps2.ATK_Up != 0 ? "攻击力" + jewelry.suitEffect.powerUps2.ATK_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.DEF_Up != 0 ? "防御力" + jewelry.suitEffect.powerUps2.DEF_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.HP_Up != 0 ? "体力" + jewelry.suitEffect.powerUps2.HP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.MP_Up != 0 ? "真气" + jewelry.suitEffect.powerUps2.MP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.Endurance_Up != 0 ? "耐力" + jewelry.suitEffect.powerUps2.Endurance_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.Hit_Up != 0 ? "命中" + jewelry.suitEffect.powerUps2.Hit_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps2.Dodge_Up != 0 ? "闪避" + jewelry.suitEffect.powerUps2.Dodge_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps2.Crit_Up != 0 ? "会心" + jewelry.suitEffect.powerUps2.Crit_Up.ToString("F2") + "\n" : "\n");
                }
                break;
            case ItemType.Medicine:
                MedicineItem medicine = Item as MedicineItem;
                if (medicine == null) break;
                Effect.text = (medicine.HP_Rec != 0 ? "体力恢复" + medicine.HP_Rec + "\n" : "")
                    + (medicine.MP_Rec != 0 ? "真气恢复" + medicine.MP_Rec + "\n" : "")
                     + (medicine.Endurance_Rec != 0 ? "耐力恢复" + medicine.Endurance_Rec + "\n" : "");
                itemType.text = medicine.ItemTypeName;
                if (medicine.Usable && TipsType != ItemTipsType.DropItem)
                {
                    MyTools.SetActive(UsedButton.gameObject, true);
                    MyTools.SetActive(HotKeyButton.gameObject, true);
                }
                else
                {
                    MyTools.SetActive(UsedButton.gameObject, false);
                    MyTools.SetActive(HotKeyButton.gameObject, false);
                }
                MyTools.SetActive(EnchantOrMaterialsList, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, true);
                MyTools.SetActive(SuitEffect, false);
                break;
            case ItemType.Material:
                MaterialItem material = Item as MaterialItem;
                if (material == null) break;
                Effect.text = string.Empty;
                itemType.text = material.ItemTypeName;
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                MyTools.SetActive(EnchantOrMaterialsList, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, true);
                MyTools.SetActive(SuitEffect, false);
                break;
            default:
                MyTools.SetActive(EnchantOrMaterialsList, false);
                Effect.text = string.Empty;
                itemType.text = Item.ItemTypeName;
                if (Item.Usable && TipsType != ItemTipsType.DropItem)
                {
                    MyTools.SetActive(UsedButton.gameObject, true);
                    MyTools.SetActive(HotKeyButton.gameObject, true);
                }
                else
                {
                    MyTools.SetActive(UsedButton.gameObject, false);
                    MyTools.SetActive(HotKeyButton.gameObject, false);
                }
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, true);
                MyTools.SetActive(SuitEffect, false);
                break;
        }
        Description.text = Item.Description;
        priceType.text = "贩卖价格";
        Price.text = Item.SellAble ? Item.SellPrice.ToString() + "文" : "<color=red>不可贩卖</color>";
        Weight.text = Item.Weight.ToString("F2") + "斤";
        if(TipsType == ItemTipsType.DropItem)
        {
            MyTools.SetActive(UsedButton.gameObject, false);
            MyTools.SetActive(EquipButton.gameObject, false);
            MyTools.SetActive(DiscardButton.gameObject, false);
            MyTools.SetActive(StoreButton.gameObject, false);
            MyTools.SetActive(TakeOutButton.gameObject, false);
        }
        if(WarehouseManager.Instance.isStoring && TipsType == ItemTipsType.BagItem)
        {
            MyTools.SetActive(UsedButton.gameObject, false);
            MyTools.SetActive(EquipButton.gameObject, false);
            MyTools.SetActive(DiscardButton.gameObject, false);
            MyTools.SetActive(StoreButton.gameObject, true);
            MyTools.SetActive(TakeOutButton.gameObject, false);
        }
        else
        {
            MyTools.SetActive(StoreButton.gameObject, false);
        }
        if (TipsType == ItemTipsType.WerehouseItem)
        {
            //Debug.Log("dasdasdas");
            MyTools.SetActive(UsedButton.gameObject, false);
            MyTools.SetActive(EquipButton.gameObject, false);
            MyTools.SetActive(DiscardButton.gameObject, false);
            MyTools.SetActive(StoreButton.gameObject, false);
            MyTools.SetActive(TakeOutButton.gameObject, true);
            MyTools.SetActive(HotKeyButton.gameObject, false);
        }
        if(TipsType == ItemTipsType.DropItem)
        {

        }
    }

    public void ShowShopItemInfo()
    {
        CheckBottonStatu();
        ItemBase Item;
        if (ShopManager.Instance.PlayerSelling) Item = buyerItemAgent.itemInfo.Item;
        else if (!ShopManager.Instance.PlayerMaking) Item = goodsAgent.goodsInfo.Item;
        else Item = makingAgent.makingInfo.Item;
        Icon.overrideSprite = Resources.Load(Item.Icon, typeof(Sprite)) as Sprite;
        //Debug.Log(Item.Icon);
        Name.text = Item.Name;
        MyTools.SetActive(EnchantOrMaterialsList, false);
        //Debug.Log(Item.ItemType);
        switch (Item.ItemType)
        {
            case ItemType.Weapon:
                WeaponItem weapon = Item as WeaponItem;
                if (weapon == null) break;
                Effect.text = "攻击力" + weapon.ATK.ToString("F0");
                itemType.text = weapon.ItemTypeName + "/" + weapon.type_name;
                if (!ShopManager.Instance.PlayerMaking)
                {
                    if (weapon.enchant == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        EnchantOrMaterialsList.GetComponent<Text>().text = "淬火属性";
                        ListText.text =
                            (weapon.enchant.powerUps.ATK_Up != 0 ? "攻击力" + weapon.enchant.powerUps.ATK_Up.ToString("F0") + "\n" : "")
                            + (weapon.enchant.powerUps.DEF_Up != 0 ? "防御力" + weapon.enchant.powerUps.DEF_Up.ToString("F0") + "\n" : "")
                            + (weapon.enchant.powerUps.HP_Up != 0 ? "体力" + weapon.enchant.powerUps.HP_Up.ToString("F0") + "\n" : "")
                            + (weapon.enchant.powerUps.MP_Up != 0 ? "真气" + weapon.enchant.powerUps.MP_Up.ToString("F0") + "\n" : "")
                            + (weapon.enchant.powerUps.Endurance_Up != 0 ? "耐力" + weapon.enchant.powerUps.Endurance_Up.ToString("F0") + "\n" : "")
                            + (weapon.enchant.powerUps.Hit_Up != 0 ? "命中" + weapon.enchant.powerUps.Hit_Up.ToString("F2") + "\n" : "")
                            + (weapon.enchant.powerUps.Dodge_Up != 0 ? "闪避" + weapon.enchant.powerUps.Dodge_Up.ToString("F2") + "\n" : "")
                            + (weapon.enchant.powerUps.Crit_Up != 0 ? "会心" + weapon.enchant.powerUps.Crit_Up.ToString("F2") + "\n" : "")
                            + (weapon.enchant.Res_Rig_Add != 0 ? "僵直抗性" + weapon.enchant.Res_Rig_Add.ToString("F2") + "\n" : "")
                            + (weapon.enchant.Res_Req_Add != 0 ? "击退抗性" + weapon.enchant.Res_Req_Add.ToString("F2") + "\n" : "")
                            + (weapon.enchant.Res_Stu_Add != 0 ? "气绝抗性" + weapon.enchant.Res_Stu_Add.ToString("F2") + "\n" : "")
                            + (weapon.enchant.Res_Flo_Add != 0 ? "浮空抗性" + weapon.enchant.Res_Flo_Add.ToString("F2") + "\n" : "")
                            + (weapon.enchant.Res_Blo_Add != 0 ? "击飞抗性" + weapon.enchant.Res_Blo_Add.ToString("F2") + "\n" : "")
                            + (weapon.enchant.Res_Fal_Add != 0 ? "倒地抗性" + weapon.enchant.Res_Fal_Add.ToString("F2") + "\n" : "");
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                    }
                }
                else
                {
                    if (makingAgent.makingInfo.Item.MaterialsList == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                        EnchantOrMaterialsList.GetComponent<Text>().text = "材料列表";
                        ListText.text = string.Empty;
                        foreach (string s in makingAgent.makingInfo.infos)
                            ListText.GetComponent<Text>().text += s;
                    }
                }
                MyTools.SetActive(SuitEffect, true);
                if (weapon.suitEffect == null)
                {
                    SuitText.text = "无";
                }
                else
                {
                    SuitText.text = weapon.suitEffect.SuitName + "\n" + weapon.suitEffect.suit1Num + "件:"
                        + (weapon.suitEffect.powerUps1.ATK_Up != 0 ? "攻击力" + weapon.suitEffect.powerUps1.ATK_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps1.DEF_Up != 0 ? "防御力" + weapon.suitEffect.powerUps1.DEF_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps1.HP_Up != 0 ? "体力" + weapon.suitEffect.powerUps1.HP_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps1.MP_Up != 0 ? "真气" + weapon.suitEffect.powerUps1.MP_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps1.Endurance_Up != 0 ? "耐力" + weapon.suitEffect.powerUps1.Endurance_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps1.Hit_Up != 0 ? "命中" + weapon.suitEffect.powerUps1.Hit_Up.ToString("F2") + " " : "")
                        + (weapon.suitEffect.powerUps1.Dodge_Up != 0 ? "闪避" + weapon.suitEffect.powerUps1.Dodge_Up.ToString("F2") + " " : "")
                        + (weapon.suitEffect.powerUps1.Crit_Up != 0 ? "会心" + weapon.suitEffect.powerUps1.Crit_Up.ToString("F2") + "\n" : "\n")
                    + weapon.suitEffect.suit2Num + "件:"
                        + (weapon.suitEffect.powerUps2.ATK_Up != 0 ? "攻击力" + weapon.suitEffect.powerUps2.ATK_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps2.DEF_Up != 0 ? "防御力" + weapon.suitEffect.powerUps2.DEF_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps2.HP_Up != 0 ? "体力" + weapon.suitEffect.powerUps2.HP_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps2.MP_Up != 0 ? "真气" + weapon.suitEffect.powerUps2.MP_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps2.Endurance_Up != 0 ? "耐力" + weapon.suitEffect.powerUps2.Endurance_Up.ToString() + " " : "")
                        + (weapon.suitEffect.powerUps2.Hit_Up != 0 ? "命中" + weapon.suitEffect.powerUps2.Hit_Up.ToString("F2") + " " : "")
                        + (weapon.suitEffect.powerUps2.Dodge_Up != 0 ? "闪避" + weapon.suitEffect.powerUps2.Dodge_Up.ToString("F2") + " " : "")
                        + (weapon.suitEffect.powerUps2.Crit_Up != 0 ? "会心" + weapon.suitEffect.powerUps2.Crit_Up.ToString("F2") + "\n" : "\n");
                }
                break;
            case ItemType.Armor:
                ArmorItem armor = Item as ArmorItem;
                if (armor == null) break;
                Effect.text = "防御力" + armor.DEF.ToString("F0");
                itemType.text = armor.ItemTypeName + "/" + armor.type_name;
                if (!ShopManager.Instance.PlayerMaking)
                {
                    if (armor.enchant == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        EnchantOrMaterialsList.GetComponent<Text>().text = "淬火属性";
                        ListText.text =
                            (armor.enchant.powerUps.ATK_Up != 0 ? "攻击力" + armor.enchant.powerUps.ATK_Up.ToString() + "\n" : "")
                            + (armor.enchant.powerUps.DEF_Up != 0 ? "防御力" + armor.enchant.powerUps.DEF_Up.ToString() + "\n" : "")
                            + (armor.enchant.powerUps.HP_Up != 0 ? "体力" + armor.enchant.powerUps.HP_Up.ToString("F0") + "\n" : "")
                            + (armor.enchant.powerUps.MP_Up != 0 ? "真气" + armor.enchant.powerUps.MP_Up.ToString() + "\n" : "")
                            + (armor.enchant.powerUps.Endurance_Up != 0 ? "耐力" + armor.enchant.powerUps.Endurance_Up.ToString() + "\n" : "")
                            + (armor.enchant.powerUps.Hit_Up != 0 ? "命中" + armor.enchant.powerUps.Hit_Up.ToString("F2") + "\n" : "")
                            + (armor.enchant.powerUps.Dodge_Up != 0 ? "闪避" + armor.enchant.powerUps.Dodge_Up.ToString("F2") + "\n" : "")
                            + (armor.enchant.powerUps.Crit_Up != 0 ? "暴击" + armor.enchant.powerUps.Crit_Up.ToString("F2") + "\n" : "")
                            + (armor.enchant.Res_Rig_Add != 0 ? "硬直抗性" + armor.enchant.Res_Rig_Add.ToString("F2") + "\n" : "")
                            + (armor.enchant.Res_Req_Add != 0 ? "击退抗性" + armor.enchant.Res_Req_Add.ToString("F2") + "\n" : "")
                            + (armor.enchant.Res_Stu_Add != 0 ? "气绝抗性" + armor.enchant.Res_Stu_Add.ToString("F2") + "\n" : "")
                            + (armor.enchant.Res_Flo_Add != 0 ? "浮空抗性" + armor.enchant.Res_Flo_Add.ToString("F2") + "\n" : "")
                            + (armor.enchant.Res_Blo_Add != 0 ? "击飞抗性" + armor.enchant.Res_Blo_Add.ToString("F2") + "\n" : "")
                            + (armor.enchant.Res_Fal_Add != 0 ? "倒地抗性" + armor.enchant.Res_Fal_Add.ToString("F2") + "\n" : "");
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                    }
                }
                else
                {
                    if (makingAgent.makingInfo.Item.MaterialsList == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                        EnchantOrMaterialsList.GetComponent<Text>().text = "材料列表";
                        ListText.text = string.Empty;
                        foreach (string s in makingAgent.makingInfo.infos)
                            ListText.GetComponent<Text>().text += s;
                    }
                }
                MyTools.SetActive(SuitEffect, true);
                if (armor.suitEffect == null)
                {
                    SuitText.text = "无";
                }
                else
                {
                    SuitText.text = armor.suitEffect.SuitName + "\n" + armor.suitEffect.suit1Num + "件:"
                        + (armor.suitEffect.powerUps1.ATK_Up != 0 ? "攻击力" + armor.suitEffect.powerUps1.ATK_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.DEF_Up != 0 ? "防御力" + armor.suitEffect.powerUps1.DEF_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.HP_Up != 0 ? "体力" + armor.suitEffect.powerUps1.HP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.MP_Up != 0 ? "真气" + armor.suitEffect.powerUps1.MP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.Endurance_Up != 0 ? "耐力" + armor.suitEffect.powerUps1.Endurance_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps1.Hit_Up != 0 ? "命中" + armor.suitEffect.powerUps1.Hit_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps1.Dodge_Up != 0 ? "闪避" + armor.suitEffect.powerUps1.Dodge_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps1.Crit_Up != 0 ? "会心" + armor.suitEffect.powerUps1.Crit_Up.ToString("F2") + "\n" : "\n")
                    + armor.suitEffect.suit2Num + "件:"
                        + (armor.suitEffect.powerUps2.ATK_Up != 0 ? "攻击力" + armor.suitEffect.powerUps2.ATK_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.DEF_Up != 0 ? "防御力" + armor.suitEffect.powerUps2.DEF_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.HP_Up != 0 ? "体力" + armor.suitEffect.powerUps2.HP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.MP_Up != 0 ? "真气" + armor.suitEffect.powerUps2.MP_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.Endurance_Up != 0 ? "耐力" + armor.suitEffect.powerUps2.Endurance_Up.ToString() + " " : "")
                        + (armor.suitEffect.powerUps2.Hit_Up != 0 ? "命中" + armor.suitEffect.powerUps2.Hit_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps2.Dodge_Up != 0 ? "闪避" + armor.suitEffect.powerUps2.Dodge_Up.ToString("F2") + " " : "")
                        + (armor.suitEffect.powerUps2.Crit_Up != 0 ? "会心" + armor.suitEffect.powerUps2.Crit_Up.ToString("F2") + "\n" : "\n");
                }
                break;
            case ItemType.Jewelry:
                JewelryItem jewelry = Item as JewelryItem;
                if (jewelry == null) break;
                Effect.text = (jewelry.power_Add.ATK_Up != 0 ? "攻击力" + jewelry.power_Add.ATK_Up.ToString() + "\n" : "")
                    + (jewelry.power_Add.DEF_Up != 0 ? "防御力" + jewelry.power_Add.DEF_Up.ToString() + "\n" : "")
                    + (jewelry.power_Add.HP_Up != 0 ? "体力" + jewelry.power_Add.HP_Up.ToString() : "")
                    + (jewelry.power_Add.MP_Up != 0 ? "真气" + jewelry.power_Add.MP_Up.ToString() + "\n" : "")
                    + (jewelry.power_Add.Endurance_Up != 0 ? "耐力" + jewelry.power_Add.Endurance_Up.ToString() : "")
                    + (jewelry.power_Add.Hit_Up != 0 ? "命中" + jewelry.power_Add.Hit_Up.ToString("F2") + "\n" : "")
                    + (jewelry.power_Add.Dodge_Up != 0 ? "闪避" + jewelry.power_Add.Dodge_Up.ToString("F2") : "")
                    + (jewelry.power_Add.Crit_Up != 0 ? "会心" + jewelry.power_Add.Crit_Up.ToString("F2") + "\n" : "");
                itemType.text = jewelry.ItemTypeName + "/" + jewelry.type_name;
                if (!ShopManager.Instance.PlayerMaking)
                {
                    if (jewelry.enchant == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        EnchantOrMaterialsList.GetComponent<Text>().text = "淬火属性";
                        ListText.text =
                            (jewelry.enchant.powerUps.ATK_Up != 0 ? "攻击力" + jewelry.enchant.powerUps.ATK_Up.ToString("F0") + "\n" : "")
                            + (jewelry.enchant.powerUps.DEF_Up != 0 ? "防御力" + jewelry.enchant.powerUps.DEF_Up.ToString("F0") + "\n" : "")
                            + (jewelry.enchant.powerUps.HP_Up != 0 ? "体力" + jewelry.enchant.powerUps.HP_Up.ToString("F0") + "\n" : "")
                            + (jewelry.enchant.powerUps.MP_Up != 0 ? "真气" + jewelry.enchant.powerUps.MP_Up.ToString("F0") + "\n" : "")
                            + (jewelry.enchant.powerUps.Endurance_Up != 0 ? "耐力" + jewelry.enchant.powerUps.Endurance_Up.ToString("F0") + "\n" : "")
                            + (jewelry.enchant.powerUps.Hit_Up != 0 ? "命中" + jewelry.enchant.powerUps.Hit_Up.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.powerUps.Dodge_Up != 0 ? "闪避" + jewelry.enchant.powerUps.Dodge_Up.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.powerUps.Crit_Up != 0 ? "会心" + jewelry.enchant.powerUps.Crit_Up.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.Res_Rig_Add != 0 ? "僵直抗性" + jewelry.enchant.Res_Rig_Add.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.Res_Req_Add != 0 ? "击退抗性" + jewelry.enchant.Res_Req_Add.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.Res_Stu_Add != 0 ? "气绝抗性" + jewelry.enchant.Res_Stu_Add.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.Res_Flo_Add != 0 ? "浮空抗性" + jewelry.enchant.Res_Flo_Add.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.Res_Blo_Add != 0 ? "击飞抗性" + jewelry.enchant.Res_Blo_Add.ToString("F2") + "\n" : "")
                            + (jewelry.enchant.Res_Fal_Add != 0 ? "倒地抗性" + jewelry.enchant.Res_Fal_Add.ToString("F2") + "\n" : "");
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                    }
                }
                else
                {
                    if (makingAgent.makingInfo.Item.MaterialsList == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                        EnchantOrMaterialsList.GetComponent<Text>().text = "材料列表";
                        ListText.text = string.Empty;
                        foreach (string s in makingAgent.makingInfo.infos)
                            ListText.GetComponent<Text>().text += s;
                    }
                }
                MyTools.SetActive(SuitEffect, true);
                if (jewelry.suitEffect == null)
                {
                    SuitText.text = "无";
                }
                else
                {
                    SuitText.text = jewelry.suitEffect.SuitName + "\n" + jewelry.suitEffect.suit1Num + "件:"
                        + (jewelry.suitEffect.powerUps1.ATK_Up != 0 ? "攻击力" + jewelry.suitEffect.powerUps1.ATK_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.DEF_Up != 0 ? "防御力" + jewelry.suitEffect.powerUps1.DEF_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.HP_Up != 0 ? "体力" + jewelry.suitEffect.powerUps1.HP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.MP_Up != 0 ? "真气" + jewelry.suitEffect.powerUps1.MP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.Endurance_Up != 0 ? "耐力" + jewelry.suitEffect.powerUps1.Endurance_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps1.Hit_Up != 0 ? "命中" + jewelry.suitEffect.powerUps1.Hit_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps1.Dodge_Up != 0 ? "闪避" + jewelry.suitEffect.powerUps1.Dodge_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps1.Crit_Up != 0 ? "会心" + jewelry.suitEffect.powerUps1.Crit_Up.ToString("F2") + "\n" : "\n")
                    + jewelry.suitEffect.suit2Num + "件:"
                        + (jewelry.suitEffect.powerUps2.ATK_Up != 0 ? "攻击力" + jewelry.suitEffect.powerUps2.ATK_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.DEF_Up != 0 ? "防御力" + jewelry.suitEffect.powerUps2.DEF_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.HP_Up != 0 ? "体力" + jewelry.suitEffect.powerUps2.HP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.MP_Up != 0 ? "真气" + jewelry.suitEffect.powerUps2.MP_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.Endurance_Up != 0 ? "耐力" + jewelry.suitEffect.powerUps2.Endurance_Up.ToString() + " " : "")
                        + (jewelry.suitEffect.powerUps2.Hit_Up != 0 ? "命中" + jewelry.suitEffect.powerUps2.Hit_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps2.Dodge_Up != 0 ? "闪避" + jewelry.suitEffect.powerUps2.Dodge_Up.ToString("F2") + " " : "")
                        + (jewelry.suitEffect.powerUps2.Crit_Up != 0 ? "会心" + jewelry.suitEffect.powerUps2.Crit_Up.ToString("F2") + "\n" : "\n");
                }
                break;
            case ItemType.Medicine:
                MedicineItem medicine = Item as MedicineItem;
                if (medicine == null) break;
                Effect.text = (medicine.HP_Rec != 0 ? "体力恢复" + medicine.HP_Rec + "\n" : "")
                    + (medicine.MP_Rec != 0 ? "真气恢复" + medicine.MP_Rec + "\n" : "")
                     + (medicine.Endurance_Rec != 0 ? "耐力恢复" + medicine.Endurance_Rec + "\n" : "");
                itemType.text = medicine.ItemTypeName;
                MyTools.SetActive(SuitEffect, false);
                if (ShopManager.Instance.PlayerMaking)
                {
                    if (makingAgent.makingInfo.Item.MaterialsList == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                        EnchantOrMaterialsList.GetComponent<Text>().text = "材料列表";
                        ListText.text = string.Empty;
                        foreach (string s in makingAgent.makingInfo.infos)
                            ListText.GetComponent<Text>().text += s;
                    }
                }
                break;
            case ItemType.Material:
                MaterialItem material = Item as MaterialItem;
                if (material == null) break;
                Effect.text = string.Empty;
                itemType.text = material.ItemTypeName;
                MyTools.SetActive(SuitEffect, false);
                if (ShopManager.Instance.PlayerMaking)
                {
                    if (makingAgent.makingInfo.Item.MaterialsList == null) MyTools.SetActive(EnchantOrMaterialsList, false);
                    else
                    {
                        MyTools.SetActive(EnchantOrMaterialsList, true);
                        EnchantOrMaterialsList.GetComponent<Text>().text = "材料列表";
                        ListText.text = string.Empty;
                        foreach (string s in makingAgent.makingInfo.infos)
                            ListText.GetComponent<Text>().text += s;
                    }
                }
                break;
            default:
                MyTools.SetActive(EnchantOrMaterialsList, false);
                Effect.text = string.Empty;
                itemType.text = Item.ItemTypeName;
                MyTools.SetActive(SuitEffect, false);
                break;
        }
        Description.text = Item.Description;
        Price.text = ShopManager.Instance.PlayerMaking ? makingAgent.makingInfo.Cost + "文" : ShopManager.Instance.PlayerSelling ? Item.SellPrice + "文" : goodsAgent.goodsInfo.Price + "文";
        Weight.text = Item.Weight.ToString("F2") + "斤";
        if (ShopManager.Instance.PlayerSelling)
        {
            priceType.text = "贩卖价格";
            MyTools.SetActive(BuyButton.gameObject, false);
            MyTools.SetActive(SellButton.gameObject, true);
            MyTools.SetActive(MakingButton.gameObject, false);
        }
        else if (!ShopManager.Instance.PlayerMaking)
        {
            priceType.text = "购买价格";
            MyTools.SetActive(BuyButton.gameObject, true);
            MyTools.SetActive(SellButton.gameObject, false);
            MyTools.SetActive(MakingButton.gameObject, false);
        }
        else
        {
            priceType.text = "制作费用";
            MyTools.SetActive(BuyButton.gameObject, false);
            MyTools.SetActive(SellButton.gameObject, false);
            List<string> infos;
            if (Item.CheckMaterials(BagManager.Instance.bagInfo, 1, out infos)) MyTools.SetActive(MakingButton.gameObject, true);
            else MyTools.SetActive(MakingButton.gameObject, false);
        }
    }

    #region 按钮行为
    public void OnUsedButtonClick()
    {
        itemAgent.Used();
    }

    public void OnEquipButtonClick()
    {
        itemAgent.Used();
    }

    public void OnPickUpButtonClick()
    {
        dropItemAgent.OnPickUp();
    }

    public void OnDiscardButtonClick()
    {
        if (itemAgent)
            itemAgent.OnDiscard();
    }

    public void OnBuyButtonClick()
    {
        if (goodsAgent)
            goodsAgent.OnBuyButtonClick();
    }

    public void OnSellButtonClick()
    {
        if (buyerItemAgent)
            buyerItemAgent.OnSellButtonClick();
    }

    public void OnMakingButtonClick()
    {
        if (makingAgent)
            makingAgent.OnMakingButtonClick();
    }

    public void OnStoreButtonClick()
    {
        if (itemAgent)
            itemAgent.OnStore();
    }

    public void OnTakeOutButtonClick()
    {
        if (itemAgent)
            itemAgent.OnTakeOut();
    }

    public void OnHotKeyButtonClick()
    {
        if (itemAgent)
            HotKeyBarManager.Self.OpenSelectUI();
    }
    #endregion

    void CheckBottonStatu()
    {
        switch (TipsType)
        {
            case ItemTipsType.BagItem:
                MyTools.SetActive(UsedButton.gameObject, true);
                MyTools.SetActive(EquipButton.gameObject, true);
                MyTools.SetActive(DiscardButton.gameObject, true);
                MyTools.SetActive(UnequipButton.gameObject, false);
                MyTools.SetActive(PickUpButton.gameObject, false);
                MyTools.SetActive(BuyButton.gameObject, false);
                MyTools.SetActive(SellButton.gameObject, false);
                MyTools.SetActive(MakingButton.gameObject, false);
                MyTools.SetActive(StoreButton.gameObject, true);
                MyTools.SetActive(TakeOutButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, true);
                break;
            case ItemTipsType.WerehouseItem:
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, false);
                MyTools.SetActive(UnequipButton.gameObject, false);
                MyTools.SetActive(PickUpButton.gameObject, false);
                MyTools.SetActive(BuyButton.gameObject, false);
                MyTools.SetActive(SellButton.gameObject, false);
                MyTools.SetActive(MakingButton.gameObject, false);
                MyTools.SetActive(StoreButton.gameObject, false);
                MyTools.SetActive(TakeOutButton.gameObject, true);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                break;
            case ItemTipsType.EuipmentItem:
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, false);
                MyTools.SetActive(UnequipButton.gameObject, true);
                MyTools.SetActive(PickUpButton.gameObject, false);
                MyTools.SetActive(BuyButton.gameObject, false);
                MyTools.SetActive(SellButton.gameObject, false);
                MyTools.SetActive(MakingButton.gameObject, false);
                MyTools.SetActive(StoreButton.gameObject, false);
                MyTools.SetActive(TakeOutButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                break;
            case ItemTipsType.DropItem:
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, false);
                MyTools.SetActive(UnequipButton.gameObject, false);
                MyTools.SetActive(PickUpButton.gameObject, true);
                MyTools.SetActive(BuyButton.gameObject, false);
                MyTools.SetActive(SellButton.gameObject, false);
                MyTools.SetActive(MakingButton.gameObject, false);
                MyTools.SetActive(StoreButton.gameObject, false);
                MyTools.SetActive(TakeOutButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                break;
            case ItemTipsType.GoodsItem:
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, false);
                MyTools.SetActive(UnequipButton.gameObject, false);
                MyTools.SetActive(PickUpButton.gameObject, false);
                MyTools.SetActive(BuyButton.gameObject, true);
                MyTools.SetActive(SellButton.gameObject, false);
                MyTools.SetActive(MakingButton.gameObject, false);
                MyTools.SetActive(StoreButton.gameObject, false);
                MyTools.SetActive(TakeOutButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                break;
            case ItemTipsType.BuyerItem:
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, false);
                MyTools.SetActive(UnequipButton.gameObject, false);
                MyTools.SetActive(PickUpButton.gameObject, false);
                MyTools.SetActive(BuyButton.gameObject, false);
                MyTools.SetActive(SellButton.gameObject, true);
                MyTools.SetActive(MakingButton.gameObject, false);
                MyTools.SetActive(StoreButton.gameObject, false);
                MyTools.SetActive(TakeOutButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                break;
            case ItemTipsType.MakingItem:
                MyTools.SetActive(UsedButton.gameObject, false);
                MyTools.SetActive(EquipButton.gameObject, false);
                MyTools.SetActive(DiscardButton.gameObject, false);
                MyTools.SetActive(UnequipButton.gameObject, false);
                MyTools.SetActive(PickUpButton.gameObject, false);
                MyTools.SetActive(BuyButton.gameObject, false);
                MyTools.SetActive(SellButton.gameObject, false);
                MyTools.SetActive(MakingButton.gameObject, true);
                MyTools.SetActive(StoreButton.gameObject, false);
                MyTools.SetActive(TakeOutButton.gameObject, false);
                MyTools.SetActive(HotKeyButton.gameObject, false);
                break;
        }
    }

    public void OpenUI()
    {
        if ((itemAgent && TipsType == ItemTipsType.BagItem || TipsType == ItemTipsType.WerehouseItem) || (dropItemAgent && TipsType == ItemTipsType.DropItem) || (equipInfo != null && TipsType == ItemTipsType.EuipmentItem)) ShowItemInfo();
        if ((goodsAgent && TipsType == ItemTipsType.GoodsItem) || (buyerItemAgent && TipsType == ItemTipsType.BuyerItem) || (makingAgent && TipsType == ItemTipsType.MakingItem))
            ShowShopItemInfo();
        MyTools.SetActive(UI, true);
    }
    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
        itemAgent = null;
        goodsAgent = null;
        buyerItemAgent = null;
        makingAgent = null;
        equipInfo = null;
    }
}
