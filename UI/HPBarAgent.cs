using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarAgent : MonoBehaviour {

    public UnityEngine.UI.Image HP;
    public EnemyInfoAgent enemyInfoAgent;

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
		if(enemyInfoAgent)
        {
            (transform as RectTransform).anchoredPosition = GetPosition();
            ShowHPValue();
            if (!enemyInfoAgent.IsAlive)
            {
                ResetBar();
            }
        }
	}

    public void SetEnermy(EnemyInfoAgent enemyInfo)
    {
        enemyInfoAgent = enemyInfo;
        enemyInfoAgent.HPBar = this;
        ShowBar();
    }

    Vector2 GetPosition()
    {
        Vector2 temppos = Camera.main.WorldToScreenPoint(enemyInfoAgent.HPBarPoint.position);
        //Debug.Log(temppos);
        Vector2 uiPos = new Vector2(temppos.x - Screen.width / 2, temppos.y - Screen.height / 2);
        return uiPos;
    }

    void ShowHPValue()
    {
        HP.fillAmount = enemyInfoAgent.Current_HP / enemyInfoAgent.HP;
    }

    public void ResetBar()
    {
        enemyInfoAgent = null;
        MyTools.SetActive(gameObject, false);
    }

    public void HideBar()
    {
        MyTools.SetActive(gameObject, false);
    }

    public void ShowBar()
    {
        if (!enemyInfoAgent)
        {
            ResetBar();
            return;
        }
        if (gameObject.activeSelf) return;
        (transform as RectTransform).anchoredPosition = GetPosition();
        gameObject.SetActive(true);
    }
}
