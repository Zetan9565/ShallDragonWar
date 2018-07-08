using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlatformSelector : MonoBehaviour {

    public List<GameObject> androidButtons = new List<GameObject>();
    public List<GameObject> windowsButtons = new List<GameObject>();
    public List<ButtonHandler> unActiveBtnHandlers = new List<ButtonHandler>();

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            foreach (GameObject ui in androidButtons)
                MyTools.SetActive(ui, true);
            foreach (ButtonHandler bh in unActiveBtnHandlers)
                bh.active = true;
            FindObjectOfType<MapMagic.MapMagic>().hideFarTerrains = true;
        }
        else
        {
            foreach (GameObject ui in androidButtons)
                MyTools.SetActive(ui, false);
            foreach (ButtonHandler bh in unActiveBtnHandlers)
                bh.active = false;
            FindObjectOfType<MapMagic.MapMagic>().hideFarTerrains = false;
        }
    }
}
