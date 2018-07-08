using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadProgressShower : MonoBehaviour {

    UnityEngine.UI.Image progress;

	// Use this for initialization
	void Start () {
        progress = GetComponent<UnityEngine.UI.Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneLoader.Instance)
            progress.fillAmount = SceneLoader.Instance.progress;
	}
}
