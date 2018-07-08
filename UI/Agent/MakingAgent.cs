using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakingAgent : MonoBehaviour {

    public MakingInfo makingInfo;
    Text Name;
    Button IconButton;
    Text Cost;
    Text MakeAble;
    Button MakeButton;
    Image icon;
    [HideInInspector]
    public Sprite iconImage;

    // Use this for initialization
    void Start () {
        Name = transform.Find("Name").GetComponent<Text>();
        IconButton = transform.Find("Icon").GetComponent<Button>();
        icon = IconButton.GetComponent<Image>();
        Cost = transform.Find("MakingCost/Cost").GetComponent<Text>();
        MakeAble = transform.Find("MakeAble").GetComponent<Text>();
        MakeButton = transform.Find("Make").GetComponent<Button>();
        IconButton.onClick.AddListener(OnIconClick);
        MakeButton.onClick.AddListener(OnMakingButtonClick);
        ShowInfo();
    }

    // Update is called once per frame
    void Update () {
        CheckMakeAble();
    }

    void CheckMakeAble()
    {
        if (makingInfo != null)
        {
            try
            {
                makingInfo.CheckMaxMake(BagManager.Instance.bagInfo);
                icon.overrideSprite = iconImage;
                MakeAble.text = makingInfo.Check(BagManager.Instance.bagInfo, 1) ? "可制作" : "<color=red>材料不足</color>";
                if (makingInfo.Check(BagManager.Instance.bagInfo, 1)) MakeButton.interactable = true;
                else MakeButton.interactable = false;
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex.Message);
                icon.overrideSprite = iconImage;
                MakeAble.text = "<color=red>材料不明</color>";
                MakeButton.interactable = false;
            }
        }
    }

    void ShowInfo()
    {
        Name.text = makingInfo.Item.Name;
        icon.overrideSprite = Resources.Load(makingInfo.Item.Icon, typeof(Sprite)) as Sprite;
        iconImage = icon.overrideSprite;
        Cost.text = makingInfo.Cost + "文";
        MakeAble.text = makingInfo.Check(BagManager.Instance.bagInfo, 1) ? "可制作" : "<color=red>材料不足</color>";
        makingInfo.CheckMaxMake(BagManager.Instance.bagInfo);
    }

    public void OnMakingButtonClick()
    {
        if (makingInfo == null) return;
        ItemTipsManager.Instance.CloseUI();
        ItemConfirmManager.Instance.ItemName.text = makingInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = iconImage;
        ItemConfirmManager.Instance.MaxNumber = makingInfo.MaxMake;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(Make);
        ItemConfirmManager.Instance.OpenUI();
    }

    void Make()
    {
        try
        {
            ItemInfo find = BagManager.Instance.bagInfo.itemList.Find(i => i.ItemID == makingInfo.Item.ID);
            int finallyMake = find == null ? 0 : find.Quantity;
            makingInfo.Make(BagManager.Instance.bagInfo, ItemConfirmManager.Instance.ItemNumber);
            find = BagManager.Instance.bagInfo.itemList.Find(i => i.ItemID == makingInfo.Item.ID);
            finallyMake = find == null ? 0 : find.Quantity - finallyMake;
            if (ItemConfirmManager.Instance.ItemNumber > 0)
            {
                NotificationManager.Instance.NewNotification("制作了" + finallyMake + "个<color=orange>" + ItemConfirmManager.Instance.ItemName.text + "</color>");
            }
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
        ItemConfirmManager.Instance.CloseUI();
        BagManager.Instance.LoadFromBagInfo();
    }

    void OnIconClick()
    {
        ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.MakingItem;
        ItemTipsManager.Instance.makingAgent = this;
        ShopManager.Instance.PlayerSelling = false;
        ShopManager.Instance.PlayerMaking = true;
        ItemTipsManager.Instance.OpenUI();
    }
}
