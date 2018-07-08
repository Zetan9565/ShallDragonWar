using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyItemAgent : MonoBehaviour {

    //[HideInInspector]
    public ItemInfo itemInfo;
    //[Range(1, 4)]
    //public int hotKeyIndex = 1;
    public Sprite emptyIcon;
    Image icon;
    Sprite iconImage;
    Button iconButton;
    Text amount;

	// Use this for initialization
	void Start () {
        iconButton = GetComponent<Button>();
        iconButton.onClick.AddListener(OnIconClick);
        icon = transform.Find("Icon").GetComponent<Image>();
        amount = transform.Find("Amount").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(itemInfo != null)
        {
            amount.text = itemInfo.Quantity <= 1 ? string.Empty : itemInfo.Quantity.ToString();
            icon.overrideSprite = iconImage;
            if (itemInfo.Quantity <= 0) itemInfo = null;
        }
        else
        {
            icon.overrideSprite = emptyIcon;
            amount.text = string.Empty;
        }
        /*if (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButton("HotKey" + hotKeyIndex))
            OnIconClick();*/
	}

    public void OnIconClick()
    {
        if(itemInfo != null)
        {
            if (BagManager.Instance.itemAgents.Find(i => i.itemInfo == itemInfo))
                BagManager.Instance.itemAgents.Find(i => i.itemInfo == itemInfo).Used();
        }
    }

    public void SetItem(ItemAgent itemAgent)
    {
        if (!itemAgent) return;
        itemInfo = itemAgent.itemInfo;
        icon.overrideSprite = itemAgent.iconImage;
        iconImage = icon.overrideSprite;
    }
}
