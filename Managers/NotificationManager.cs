using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour {
    public static NotificationManager Instance;

    public GameObject NotificationPrefab;
    public List<GameObject> notifications;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        notifications = new List<GameObject>();
	}
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

    public void NewNotification(string message)
    {
        if (!notifications.Exists(n => !n.activeSelf))
        {
            GameObject notification = Instantiate(NotificationPrefab, transform) as GameObject;
            notification.GetComponent<NotificationAgent>().Message.text = message;
            notifications.Add(notification);
        }
        else
        {
            foreach (GameObject n in notifications)
                if (!n.activeSelf)
                {
                    n.GetComponent<NotificationAgent>().Message.text = message;
                    n.SetActive(true);
                }
        }
    }
}
