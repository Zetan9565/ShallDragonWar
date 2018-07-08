using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour {

    public static PlayerSelector Instance;

    public MyEnums.PlayerCharacterType characterType;

    public GameObject boyPrefab;
    public GameObject girlPrefab;
    public GameObject littleGirlPrefab;
    public Button enterButton;
    bool haSelect;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Selector(int select)
    {
        switch (select)
        {
            case 1:
                characterType = MyEnums.PlayerCharacterType.Boy;
                break;
            case 2:
                characterType = MyEnums.PlayerCharacterType.Girl;
                break;
            case 3:
                characterType = MyEnums.PlayerCharacterType.LittleGirl;
                break;
        }
        if(enterButton) MyTools.SetActive(enterButton.gameObject, true);
    }

    public void Init()
    {
        //Debug.Log("Init");
        StartCoroutine(InitCharacter());
    }

    public void OnEntherButtonClick()
    {
        StartCoroutine(Enter());
    }

    IEnumerator Enter()
    {
        yield return StartCoroutine(SceneLoader.Instance.LoadScene("WorldMap"));
        StartCoroutine(InitCharacter());
    }

    public IEnumerator InitCharacter()
    {
        yield return new WaitUntil(() => PlayerInfoManager.Instance && PlayerLocomotionManager.Instance && PlayerWeaponManager.Instance);
        switch (characterType)
        {
            case MyEnums.PlayerCharacterType.Boy: PlayerInfoManager.Instance.Init(boyPrefab); break;
            case MyEnums.PlayerCharacterType.Girl: PlayerInfoManager.Instance.Init(girlPrefab); break;
            case MyEnums.PlayerCharacterType.LittleGirl: PlayerInfoManager.Instance.Init(littleGirlPrefab); break;
        }
    }
}
