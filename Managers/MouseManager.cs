using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftAlt) || CheckMouseState())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CameraFollow.Camera.rotateAble = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CameraFollow.Camera.rotateAble = true;
        }
	}

    bool CheckMouseState()
    {
        if (BagManager.Instance.UI.activeSelf || WarehouseManager.Instance.UI.activeSelf || ShopManager.Instance.UI.activeSelf || PlayerSkillManager.Instance.UI.activeSelf || 
            EquipmentsManager.Instance.UI.activeSelf || PickUpManager.Instance.UI.activeSelf || Confirm.Self.UI.activeSelf || GameSettingManager.Instance.UI.activeSelf || TalkManager.Instance.isTalking)
            return true;
        return false;
    }
}