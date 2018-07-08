using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirm : MonoBehaviour {

    public static Confirm Self;

    public GameObject UI;
    public Button Yes;
    public Button Cancel;
    public Text Message;

	// Use this for initialization
	void Awake () {
        Self = this;
        Cancel.onClick.AddListener(CloseUI);
        CloseUI();
	}

    public void NewConfirm(string massege,UnityEngine.Events.UnityAction onYes)
    {
        Message.text = massege;
        Yes.onClick.AddListener(onYes);
        Yes.onClick.AddListener(CloseUI);
        MyTools.SetActive(UI, true);
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
        Message.text = string.Empty;
        Yes.onClick.RemoveAllListeners();
    }
}
