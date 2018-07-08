using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class HotKeyBarManager : MonoBehaviour {
    public static HotKeyBarManager Self;

    public GameObject UI;
    public GameObject selectionUI;
    [Space]
    public HotKeyItemAgent[] hotKeys;
    /*[Space]
    public Button selectBtn1;
    public Button selectBtn2;
    public Button selectBtn3;
    public Button selectBtn4;*/

    private void Awake()
    {
        Self = this;
    }

    // Use this for initialization
    void Start () {
        /*selectBtn1.onClick.AddListener(delegate { OnSelectButtonClick(1); });
        selectBtn2.onClick.AddListener(delegate { OnSelectButtonClick(2); });
        selectBtn3.onClick.AddListener(delegate { OnSelectButtonClick(3); });
        selectBtn4.onClick.AddListener(delegate { OnSelectButtonClick(4); });*/
        MyTools.SetActive(UI, false);
        MyTools.SetActive(selectionUI, false);
    }

    // Update is called once per frame
    void Update () {
        if (!Application.isMobilePlatform)
        {
            if (CrossPlatformInputManager.GetButtonDown("HotKey1"))
                hotKeys[0].OnIconClick();
            if (CrossPlatformInputManager.GetButtonDown("HotKey2"))
                hotKeys[1].OnIconClick();
            if (CrossPlatformInputManager.GetButtonDown("HotKey3"))
                hotKeys[2].OnIconClick();
            if (CrossPlatformInputManager.GetButtonDown("HotKey4"))
                hotKeys[3].OnIconClick();
        }
    }

    public void SwapBar()
    {
        if (UI.activeSelf)
        {
            MyTools.SetActive(UI, false);
        }
        else
        {
            MyTools.SetActive(UI, true);
        }
    }

    public void OpenSelectUI()
    {
        MyTools.SetActive(selectionUI, true);
    }

    public void CloseSelectUI()
    {
        MyTools.SetActive(selectionUI, false);
    }

    public void OnSelectButtonClick(int select)
    {
        if(ItemTipsManager.Instance.itemAgent)
        {
            switch (select)
            {
                case 1: hotKeys[0].SetItem(ItemTipsManager.Instance.itemAgent); break;
                case 2: hotKeys[1].SetItem(ItemTipsManager.Instance.itemAgent); break;
                case 3: hotKeys[2].SetItem(ItemTipsManager.Instance.itemAgent); break;
                case 4: hotKeys[3].SetItem(ItemTipsManager.Instance.itemAgent); break;
            }
            CloseSelectUI();
            ItemTipsManager.Instance.CloseUI();
        }
    }
}
