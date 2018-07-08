using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataButtons : MonoBehaviour
{

    enum ButtonType
    {
        关闭,
        存档,
        读档,
        脱离卡死,
        复活,
        主菜单
    }
    [SerializeField]
    ButtonType buttonType;

    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        switch (buttonType)
        {
            case ButtonType.关闭:
                GameDataManager.Instance.CloseUI();
                break;
            case ButtonType.存档:
                GameDataManager.Instance.isSaving = true;
                GameDataManager.Instance.OpenUI();
                break;
            case ButtonType.读档:
                GameDataManager.Instance.isSaving = false;
                GameDataManager.Instance.OpenUI();
                break;
            case ButtonType.脱离卡死:
                GameDataManager.Instance.ResetPosition(false);
                break;
            case ButtonType.复活:
                GameDataManager.Instance.ResetPosition(true);
                break;
            case ButtonType.主菜单:
                GameDataManager.Instance.BackToMainMenu();
                break;
        }
    }
}
