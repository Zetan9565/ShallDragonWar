using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineManager : MonoBehaviour {

    public static TimeLineManager Instance;
    public List<Light> lights = new List<Light>();
    [HideInInspector]
    public usky.uSkyTimeline uSkyTimeline;
    public float currentTime;

    public Text timeShower;

    private void Awake()
    {
        Instance = this;
        uSkyTimeline = FindObjectOfType<usky.uSkyTimeline>();
    }

    // Use this for initialization
    void Start () {
        if((currentTime >= 18.5f && currentTime < 24) || (currentTime >= 0 && currentTime < 6)) StartCoroutine(CheckTime(true));
        else StartCoroutine(CheckTime(false));
	}
	
	// Update is called once per frame
	void Update () {
        timeShower.text = GetTimeString();
        currentTime = uSkyTimeline.Timeline;
    }

    public string GetTimeString()
    {
        if ((uSkyTimeline.Timeline >= 23 && uSkyTimeline.Timeline <= 24) || (uSkyTimeline.Timeline >= 0 && uSkyTimeline.Timeline < 1))
            return "子时";
        else if (uSkyTimeline.Timeline >= 1 && uSkyTimeline.Timeline < 3)
            return "丑时";
        else if (uSkyTimeline.Timeline >= 3 && uSkyTimeline.Timeline < 5)
            return "寅时";
        else if (uSkyTimeline.Timeline >= 5 && uSkyTimeline.Timeline < 7)
            return "卯时";
        else if (uSkyTimeline.Timeline >= 7 && uSkyTimeline.Timeline < 9)
            return "辰时";
        else if (uSkyTimeline.Timeline >= 9 && uSkyTimeline.Timeline < 11)
            return "巳时";
        else if (uSkyTimeline.Timeline >= 11 && uSkyTimeline.Timeline < 13)
            return "午时";
        else if (uSkyTimeline.Timeline >= 13 && uSkyTimeline.Timeline < 15)
            return "未时";
        else if (uSkyTimeline.Timeline >= 15 && uSkyTimeline.Timeline < 17)
            return "申时";
        else if (uSkyTimeline.Timeline >= 17 && uSkyTimeline.Timeline < 19)
            return "酉时";
        else if (uSkyTimeline.Timeline >= 19 && uSkyTimeline.Timeline < 21)
            return "戌时";
        else
            return "亥时";
    }

    IEnumerator CheckTime(bool waitDay)
    {
        if (waitDay)
        {
            Light[] tempLights = new Light[0];
            tempLights = FindObjectsOfType<Light>();
            lights.Clear();
            foreach (Light light in tempLights)
                if (!light.gameObject.GetComponent<usky.uSkySun>() && light.tag != "FixLight") lights.Add(light);
            foreach (Light light in lights)
                if(light) light.enabled = true;
            yield return new WaitUntil(() => (currentTime < 18.5f && currentTime >= 6));
            StartCoroutine(CheckTime(false));
        }
        else
        {
            Light[] tempLights = new Light[0];
            tempLights = FindObjectsOfType<Light>();
            lights.Clear();
            foreach (Light light in tempLights)
                if (!light.gameObject.GetComponent<usky.uSkySun>() && light.tag != "FixLight") lights.Add(light);
            foreach (Light light in lights)
                if(light) light.enabled = false;
            yield return new WaitUntil(() => (currentTime >= 18.5f && currentTime < 24) || (currentTime >= 0 && currentTime < 6));
            StartCoroutine(CheckTime(true));
        }
    }

    public void CheckTime()
    {
        uSkyTimeline = FindObjectOfType<usky.uSkyTimeline>();
        if ((currentTime >= 17.5 && currentTime < 24) || (currentTime >= 0 && currentTime < 6)) StartCoroutine(CheckTime(true));
        else StartCoroutine(CheckTime(false));
    }

    public void LoadTime(float time)
    {
        uSkyTimeline.Timeline = time;
        CheckTime();
    }
}
