using MyEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{

    public static GameDataManager Instance;

    readonly string folder1 = "/PlayerData/ZetanData_1";
    readonly string folder2 = "/PlayerData/ZetanData_2";
    readonly string folder3 = "/PlayerData/ZetanData_3";
    readonly string autoSaveFolder = "/PlayerData/AutoSave";
    //readonly bool globalEncrypt = true;
    readonly string globalKey = "zetangamedatezetangamdatezetanga";

    public GameObject UI;
    public GameObject UIPrefab;

    public bool isSaving;

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
    /*void Start()
    {
        Debug.Log(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        Debug.Log(Application.dataPath.Replace('/', '\\'));
    }*/

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.End)) Save(1);
        if (Input.GetKeyDown(KeyCode.Home)) Load(1);
    }*/


    public void Save(int index)
    {
        CloseUI();
        try
        {
            string date = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string folder = string.Empty;
            switch (index)
            {
                case 1: folder = folder1; break;
                case 2: folder = folder2; break;
                case 3: folder = folder3; break;
                default: folder = autoSaveFolder; break;
            }
            if (!System.IO.Directory.Exists(Application.persistentDataPath + folder))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.persistentDataPath + folder);
                di.Create();
            }
            PlayerInfoManager.Instance.SavePlayerInfo(Application.persistentDataPath + folder, globalKey, true);
            PlayerSkillManager.Instance.SaveToFile(Application.persistentDataPath + folder, globalKey, true);
            ShopManager.Instance.SaveToFile(Application.persistentDataPath + folder, globalKey, true);
            TalkManager.Instance.Save(Application.persistentDataPath + folder, globalKey, true);
            string[] datas =
            {
            date,
            PlayerInfoManager.Instance.PlayerInfo.Level.ToString(),
            PlayerInfoManager.Instance.PlayerInfo.characterInfo.Name,
            Encryption.Encrypt(PlayerInfoManager.Instance.PlayerInfo.ID.ToString(),globalKey),
            Encryption.Encrypt(TimeLineManager.Instance.currentTime.ToString(),globalKey),
            Encryption.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(SelectClosestSavePoint().transform.position), globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath + folder + PlayerInfoManager.DataName),globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath + folder + BagInfo.DataName),globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath + folder + WarehouseInfo.DataName),globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath + folder + PlayerSkillManager.DataName1), globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath + folder + PlayerSkillManager.DataName2), globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath + folder + ShopManager.DataName),globalKey),
            Encryption.Encrypt(MyTools.GetMD5(Application.persistentDataPath+folder+TalkManager.DataName),globalKey)
            };
            System.IO.File.WriteAllLines(Application.persistentDataPath + folder + "/Data", datas, System.Text.Encoding.UTF8);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    public void Load(int index)
    {
        CloseUI();
        StartCoroutine(LoadData(index));
    }

    IEnumerator LoadData(int index)
    {
        string folder = string.Empty;
        switch (index)
        {
            case 1: folder = folder1; break;
            case 2: folder = folder2; break;
            case 3: folder = folder3; break;
            default: folder = autoSaveFolder; break;
        }
        if (System.IO.File.Exists(Application.persistentDataPath + folder + "/Data"))
        {
            bool dataHealth = false;
            string[] data = new string[13];
            data = System.IO.File.ReadAllLines(Application.persistentDataPath + folder + "/Data", System.Text.Encoding.UTF8);
            int ID = 0;
            float time = 0;
            Vector3 pos = new Vector3();
            try
            {
                ID = int.Parse(Encryption.Dencrypt(data[3], globalKey));
                //Debug.Log(ID);
                time = float.Parse(Encryption.Dencrypt(data[4], globalKey));
                //Debug.Log(time);
                pos = Newtonsoft.Json.JsonConvert.DeserializeObject<Vector3>(Encryption.Dencrypt(data[5], globalKey));
                //Debug.Log(pos);
                dataHealth = (ID == 100001 || ID == 100002 || ID == 100003) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + PlayerInfoManager.DataName, Encryption.Dencrypt(data[6], globalKey)) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + BagInfo.DataName, Encryption.Dencrypt(data[7], globalKey)) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + WarehouseInfo.DataName, Encryption.Dencrypt(data[8], globalKey)) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + PlayerSkillManager.DataName1, Encryption.Dencrypt(data[9], globalKey)) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + PlayerSkillManager.DataName2, Encryption.Dencrypt(data[10], globalKey)) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + ShopManager.DataName, Encryption.Dencrypt(data[11], globalKey)) &&
                    MyTools.CompareMD5(Application.persistentDataPath + folder + TalkManager.DataName, Encryption.Dencrypt(data[12], globalKey));
            }
            catch
            {
                dataHealth = false;
            }
            if (dataHealth)
            {
                yield return StartCoroutine(SceneLoader.Instance.LoadScene("WorldMap"));
                switch (ID)
                {
                    case 100001: PlayerSelector.Instance.characterType = PlayerCharacterType.Boy; break;
                    case 100002: PlayerSelector.Instance.characterType = PlayerCharacterType.Girl; break;
                    case 100003: PlayerSelector.Instance.characterType = PlayerCharacterType.LittleGirl; break;
                }
                yield return StartCoroutine(PlayerSelector.Instance.InitCharacter());
                yield return new WaitUntil(() => PlayerInfoManager.Instance.isInit && PlayerSkillManager.Instance && PlayerLocomotionManager.Instance);
                PlayerInfoManager.Instance.LoadPlayerInfo(Application.persistentDataPath + folder, globalKey, true);
                PlayerSkillManager.Instance.LoadFromFile(Application.persistentDataPath + folder, globalKey, true);
                ShopManager.Instance.LoadFromFile(Application.persistentDataPath + folder, globalKey, true);
                PlayerLocomotionManager.Instance.playerController.transform.position = pos;
                FindObjectOfType<MalbersAnimations.Animal>().transform.position = pos;
                TimeLineManager.Instance.LoadTime(time);
                TalkManager.Instance.Load(Application.persistentDataPath + folder, globalKey, true);
            }
            else
            {
                Debug.Log("存档损坏");
            }
        }
        else
        {
            Debug.Log("存档不存在");
        }
    }

    public bool GetDataInfo(int index, out string[] info)
    {
        info = new string[12];
        try
        {
            string folder = string.Empty;
            switch (index)
            {
                case 1: folder = folder1; break;
                case 2: folder = folder2; break;
                case 3: folder = folder3; break;
            }
            if (System.IO.File.Exists(@Application.persistentDataPath + folder + "/Data"))
            {
                info = System.IO.File.ReadAllLines(@Application.persistentDataPath + folder + "/Data", System.Text.Encoding.UTF8);
                return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
    }

    public void ResetPosition(bool relive)
    {
        SelectClosestSavePoint().ResetToHere(relive);
    }

    SaveLoadPoint SelectClosestSavePoint()
    {
        List<SaveLoadPoint> savePoints = new List<SaveLoadPoint>();
        SaveLoadPoint[] points = FindObjectsOfType<SaveLoadPoint>();
        foreach (SaveLoadPoint sp in points)
            savePoints.Add(sp);
        savePoints.Sort((x, y) => x.GetDistance().CompareTo(y.GetDistance()));
        SaveLoadPoint closest = savePoints[0];
        savePoints.Clear();
        return closest;
    }

    public void OpenUI()
    {
        if (!UI) UI = Instantiate(UIPrefab, GameObject.FindWithTag("UIRoot").transform);
        else MyTools.SetActive(UI.gameObject, true);
    }

    public void CloseUI()
    {
        if(UI) MyTools.SetActive(UI, false);
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
