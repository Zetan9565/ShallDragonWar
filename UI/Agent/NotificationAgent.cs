using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationAgent : MonoBehaviour {
    public Text Message;

    public void DistroySelf()
    {
        MyTools.SetActive(gameObject, false);
    }
}
