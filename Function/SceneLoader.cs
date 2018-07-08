using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public static SceneLoader Instance;
    public float progress;
    bool isLoading;
    AsyncOperation asyncOperation;
    Scene loadScense;
    bool isDone;

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
		
	}*/
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isLoading)
        {
            progress = Mathf.Lerp(progress, asyncOperation.progress * 1.111111f, 0.5f);
            if (progress >= 0.99) isDone = true;
        }
    }

    public void Load(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }



    public IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync("LoadScene");
        isLoading = true;
        progress = 0;
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitUntil(() => isDone);
        Debug.Log("\"" + sceneName + "\"LoadFinished");
        isLoading = false;
        isDone = false;
        asyncOperation.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncOperation.isDone);
        QualitySettings.SetQualityLevel(3);
        ScreenFader.Instance.Fade(false);
        System.GC.Collect();
    }
}
