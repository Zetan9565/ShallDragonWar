using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo
{
    public string SkillID;
    public string SkillName;
    public int skillLevel;
    public int subMP;
    public int recHPWhenHit;
    public int recMPWhenHit;
    public float attackMultiple;
    public float coolDownTime;
    public float statuRate;
    public float statuDuration;
}

public class SkillInfoAgent : MonoBehaviour
{

    public string skillID;
    public string skillName;
    public Sprite skillIcon;
    public MyEnums.WeaponType skillWeapon;
    [TextArea]
    public string description;
    public int skillLevel;
    public int subMP;
    public int recHPWhenHit;
    public int recMPWhenHit;
    public float attackMultiple;
    public float coolDownTime;
    public bool usableWhenInCD;
    public string attachBuff;
    public string attackBuff;
    public Statu attachStatu;
    public float statuRate;
    public float statuDuration;
    public float subMultiple;
    [TextArea]
    [Tooltip("[ATKM]表示攻击倍数位置，[ATKS]表示蓄劲时减伤倍数，[HREC]表示体力回复量位置，[MREC]表示真气回复量位置，[STA]表示状态名位置，[STAR]表示状态概率位置，[STAD]表示状态时间位置")]
    public string effectFormat;
    [Header("升级相关")]
    public bool isNormalAtk;
    public int Add_Sub;
    public int Sub_CD;
    public int Add_ATKMult;
    public float Add_StatuRate;
    public float Add_StatuDura;
    public int Add_HPRec;
    public int Add_MPRec;

    //[HideInInspector]
    public float currentTime;
    //[HideInInspector]
    public bool isCD;
    public bool isCDWhenUse;
    //[HideInInspector]
    public bool statuEffcted;

    public GameObject UI;
    public GameObject UIPrefab;
    public Button IconButton;
    public Image Icon;
    public Text Name;

    public bool isEnemySkill;

    // Use this for initialization
    void Awake()
    {
        if(!isEnemySkill) StartCoroutine(AddThisToList());
        isCD = true;
    }

    IEnumerator AddThisToList()
    {
        yield return new WaitUntil(() => PlayerSkillManager.Instance);
        if (!PlayerSkillManager.Instance.skillsList.Contains(this) && !GetComponentInParent<EnemySkillsAgent>())
            PlayerSkillManager.Instance.skillsList.Add(this);
        if (isNormalAtk)
        {
            for (int i = 0; i < PlayerInfoManager.Instance.PlayerInfo.Level; i++)
                OnLevelUp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCD)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= coolDownTime)
            {
                currentTime = 0;
                isCD = true;
            }
        }
    }

    public void OnUsed()
    {
        //Debug.Log("使用" + skillID);
        isCDWhenUse = isCD;
        if (!isNormalAtk || coolDownTime > 0) isCD = false;
        if(!isEnemySkill) PlayerInfoManager.Instance.PlayerInfo.Current_MP -= subMP;
    }

    public void OnIconClick()
    {
        PlayerSkillManager.Instance.ShowSkillInfo(this);
    }

    public void OnLevelUp()
    {
        if (skillLevel >= 10 && !isNormalAtk) return;
        if (PlayerInfoManager.Instance.PlayerInfo.SkillPointOne <= 0 && !isNormalAtk)
        {
            NotificationManager.Instance.NewNotification("技能点不足");
            return;
        }
        if (skillWeapon == PlayerWeaponManager.Instance.weaponOneType && !isNormalAtk)
        {
            PlayerInfoManager.Instance.PlayerInfo.SkillPointOne--;
        }
        else if (!isNormalAtk)
        {
            PlayerInfoManager.Instance.PlayerInfo.SkillPointTwo--;
        }
        skillLevel++;
        if (skillLevel > 1)
        {
            coolDownTime -= Sub_CD;
            subMP += Add_Sub;
            attackMultiple += Add_ATKMult;
            statuDuration += Add_StatuDura;
            statuRate += Add_StatuRate;
            recHPWhenHit += Add_HPRec;
            recMPWhenHit += Add_MPRec;
        }
        CheckAndGetUI();
        PlayerSkillManager.Instance.ShowSkillInfo(this);
        PlayerSkillManager.Instance.CheckSkillButtonEnable();
    }

    public SkillInfo ConvertToInfo()
    {
        return new SkillInfo()
        {
            SkillID = skillID,
            SkillName = skillName,
            skillLevel = skillLevel,
            subMP = subMP,
            recHPWhenHit = recHPWhenHit,
            recMPWhenHit = recMPWhenHit,
            attackMultiple = attackMultiple,
            coolDownTime = coolDownTime,
            statuRate = statuRate,
            statuDuration = statuDuration,
        };
    }

    public void LoadFromInfo(SkillInfo info)
    {
        if (info == null) return;
        skillID = info.SkillID;
        skillName = info.SkillName;
        skillLevel = info.skillLevel;
        subMP = info.subMP;
        recHPWhenHit = info.recHPWhenHit;
        recMPWhenHit = info.recMPWhenHit;
        attackMultiple = info.attackMultiple;
        coolDownTime = info.coolDownTime;
        statuRate = info.statuRate;
        statuDuration = info.statuDuration;
    }

    public string GetEffectText(bool next)
    {
        bool getFormat = false;
        string temp = string.Empty;
        string format = string.Empty;
        foreach (char c in effectFormat)
        {
            if (c == '[' && !getFormat)
            {
                getFormat = true;
            }
            else
            {
                if (getFormat)
                {
                    if (c != ']')
                        format += c;
                    else
                    {
                        getFormat = false;
                        if (!next)
                            switch (format)
                            {
                                case "ATKM": temp += "<color=red>" + attackMultiple + "%</color>"; break;
                                case "ATKS": temp += "<color=red>" + subMultiple + "%</color>"; break;
                                case "HREC": temp += "<color=red>" + recHPWhenHit + "</color>"; break;
                                case "MREC": temp += "<color=red>" + recMPWhenHit + "</color>"; break;
                                case "STA": temp += "<color=red>" + StatuInfo.GetStatuName(attachStatu) + "</color>"; break;
                                case "STAR": temp += "<color=red>" + statuRate + "%</color>"; break;
                                case "STAD": temp += "<color=red>" + statuDuration + "秒</color>"; break;
                            }
                        else
                            switch (format)
                            {
                                case "ATKM": temp += "<color=red>" + (attackMultiple + Add_ATKMult) + "%</color>"; break;
                                case "ATKS": temp += "<color=red>" + subMultiple + "%</color>"; break;
                                case "HREC": temp += "<color=red>" + (recHPWhenHit + Add_ATKMult) + "</color>"; break;
                                case "MREC": temp += "<color=red>" + (recMPWhenHit + Add_MPRec) + "</color>"; break;
                                case "STA": temp += "<color=red>" + StatuInfo.GetStatuName(attachStatu) + "</color>"; break;
                                case "STAR": temp += "<color=red>" + (statuRate + Add_StatuRate) + "%</color>"; break;
                                case "STAD": temp += "<color=red>" + (statuDuration + Add_StatuDura) + "秒</color>"; break;
                            }
                        format = string.Empty;
                    }
                }
                else temp += c;
            }
        }
        return temp;
    }

    public GameObject CheckAndGetUI()
    {
        if (!UI)
        {
            UI = Instantiate(UIPrefab);
            IconButton = UI.GetComponent<Button>();
            IconButton.onClick.AddListener(OnIconClick);
            Icon = UI.transform.Find("Icon").GetComponent<Image>();
            Name = UI.transform.Find("Name").GetComponent<Text>();
            Icon.overrideSprite = skillIcon;
            Name.text = skillName;
        }
        return UI;
    }
}
