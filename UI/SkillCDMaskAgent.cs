using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCDMaskAgent : MonoBehaviour {

    [HideInInspector]
    public SkillInfoAgent skillInfoAgent;
    Image mask;
    Text time;

	// Use this for initialization
	void Start () {
        mask = GetComponent<Image>();
        time = GetComponentInChildren<Text>();
        mask.fillAmount = 0;
        MyTools.SetActive(time.gameObject, false);
    }
	
	// Update is called once per frame
	void Update () {
        if (skillInfoAgent)
        {
            if (!skillInfoAgent.isCD)
            {
                MyTools.SetActive(time.gameObject, true);
                mask.fillAmount = (skillInfoAgent.coolDownTime - skillInfoAgent.currentTime) / skillInfoAgent.coolDownTime;
                time.text = "<color=yellow>" + (skillInfoAgent.coolDownTime - skillInfoAgent.currentTime).ToString("F1") + "</color>";
            }
            else
            {
                mask.fillAmount = 0;
                MyTools.SetActive(time.gameObject, false);
            }
        }
	}
}
