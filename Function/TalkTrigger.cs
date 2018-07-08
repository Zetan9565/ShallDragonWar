using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkerInfo
{
    public string talkerID;
    public string talkerName;
    public bool hasTalk;
}
[System.Serializable]
public class TalkerVariable
{
    public string varName;
    public string varString;
    public bool varBool;
    public int varInt;
    public float varFloat;
}

public class TalkTrigger : MonoBehaviour {

    public TalkerInfo talkerInfo;
    public string message;
    public List<TalkerVariable> talkerVars;
    public bool talkAble = true;
    public bool TalkAble { get; set; }
    public UnityEngine.Events.UnityEvent onTalkStart;
    public UnityEngine.Events.UnityEvent onOnceTalk;
    public UnityEngine.Events.UnityEvent onTalkFinish;

    private void Awake()
    {
        TalkAble = talkAble;
        onOnceTalk.AddListener(SetVariable);
        onTalkFinish.AddListener(delegate { talkerInfo.hasTalk = true;SetVariable(); });
    }

    public void SetVariable()
    {
        if (!talkerInfo.hasTalk) talkerInfo.hasTalk = true;
        foreach (TalkerVariable tv in talkerVars)
        {
            TalkManager.Instance.Flowchart.SetBooleanVariable(tv.varName, tv.varBool);
            TalkManager.Instance.Flowchart.SetStringVariable(tv.varName, tv.varString);
            TalkManager.Instance.Flowchart.SetIntegerVariable(tv.varName, tv.varInt);
            TalkManager.Instance.Flowchart.SetFloatVariable(tv.varName, tv.varFloat);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && TalkAble)
        {
            if (!TalkManager.Instance.talkTrigger)
                TalkManager.Instance.CanTalk(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && TalkAble)
        {
            if (!TalkManager.Instance.talkTrigger && other.enabled)
                TalkManager.Instance.CanTalk(this);
            else if (!other.enabled && TalkManager.Instance.talkTrigger && TalkManager.Instance.talkTrigger == this)
                TalkManager.Instance.CantTalk();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (TalkManager.Instance.talkTrigger && TalkManager.Instance.talkTrigger == this)
                TalkManager.Instance.CantTalk();
        }
    }

    
}
