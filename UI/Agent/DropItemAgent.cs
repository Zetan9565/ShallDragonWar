using UnityEngine;
using UnityEngine.UI;

public class DropItemAgent : MonoBehaviour
{
    public DropItemListAgent parentAgent;
    public DropItemInfo dropItemInfo;
    Text amount;
    Image icon;
    [HideInInspector]
    public Sprite iconImage;

    // Use this for initialization
    void Start()
    {
        amount = transform.Find("Amount").GetComponent<Text>();
        icon = GetComponent<Image>();
        if (dropItemInfo != null)
        {
            icon.overrideSprite = Resources.Load(dropItemInfo.Item.Icon, typeof(Sprite)) as Sprite;
            iconImage = icon.overrideSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dropItemInfo != null && dropItemInfo.Left <= 0) Destroy(gameObject);
        if (dropItemInfo != null)
        {
            icon.overrideSprite = iconImage;
            amount.text = dropItemInfo.Left <= 1 ? string.Empty : dropItemInfo.Left.ToString();
        }
    }

    public void OnIconClick()
    {
        ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.DropItem;
        ItemTipsManager.Instance.dropItemAgent = this;
        ItemTipsManager.Instance.OpenUI();
    }

    public void OnPickUp()
    {
        if (dropItemInfo == null) return;
        ItemTipsManager.Instance.CloseUI();
        ItemConfirmManager.Instance.ItemName.text = dropItemInfo.Item.Name;
        ItemConfirmManager.Instance.ItemIcon.overrideSprite = Resources.Load(dropItemInfo.Item.Icon, typeof(Sprite)) as Sprite;
        ItemConfirmManager.Instance.MaxNumber = dropItemInfo.Left;
        ItemConfirmManager.Instance.YesButton.onClick.AddListener(PickUp);
        ItemConfirmManager.Instance.OpenUI();
    }

    public void PickUp()
    {
        try
        {
            BagManager.Instance.GetItem(dropItemInfo.Item, dropItemInfo.TryPickUp(BagManager.Instance.bagInfo, ItemConfirmManager.Instance.ItemNumber));
            ItemConfirmManager.Instance.CloseUI();
            if (dropItemInfo.Left <= 0)
            {
                PickUpManager.Instance.itemCells.Remove(this);
                //Debug.Log(parentAgent.dropItemList.RemoveAll(i => i == dropItemInfo));
                parentAgent.dropItemList.RemoveAll(i => i == dropItemInfo);
                if (PickUpManager.Instance.itemCells.Count <= 0)
                {
                    //PickUpManager.Self.dropItemListAgent.dropItemList.RemoveAll(i => i == dropItemInfo);
                    PickUpManager.Instance.dropItemListAgent = null;
                    PickUpManager.Instance.CloseUI();
                }
                Destroy(gameObject);
            }
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
        }
    }

    /*public void PickUpAll()
    {
        try
        {
            //Debug.Log("PickAll");
            BagManager.Self.GetItem(dropItemInfo.Item, dropItemInfo.TryPickUpAll(BagManager.Self.bagInfo));
            if (dropItemInfo.Left <= 0)
            {
                PickUpManager.Self.itemCells.Remove(this);
                parentAgent.dropItemList.RemoveAll(i => i == dropItemInfo);
                if (PickUpManager.Self.itemCells.Count <= 0)
                {
                    //PickUpManager.Self.dropItemListAgent.dropItemList.RemoveAll(i => i == dropItemInfo);
                    PickUpManager.Self.dropItemListAgent = null;
                    PickUpManager.Self.CloseUI();
                }
                Destroy(gameObject);
            }
        }
        catch (System.Exception ex)
        {
            NotificationManager.Self.NewNotification(ex.Message);
        }
    }*/
}
