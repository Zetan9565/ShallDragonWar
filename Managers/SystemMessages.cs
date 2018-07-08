using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMessages : MonoBehaviour {
    static List<string> messageList = new List<string>();

    public static string LatestMessage()
    {
        if (messageList.Count <= 0) return string.Empty;
        return messageList[messageList.Count - 1];
    }

    public static void AddMessage(string message)
    {
        messageList.Add(message);
    }

    public static void Clear()
    {
        messageList.Clear();
    }

    public static int Count()
    {
        return messageList.Count;
    }
}
