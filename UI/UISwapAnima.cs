using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UISwapAnima : MonoBehaviour
{
    public enum AnimaPlayModle
    {
        ChangeScale,
        ChangePosition
    }
    public float AnimaPlaySpeed = 5.0f;
    [Tooltip("请勿在游玩模式更改此项!")]
    public AnimaPlayModle animaPlayMode;
    Vector3 OriginScale;
    Vector3 OriginPosition;
    bool isOpenning;
    bool isClosing;
    public bool isOpen;
    public List<Scrollbar> fiexdScrollbars;
    //[HideInInspector]
    public bool DiyValues;
    public List<float> barValues;
    public UnityEngine.Events.UnityEvent onCloseFinished;
    public UnityEngine.Events.UnityEvent onOpenFinished;

    float time;
    // Use this for initialization
    void Start()
    {
        OriginScale = transform.localScale;
        OriginPosition = transform.localPosition;
        if (animaPlayMode == AnimaPlayModle.ChangeScale && !DiyValues)
        {
            barValues = new List<float>();
            foreach (Scrollbar bar in fiexdScrollbars)
                barValues.Add(bar.value);
        }
        if (!isOpen)
        {
            if (animaPlayMode == AnimaPlayModle.ChangeScale) transform.localScale = Vector3.zero;
            else transform.localPosition = OriginPosition + new Vector3(0, Screen.height, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClosing)
        {
            switch (animaPlayMode)
            {
                case AnimaPlayModle.ChangeScale:
                    time += Time.deltaTime;
                    transform.localScale = Vector3.Lerp(OriginScale, Vector3.zero, time * AnimaPlaySpeed);
                    if (transform.localScale == Vector3.zero)
                    {
                        isClosing = false;
                        time = 0;
                        isOpen = false;
                        onCloseFinished.Invoke();
                        if(!DiyValues)
                        {
                            barValues.Clear();
                            foreach (Scrollbar bar in fiexdScrollbars)
                                barValues.Add(bar.value);
                        }
                    }
                    break;
                case AnimaPlayModle.ChangePosition:
                    time += Time.deltaTime;
                    transform.localPosition = Vector3.Lerp(OriginPosition, OriginPosition + new Vector3(0, Screen.height, 0), time * AnimaPlaySpeed);
                    if (transform.localPosition == OriginPosition + new Vector3(0, Screen.height, 0))
                    {
                        isClosing = false;
                        time = 0;
                        isOpen = false;
                        onCloseFinished.Invoke();
                    }
                    break;
            }
        }
        if (isOpenning)
        {
            switch (animaPlayMode)
            {
                case AnimaPlayModle.ChangeScale:
                    time += Time.deltaTime;
                    transform.localScale = Vector3.Lerp(Vector3.zero, OriginScale, time * AnimaPlaySpeed);
                    if (transform.localScale == OriginScale)
                    {
                        isOpenning = false;
                        time = 0;
                        isOpen = true;
                        onOpenFinished.Invoke();
                        int i = 0;
                        foreach (Scrollbar bar in fiexdScrollbars)
                        {
                            bar.value = barValues[i];
                            i++;
                        }
                    }
                    break;
                case AnimaPlayModle.ChangePosition:
                    time += Time.deltaTime;
                    transform.localPosition = Vector3.Lerp(OriginPosition + new Vector3(0, Screen.height, 0), OriginPosition, time * AnimaPlaySpeed);
                    if (transform.localPosition == OriginPosition)
                    {
                        isOpenning = false;
                        time = 0;
                        isOpen = true;
                        onOpenFinished.Invoke();
                    }
                    break;
            }
        }
    }

    public void OnOpen()
    {
        //barValues.Clear();
        if (!isOpen)
            isOpenning = true;
    }

    public void OnClose()
    {
        //barValues.Clear();
        if (isOpen)
            isClosing = true;
    }
}
