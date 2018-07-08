using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour {

    public static TalkManager Instance;
    public readonly static string DataName = "/Story.zetan";
    public string saveDataName;
    public bool isTalking;
    public Fungus.Flowchart Flowchart;

    private void Awake()
    {
        Instance = this;
        talkButton.onClick.AddListener(OnTalkButtonClick);
        MyTools.SetActive(talkButton.gameObject, false);
        Flowchart = FindObjectOfType<Fungus.Flowchart>();
    }

    public UnityEngine.UI.Button talkButton;
    public UnityEngine.UI.Text talkerName;
    public TalkTrigger talkTrigger;

    public void OnTalkButtonClick()
    {
        Fungus.Flowchart.BroadcastFungusMessage(talkTrigger.message);
        MyTools.SetActive(talkButton.gameObject, false);
        OnTalkStart();
    }

    public void CanTalk(TalkTrigger trigger)
    {
        talkTrigger = trigger;
        talkerName.text = talkTrigger.talkerInfo.talkerName;
        MyTools.SetActive(talkButton.gameObject, true);
    }

    public void CantTalk()
    {
        //Debug.Log("Cant");
        talkTrigger = null;
        talkerName.text = string.Empty;
        MyTools.SetActive(talkButton.gameObject, false);
    }

    public void OnTalkStart()
    {
        isTalking = true;
        PlayerInfoManager.Instance.Player.GetComponent<PlayerUserController>().enabled = false;
        if(talkTrigger) talkTrigger.onTalkStart.Invoke();
    }

    public void OnTalkFinish()
    {
        isTalking = false;
        if(!PlayerInfoManager.Instance.PlayerInfo.IsMounting) PlayerInfoManager.Instance.Player.GetComponent<PlayerUserController>().enabled = true;
        if(talkTrigger) talkTrigger.onTalkFinish.Invoke();
    }

    public void OnTalkAgain()
    {
        isTalking = true;
        PlayerInfoManager.Instance.Player.GetComponent<PlayerUserController>().enabled = false;
        if (talkTrigger) talkTrigger.onOnceTalk.Invoke();
    }

    public void Save(string path, string key = "",bool encrypt = false)
    {
        try
        {
            List<TalkerInfo> takerInfos = new List<TalkerInfo>();
            foreach (TalkTrigger tt in FindObjectsOfType<TalkTrigger>())
                takerInfos.Add(tt.talkerInfo);
            if (encrypt && key.Length == 32) System.IO.File.WriteAllText(path + DataName, Encryption.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(takerInfos), key), System.Text.Encoding.UTF8);
            else System.IO.File.WriteAllText(path + DataName, Newtonsoft.Json.JsonConvert.SerializeObject(takerInfos), System.Text.Encoding.UTF8);
        }
        catch(System.Exception ex)  { Debug.Log(ex.Message); }
    }

    public void Load(string path, string key = "", bool dencrypt = false)
    {
        try
        {
            List<TalkerInfo> takerInfos;
            if (dencrypt && key.Length == 32) takerInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TalkerInfo>>(Encryption.Dencrypt(System.IO.File.ReadAllText(path + DataName, System.Text.Encoding.UTF8), key));
            else takerInfos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TalkerInfo>>(System.IO.File.ReadAllText(path + DataName, System.Text.Encoding.UTF8));
            TalkTrigger[] talkTriggers = FindObjectsOfType<TalkTrigger>();
            foreach(TalkerInfo ti in takerInfos)
            {
                foreach(TalkTrigger tt in talkTriggers)
                    if(ti.talkerID == tt.talkerInfo.talkerID)
                    {
                        //Debug.Log("Find");
                        tt.talkerInfo.hasTalk = ti.hasTalk;
                        if(ti.hasTalk) tt.onOnceTalk.Invoke();
                    }
            }
        }
        catch (System.Exception ex) { Debug.Log(ex.Message); }
    }
}
