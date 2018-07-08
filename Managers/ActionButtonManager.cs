using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager: MonoBehaviour {

    public static ActionButtonManager Instance;

    public GameObject fightActions;
    public GameObject notFightActions;
    public GameObject horseButtons;
    public Button sitButton;
    public bool isInit;

	// Use this for initialization
	void Init () {
        if (PlayerInfoManager.Instance.PlayerInfo.IsFighting) EnableFightActions();
        else EnableNotFightActions();
        sitButton.onClick.AddListener(PlayerLocomotionManager.Instance.SetSitAnima);
        isInit = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!PlayerInfoManager.Instance.isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.IsFighting && !PlayerInfoManager.Instance.PlayerInfo.IsMounting) EnableFightActions();
        else EnableNotFightActions();
        MyTools.SetActive(horseButtons, PlayerInfoManager.Instance.PlayerInfo.IsMounting);
	}

    public void EnableFightActions()
    {
        MyTools.SetActive(notFightActions, false);
        fightActions.SetActive(true);
    }
    public void EnableNotFightActions()
    {
        MyTools.SetActive(notFightActions, true);
        MyTools.SetActive(fightActions, false);
    }
}
