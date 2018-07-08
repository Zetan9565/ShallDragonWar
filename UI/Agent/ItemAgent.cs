using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemAgent : MonoBehaviour
{ 
    public ItemInfo itemInfo;
    public GameObject acted_obj;
    [HideInInspector]
    public Text amount;
    public UnityEvent onUsed;
    [HideInInspector]
    public bool isStored;
    Image icon;
    [HideInInspector]
    public Sprite iconImage;

    // Use this for initialization
    void Start()
    {
        icon = GetComponent<Image>();
        amount = GetComponentInChildren<Text>();
        acted_obj = GameObject.FindGameObjectWithTag("Player");
        if (itemInfo != null)
        {
            icon.overrideSprite = Resources.Load(itemInfo.Item.Icon, typeof(Sprite)) as Sprite;
            iconImage = icon.overrideSprite;
            amount.text = itemInfo.Quantity <= 1 ? string.Empty : itemInfo.Quantity.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (itemInfo != null && itemInfo.Quantity <= 0)
        {
            Destroy(gameObject);
        }
        if (itemInfo != null)
        {
            icon.overrideSprite = iconImage;
            amount.text = itemInfo.Quantity <= 1 ? string.Empty : itemInfo.Quantity.ToString();
        }
    }

    public void Used()
    {
        try
        {
            itemInfo.Item.OnUsed(acted_obj);
            itemInfo.Item.OnUsed(PlayerInfoManager.Instance.PlayerInfo);
            if (itemInfo.Item.ItemType == MyEnums.ItemType.Weapon)
            {
                WeaponItem weapon = itemInfo.Item as WeaponItem;
                if (weapon != null && PlayerInfoManager.Instance.PlayerInfo.equipments.weapon == weapon)
                {
                    PlayerWeaponManager.Instance.EquipWeapon();
                }
            }
            EquipmentsManager.Instance.CheckEquipment();
            BagManager.Instance.LoadFromBagInfo();
            if (onUsed != null) onUsed.Invoke();
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
    }

    public void OnDiscard()
    {
        ItemTipsManager.Instance.CloseUI();
        ItemConfirmManager.Instance.ItemName.text = itemInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = icon.overrideSprite;
        ItemConfirmManager.Instance.MaxNumber = itemInfo.Quantity;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(Discard);
        ItemConfirmManager.Instance.OpenUI();
    }

    public void Discard()
    {
        try
        {
            BagManager.Instance.bagInfo.LoseItem(itemInfo.Item, ItemConfirmManager.Instance.ItemNumber);
            //Debug.Log(ItemConfirmManager.Self.ItemNumber);
            if (ItemConfirmManager.Instance.ItemNumber > 0)
            {
                NotificationManager.Instance.NewNotification("丢弃了" + ItemConfirmManager.Instance.ItemNumber + "个<color=orange>" + ItemConfirmManager.Instance.ItemName.text + "</color>");
            }
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
        ItemConfirmManager.Instance.CloseUI();
    }

    public void OnIconClick()
    {
        if (isStored)
            ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.WerehouseItem;
        else 
            ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.BagItem;
        ItemTipsManager.Instance.itemAgent = this;
        ItemTipsManager.Instance.OpenUI();
    }

    public void OnTakeOut()
    {
        ItemTipsManager.Instance.CloseUI();
        ItemConfirmManager.Instance.ItemName.text = itemInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = icon.overrideSprite;
        ItemConfirmManager.Instance.MaxNumber = itemInfo.Quantity;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(TakeOut);
        ItemConfirmManager.Instance.OpenUI();
    }

    public void TakeOut()
    {
        try
        {
            int finallyTake = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.TakeOutItem(itemInfo, BagManager.Instance.bagInfo, ItemConfirmManager.Instance.ItemNumber);
            //Debug.Log(ItemConfirmManager.Self.ItemNumber);
            if (finallyTake > 0)
            {
                NotificationManager.Instance.NewNotification("取出了" + ItemConfirmManager.Instance.ItemNumber + "个<color=orange>" + ItemConfirmManager.Instance.ItemName.text + "</color>");
            }
            BagManager.Instance.LoadFromBagInfo();
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
        ItemConfirmManager.Instance.CloseUI();
    }

    public void OnStore()
    {
        ItemTipsManager.Instance.CloseUI();
        ItemConfirmManager.Instance.ItemName.text = itemInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = icon.overrideSprite;
        ItemConfirmManager.Instance.MaxNumber = itemInfo.Quantity;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(Store);
        ItemConfirmManager.Instance.OpenUI();
    }

    public void Store()
    {
        try
        {
            WarehouseManager.Instance.StoreItem(itemInfo.Item, ItemConfirmManager.Instance.ItemNumber);
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
        ItemConfirmManager.Instance.CloseUI();
    }
}
