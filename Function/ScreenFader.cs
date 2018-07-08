using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {
    public static ScreenFader Instance;
    [Range(0.1f,3f)]
    public float FadeSpeed = 1.5f;
    public GameObject faderUI;
    public GameObject faderPrefab;
    Image filler;
    //[SerializeField]
    bool isFadeIn;
    //[SerializeField]
    bool isFadeOut;
    float blackTime;
    //[SerializeField]
    bool isAuto;
    float currentA = 1;
    public UnityEngine.Events.UnityEvent onFadeInStart;
    public UnityEngine.Events.UnityEvent onFadeInEnd;
    public UnityEngine.Events.UnityEvent onFadeOutStart;
    public UnityEngine.Events.UnityEvent onFadeOutEnd;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Use this for initialization
    /*void Start () {
        faderUI = Instantiate(faderPrefab, GameObject.FindWithTag("UIRoot").transform);
        filler = faderUI.GetComponentInChildren<Image>();
        filler.color = new Color(0, 0, 0, 1);
    }*/
	
	// Update is called once per frame
	void Update () {
        if (isFadeIn)
        {
            isFadeOut = false;
            if (currentA < 1) currentA += Time.deltaTime * FadeSpeed;
            else
            {
                currentA = 1;
                isFadeIn = false;
                filler.raycastTarget = false;
                onFadeInEnd.Invoke();
            }
            filler.color = new Color(0, 0, 0, currentA);
        }
        if(isFadeOut)
        {
            isFadeIn = false;
            if (currentA > 0) currentA -= Time.deltaTime * FadeSpeed;
            else
            {
                currentA = 0;
                isFadeOut = false;
                filler.raycastTarget = false;
                onFadeOutEnd.Invoke();
            }
            filler.color = new Color(0, 0, 0, currentA);
        }
        if(isAuto)
        {
            isFadeOut = false;
            if (currentA < 1) currentA += Time.deltaTime * FadeSpeed;
            else
            {
                isAuto = false;
                currentA = 1;
                filler.raycastTarget = false;
                StartCoroutine(WaitForSecondsForFadeOut(blackTime));
                onFadeInEnd.Invoke();
            }
            filler.color = new Color(0, 0, 0, currentA);
        }
	}

    public void Fade(bool fadeIn)
    {
        if (fadeIn)
        {
            if (!faderUI)
            {
                faderUI = Instantiate(faderPrefab, GameObject.FindWithTag("UIRoot").transform);
                filler = faderUI.GetComponentInChildren<Image>();
                filler.color = new Color(0, 0, 0, 0);
            }
            currentA = 0;
            isFadeIn = true;
            isFadeOut = false;
            filler.raycastTarget = true;
            onFadeInStart.Invoke();
        }
        else
        {
            if (!faderUI)
            {
                faderUI = Instantiate(faderPrefab, GameObject.FindWithTag("UIRoot").transform);
                filler = faderUI.GetComponentInChildren<Image>();
                filler.color = new Color(0, 0, 0, 1);
            }
            currentA = 1;
            isFadeOut = true;
            isFadeIn = false;
            filler.raycastTarget = true;
            onFadeOutStart.Invoke();
        }
    }

    public void AutoFadeInAndOut(float blackTime)
    {
        isAuto = true;
        this.blackTime = blackTime;
    }

    IEnumerator WaitForSecondsForFadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        blackTime = 0;
        Fade(false);
    }
}
