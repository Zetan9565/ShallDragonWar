using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultFuncButton : MonoBehaviour {

	// Use this for initialization
	/*void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/

    public void OnPlayerSelectorEnterClick()
    {
        PlayerSelector.Instance.OnEntherButtonClick();
    }

    public void OnPlayerSelectorSelect1(UnityEngine.UI.Button enterButton)
    {
        PlayerSelector.Instance.enterButton = enterButton;
        PlayerSelector.Instance.Selector(1);
    }
    public void OnPlayerSelectorSelect2(UnityEngine.UI.Button enterButton)
    {
        PlayerSelector.Instance.enterButton = enterButton;
        PlayerSelector.Instance.Selector(2);
    }
    public void OnPlayerSelectorSelect3(UnityEngine.UI.Button enterButton)
    {
        PlayerSelector.Instance.enterButton = enterButton;
        PlayerSelector.Instance.Selector(3);
    }

    public void OnLoadScene(string sceneNmae)
    {
        SceneLoader.Instance.Load(sceneNmae);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
