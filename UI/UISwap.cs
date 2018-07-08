using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UISwap : MonoBehaviour {
    public UISwapAnima toClose;
    public UISwapAnima toOpen;
    public Button swapButton;
    bool isClick;

    private void Start()
    {
        swapButton.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if (toClose != null && toOpen != null)
        {
            if (isClick && toClose != toOpen)
            {
                toClose.OnClose();
                toOpen.OnOpen();
                isClick = false;
            }
            else if (isClick && toClose == toOpen)
            {
                if (toClose.isOpen) toClose.OnClose();
                else toClose.OnOpen();
                isClick = false;
            }
        }
        else if (toOpen == null && toClose != null)
        {
            if (isClick)
            {
                toClose.OnClose();
                isClick = false;
            }
        }
        else if (toClose == null && toOpen != null)
        {
            if (isClick)
            {
                toOpen.OnOpen();
                isClick = false;
            }
        }
    }

    public void OnClick()
    {
        isClick = true;
    }
}
