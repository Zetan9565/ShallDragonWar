using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectAgent : MonoBehaviour {

    [Header("ForWeapon")]
    public List<GameObject> weaponsInHand;
    public List<GameObject> weaponsInBack;
    public Collider frontTrigger;
    public XftWeapon.XWeaponTrail[] frontTrails;
    public Collider backTrigger;
    public XftWeapon.XWeaponTrail[] backTrails;

    [Header("ForLocomotion")]
    public PlayerController playerController;
    public List<GameObject> GatherTools;
    public Transform normalRoot;
    public GameObject normalBips;
    public GameObject normalBody;
    public Transform mountRoot;
    public GameObject mountBips;
    public GameObject mountBody;

    // Use this for initialization
    /*void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/

    public void OnMountIndirect()
    {
        PlayerLocomotionManager.Instance.OnMount();
    }
    public void OnDismountIndirect()
    {
        PlayerLocomotionManager.Instance.OnDismount();
    }
}
