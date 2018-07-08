using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeProgressBarManager : MonoBehaviour {

    public static TimeProgressBarManager Instance;
    public GameObject progressBar;
    [Space]
    public Text Title;
    public Image fillArea;
    public Text leftTimeText;
    public float currentTime;
    public float totalTime;
    [HideInInspector]
    public bool isStart;
    [HideInInspector]
    public UnityEvent onStart;
    [HideInInspector]
    public UnityEvent onCancel;
    [HideInInspector]
    public UnityEvent onEnd;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        CancelProgress();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= totalTime) { isStart = false; EndProgress(); }
            fillArea.fillAmount = currentTime / totalTime;
            leftTimeText.text = currentTime.ToString("F1") + " 秒";
        }
    }

    public void StartProgress()
    {
        //Debug.Log("StartProgress");
        MyTools.SetActive(progressBar.gameObject, true);
        isStart = true;
        onStart.Invoke();
    }

    public void CancelProgress()
    {
        //Debug.Log("CancelProgress");
        MyTools.SetActive(progressBar.gameObject, false);
        isStart = false;
        currentTime = 0;
        totalTime = 0;
        leftTimeText.text = string.Empty;
        onCancel.Invoke();
        onStart.RemoveAllListeners();
        onCancel.RemoveAllListeners();
        onEnd.RemoveAllListeners();
    }

    public void EndProgress()
    {
        //Debug.Log("EndProgress");
        MyTools.SetActive(progressBar.gameObject, false);
        isStart = false;
        currentTime = 0;
        totalTime = 0;
        leftTimeText.text = string.Empty;
        onEnd.Invoke();
        onStart.RemoveAllListeners();
        onCancel.RemoveAllListeners();
        onEnd.RemoveAllListeners();
    }

    public void NewTimeProgress(string title, float time)
    {
        //Debug.Log("NewProgress");
        Title.text = title;
        totalTime = time;
        currentTime = 0;
        StartProgress();
    }
}
