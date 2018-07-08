using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Destroy : MonoBehaviour {
    public float DestroyTime = 2;

    AudioSource fxSound;

    private void Start()
    {
        fxSound = GetComponent<AudioSource>();
        if (fxSound)
        {
            GameSettingManager.Instance.fxList.Add(fxSound);
            GameSettingManager.Instance.ChangeFxVolume();
        }
    }

    // Use this for initialization
    void OnEnable () {
        StartCoroutine(Deactive());
	}

    IEnumerator Deactive()
    {
        yield return new WaitForSeconds(DestroyTime);
        ObjectPoolManager.Instance.Put(gameObject);
    }

    private void OnDisable()
    {
        StopCoroutine(Deactive());
    }
}
