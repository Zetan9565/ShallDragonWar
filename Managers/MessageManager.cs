using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {
    public Text message;

	// Use this for initialization
	/*void Start () {

	}*/
	
	// Update is called once per frame
	void Update () {
        if (SystemMessages.Count() > 0)
        {
            message.text = "[" + System.DateTime.Now.ToString("HH:mm:ss") +"]"+ SystemMessages.LatestMessage();
            SystemMessages.Clear();
        }
	}
}
