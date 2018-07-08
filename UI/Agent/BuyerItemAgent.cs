using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyerItemAgent : MonoBehaviour {

    public ItemInfo itemInfo;
    Text Name;
    Button IconButton;
    Text SellPrice;
    Text Quantity;
    Button SellButton;
    Image icon;
    [HideInInspector]
    public Sprite iconImage;

    // Use this for initialization
    void Start () {
        Name = transform.Find("Name").GetComponent<Text>();
        IconButton = transform.Find("Icon").GetComponent<Button>();
        icon = IconButton.GetComponent<Image>();
        SellPrice = transform.Find("SellPrice/Price").GetComponent<Text>();
        Quantity = transform.Find("Quantity/Num").GetComponent<Text>();
        SellButton = transform.Find("Sell").GetComponent<Button>();
        IconButton.onClick.AddListener(OnIconClick);
        SellButton.onClick.AddListener(OnSellButtonClick);
        ShowInfo();
    }
	
	// Update is called once per frame
	void Update () {
        if (itemInfo != null)
        {
            Quantity.text = itemInfo.Quantity.ToString();
            icon.overrideSprite = iconImage;
        }
        if (itemInfo.Quantity <= 0)
        {
            ShopManager.Instance.OnPlayerSell();
        }
    }

    void ShowInfo()
    {
        Name.text = itemInfo.Item.Name;
        icon.overrideSprite = Resources.Load(itemInfo.Item.Icon, typeof(Sprite)) as Sprite;
        iconImage = icon.overrideSprite;
        SellPrice.text = itemInfo.Item.SellPrice + "文";
        Quantity.text = itemInfo.Quantity.ToString();
    }

    public void OnSellButtonClick()
    {
        if (itemInfo == null) return;
        ItemConfirmManager.Instance.ItemName.text = itemInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = Resources.Load(itemInfo.Item.Icon, typeof(Sprite)) as Sprite;
        ItemConfirmManager.Instance.MaxNumber = itemInfo.Quantity;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(Sell);
        ItemConfirmManager.Instance.OpenUI();
    }

    public void Sell()
    {
        try
        {
            itemInfo.Item.OnSell(BagManager.Instance.bagInfo, ItemConfirmManager.Instance.ItemNumber);
            if (ItemConfirmManager.Instance.ItemNumber > 0)
            {
                NotificationManager.Instance.NewNotification("出售了" + ItemConfirmManager.Instance.ItemNumber + "个<color=orange>" + ItemConfirmManager.Instance.ItemName.text + "</color>");
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
        ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.BuyerItem;
        ItemTipsManager.Instance.buyerItemAgent = this;
        ShopManager.Instance.PlayerSelling = true;
        ShopManager.Instance.PlayerMaking = false;
        ItemTipsManager.Instance.OpenUI();
    }
}
