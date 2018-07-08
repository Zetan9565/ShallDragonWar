using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsAgent : MonoBehaviour {

    public GoodsInfo goodsInfo;
    Text Name;
    Button IconButton;
    Text BuyPrice;
    Text numForSell;
    Button BuyButton;
    Image icon;
    [HideInInspector]
    public Sprite iconImage;

    // Use this for initialization
    void Start () {
        Name = transform.Find("Name").GetComponent<Text>();
        IconButton = transform.Find("Icon").GetComponent<Button>();
        icon = IconButton.GetComponent<Image>();
        BuyPrice = transform.Find("BuyPrice/Price").GetComponent<Text>();
        numForSell = transform.Find("NumForSell/Num").GetComponent<Text>();
        BuyButton = transform.Find("Buy").GetComponent<Button>();
        IconButton.onClick.AddListener(OnIconClick);
        BuyButton.onClick.AddListener(OnBuyButtonClick);
        ShowInfo();
    }
	
	// Update is called once per frame
	void Update () {
        if (goodsInfo != null)
        {
            icon.overrideSprite = iconImage;
            numForSell.text = goodsInfo.isSoldOut ? "<color=red>售罄</color>" : goodsInfo.sellOutAble ? goodsInfo.NumForSell.ToString() : "充足";
        }
    }

    void ShowInfo()
    {
        Name.text = goodsInfo.Item.Name;
        icon.overrideSprite = Resources.Load(goodsInfo.Item.Icon, typeof(Sprite)) as Sprite;
        iconImage = icon.overrideSprite;
        BuyPrice.text = goodsInfo.Price + "文";
        numForSell.text = goodsInfo.isSoldOut ? "<color=red>售罄</color>" : goodsInfo.sellOutAble ? goodsInfo.NumForSell.ToString() : "充足";
    }

    public void OnBuyButtonClick()
    {
        if (goodsInfo == null) return;
        ItemConfirmManager.Instance.ItemName.text = goodsInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = Resources.Load(goodsInfo.Item.Icon, typeof(Sprite)) as Sprite;
        ItemConfirmManager.Instance.MaxNumber = goodsInfo.NumForSell;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(Sell);
        ItemConfirmManager.Instance.OpenUI();
    }

    void Sell()
    {
        try
        {
            ItemInfo find = BagManager.Instance.bagInfo.itemList.Find(i => i.ItemID == goodsInfo.Item.ID);
            int finallySell = find == null ? 0 : find.Quantity;
            goodsInfo.OnSell(ItemConfirmManager.Instance.ItemNumber, BagManager.Instance.bagInfo);
            find = BagManager.Instance.bagInfo.itemList.Find(i => i.ItemID == goodsInfo.Item.ID);
            finallySell = find == null ? 0 : find.Quantity - finallySell;
            if (ItemConfirmManager.Instance.ItemNumber > 0)
            {
                NotificationManager.Instance.NewNotification("购买了" + finallySell + "个<color=orange>" + ItemConfirmManager.Instance.ItemName.text + "</color>");
            }
        }
        catch(System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
        ItemConfirmManager.Instance.CloseUI();
        BagManager.Instance.LoadFromBagInfo();
    }

    void OnIconClick()
    {
        ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.GoodsItem;
        ItemTipsManager.Instance.goodsAgent = this;
        ShopManager.Instance.PlayerSelling = false;
        ShopManager.Instance.PlayerMaking = false;
        ItemTipsManager.Instance.OpenUI();
    }
}
