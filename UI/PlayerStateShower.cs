using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateShower : MonoBehaviour {

    public Image HP;
    public Image MP;
    public Image Endurance;
    public Image Exp;

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
		if(PlayerInfoManager.Instance.PlayerInfo != null)
        {
            HP.fillAmount = (float)PlayerInfoManager.Instance.PlayerInfo.Current_HP / (float)PlayerInfoManager.Instance.PlayerInfo.HP;
            MP.fillAmount = (float)PlayerInfoManager.Instance.PlayerInfo.Current_MP / (float)PlayerInfoManager.Instance.PlayerInfo.MP;
            Endurance.fillAmount = (float)PlayerInfoManager.Instance.PlayerInfo.Current_Endurance / (float)PlayerInfoManager.Instance.PlayerInfo.Endurance;
        }
	}
}
