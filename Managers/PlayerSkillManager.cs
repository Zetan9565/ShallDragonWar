using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance;
    public static readonly string DataName1 = "/PlayerSkillsInfo1.zetan";
    public static readonly string DataName2 = "/PlayerSkillsInfo2.zetan";
    public GameObject UI;

    public Image DesIcon;
    public Text DesName;
    public Text DesText;
    public Text currentLevel;
    public Text currentCD;
    public Text currentSub;
    public Text currentEffect;
    public Text nextLevel;
    public Text nextCD;
    public Text nextSub;
    public Text nextEffect;
    public Button levelUp;
    public Text skillPoint;
    public Toggle pageOne;
    public Toggle pageTwo;
    bool pageWeaponOne;
    [Header("按钮图标")]
    public Image skill0BtnIcon;
    public Image skill1BtnIcon;
    public Image skill2BtnIcon;
    public Image skill3BtnIcon;
    public Image skill4BtnIcon;
    [Header("冷却遮罩")]
    public SkillCDMaskAgent skill1CDMask;
    public SkillCDMaskAgent skill2CDMask;
    public SkillCDMaskAgent skill3CDMask;
    public SkillCDMaskAgent skill4CDMask;
    [Header("格挡数值")]
    public GameObject blockAmount;
    Text blockAmountValue;
    [Space]
    public GameObject weaponOneSkillGrid;
    public GameObject weaponTwoSkillGrid;

    [HideInInspector]
    public SkillInfoAgent skillNowUsing;
    //[HideInInspector]
    public List<SkillInfoAgent> skillsList;
    public List<SkillInfoAgent> weaponOneSkills;
    public List<SkillInfoAgent> weaponTwoSkills;

    int SkillIDHash = Animator.StringToHash("SkillID");
    public bool usingWeaponOne;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        CloseUI();
        pageOne.onValueChanged.Invoke(pageOne.isOn);
        blockAmountValue = blockAmount.GetComponentInChildren<Text>();
        skillsList = new List<SkillInfoAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerInfoManager.Instance.isInit) return;
        if(PlayerInfoManager.Instance.PlayerInfo.IsFighting && !PlayerInfoManager.Instance.PlayerInfo.IsMounting) UseSkill();
        if (pageWeaponOne)
        {
            skillPoint.text = PlayerInfoManager.Instance.PlayerInfo.SkillPointOne.ToString();
        }
        else
        {
            skillPoint.text = PlayerInfoManager.Instance.PlayerInfo.SkillPointTwo.ToString();
        }
        if (PlayerInfoManager.Instance.PlayerInfo.IsBlocking)
        {
            MyTools.SetActive(blockAmount, true);
            blockAmountValue.text = "<color>" + ((((PlayerInfoManager.Instance.PlayerInfo.Current_BlockAmount * 1.0f) / PlayerInfoManager.Instance.PlayerInfo.BlockAmount * 1.0f)) * 100.0f).ToString("F0") + "</color>";
        }
        else if(PlayerInfoManager.Instance.PlayerInfo.IsBlockBroken)
        {
            MyTools.SetActive(blockAmount, true);
            blockAmountValue.text = "<color=red>破甲</color>";
        }
        else
        {
            MyTools.SetActive(blockAmount, false);
        }
        if(Input.GetButtonDown("SkillWindow"))
        {
            if (!UI.activeSelf) OpenUI();
            else CloseUI();
        }
    }

    public void ShowSkillInfo(SkillInfoAgent skillInfoAgent)
    {
        DesIcon.overrideSprite = skillInfoAgent.Icon.overrideSprite;
        DesName.text = skillInfoAgent.skillName;
        DesText.text = skillInfoAgent.description;
        levelUp.onClick.RemoveAllListeners();
        MyTools.SetActive(levelUp.gameObject, true);
        currentCD.text = skillInfoAgent.coolDownTime + "秒";
        currentSub.text = skillInfoAgent.subMP + "";
        if (skillInfoAgent.skillLevel >= 10)
        {
            currentLevel.text = nextLevel.text = "登峰造极";
            levelUp.interactable = false;
            nextCD.text = skillInfoAgent.coolDownTime + "秒";
            nextSub.text = skillInfoAgent.subMP + "";
            currentEffect.text = nextEffect.text = skillInfoAgent.GetEffectText(false);
        }
        else if(skillInfoAgent.skillLevel > 0)
        {
            currentLevel.text = skillInfoAgent.skillLevel + "段";
            nextLevel.text = (skillInfoAgent.skillLevel + 1) + "段";
            nextCD.text = skillInfoAgent.coolDownTime - skillInfoAgent.Sub_CD + "秒";
            nextSub.text = skillInfoAgent.subMP + skillInfoAgent.Add_Sub + "";
            currentEffect.text = skillInfoAgent.GetEffectText(false);
            nextEffect.text = skillInfoAgent.GetEffectText(true);
            if (PlayerInfoManager.Instance.PlayerInfo.SkillPointOne < 1) levelUp.interactable = false;
            else
            {
                levelUp.onClick.AddListener(skillInfoAgent.OnLevelUp);
                levelUp.interactable = true;
            }
        }
        else
        {
            currentLevel.text = "未学习";
            nextLevel.text = (skillInfoAgent.skillLevel + 1) + "段";
            nextCD.text = skillInfoAgent.coolDownTime + "秒";
            nextSub.text = skillInfoAgent.subMP + "";
            currentEffect.text = nextEffect.text = skillInfoAgent.GetEffectText(false);
            if (PlayerInfoManager.Instance.PlayerInfo.SkillPointOne < 1) levelUp.interactable = false;
            else
            {
                levelUp.onClick.AddListener(skillInfoAgent.OnLevelUp);
                levelUp.interactable = true;
            }
        }
        if (skillInfoAgent.isNormalAtk) MyTools.SetActive(levelUp.gameObject, false);
    }

    public void UseSkill()
    {
        int SkillID = -1;
        SkillInfoAgent skillToUsed = null;
#if UNITY_ANDROID
        if (CrossPlatformInputManager.GetButton("Skill0") || (CrossPlatformInputManager.GetButton("Fire1") && CameraFollow.Camera.rotateAble))
        {
            SkillID = 0;
            if (skillsStore[0]) skillToUsed = skillsStore[0];
        }
        if (CrossPlatformInputManager.GetButton("Skill1"))
        {
            SkillID = 1;
            if (skillsStore[1]) skillToUsed = skillsStore[1];
        }
        if (CrossPlatformInputManager.GetButton("Skill2"))
        {
            SkillID = 2;
            if (skillsStore[2]) skillToUsed = skillsStore[2];
        }
        if (CrossPlatformInputManager.GetButton("Skill3"))
        {
            SkillID = 3;
            if (skillsStore[3]) skillToUsed = skillsStore[3];
        }
        if (CrossPlatformInputManager.GetButton("Skill4"))
        {
            SkillID = 4;
            if (skillsStore[4]) skillToUsed = skillsStore[4];
        }
#else 
        if (Input.GetButton("Skill0") || (Input.GetButton("Fire1") && CameraFollow.Camera.rotateAble))
        {
            SkillID = 0;
            if (skillsStore[0]) skillToUsed = skillsStore[0];
        }
        if (Input.GetButton("Skill1"))
        {
            SkillID = 1;
            if (skillsStore[1]) skillToUsed = skillsStore[1];
        }
        if (Input.GetButton("Skill2"))
        {
            SkillID = 2;
            if (skillsStore[2]) skillToUsed = skillsStore[2];
        }
        if (Input.GetButton("Skill3"))
        {
            SkillID = 3;
            if (skillsStore[3]) skillToUsed = skillsStore[3];
        }
        if (Input.GetButton("Skill4"))
        {
            SkillID = 4;
            if (skillsStore[4]) skillToUsed = skillsStore[4];
        }
#endif
        if (skillToUsed)
        {
            if (CheckSkillCD(skillToUsed))
                PlayerLocomotionManager.Instance.playerAnima.SetInteger(SkillIDHash, SkillID);
            else PlayerLocomotionManager.Instance.playerAnima.SetInteger(SkillIDHash, -1);
        }
        else PlayerLocomotionManager.Instance.playerAnima.SetInteger(SkillIDHash, -1);
        if (CrossPlatformInputManager.GetButtonDown("Dodge") && PlayerInfoManager.Instance.PlayerInfo.Current_MP > 50)
            PlayerLocomotionManager.Instance.SetDodge();
    }

    public void SaveToFile(string path, string key= "", bool encrypt = false)
    {
        List<SkillInfo> list = new List<SkillInfo>();
        foreach (SkillInfoAgent sa in weaponOneSkills)
            list.Add(sa.ConvertToInfo());
        if (key.Length == 32 && encrypt)
        {
            File.WriteAllText(path + DataName1, Encryption.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(list), key), System.Text.Encoding.UTF8);
        }
        else
        {
            File.WriteAllText(path + DataName1, Newtonsoft.Json.JsonConvert.SerializeObject(list), System.Text.Encoding.UTF8);
        }
        list.Clear();
        foreach (SkillInfoAgent sa in weaponTwoSkills)
            list.Add(sa.ConvertToInfo());
        if (key.Length == 32 && encrypt)
        {
            File.WriteAllText(path + DataName2, Encryption.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(list), key), System.Text.Encoding.UTF8);
        }
        else
        {
            File.WriteAllText(path + DataName2, Newtonsoft.Json.JsonConvert.SerializeObject(list), System.Text.Encoding.UTF8);
        }
    }

    public void LoadFromFile(string path, string key = "", bool dencrypt = false)
    {
        List<SkillInfo> infos = key.Length == 32 && dencrypt ?
            Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkillInfo>>(Encryption.Dencrypt(File.ReadAllText(path + DataName1, System.Text.Encoding.UTF8), key)) :
        infos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkillInfo>>(File.ReadAllText(path + DataName1, System.Text.Encoding.UTF8));
        foreach (SkillInfoAgent sa in weaponOneSkills)
            sa.LoadFromInfo(infos.Find(s => s.SkillID == sa.skillID));
        infos.Clear();
        infos = key.Length == 32 && dencrypt ?
            Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkillInfo>>(Encryption.Dencrypt(File.ReadAllText(path + DataName2, System.Text.Encoding.UTF8), key)) :
        infos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SkillInfo>>(File.ReadAllText(path + DataName2, System.Text.Encoding.UTF8));
        foreach (SkillInfoAgent sa in weaponTwoSkills)
            sa.LoadFromInfo(infos.Find(s => s.SkillID == sa.skillID));
        foreach (SkillInfoAgent sa in weaponOneSkills)
            sa.UI.transform.SetParent(weaponOneSkillGrid.transform);
        foreach (SkillInfoAgent sa in weaponTwoSkills)
            sa.UI.transform.SetParent(weaponTwoSkillGrid.transform);
    }

    [SerializeField]
    SkillInfoAgent[] skillsStore = new SkillInfoAgent[10];

    public void StoreSkillToUse()
    {
        if (usingWeaponOne)
        {
            skillsStore[0] = weaponOneSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "00");
            skillsStore[1] = weaponOneSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "01");
            skillsStore[2] = weaponOneSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "02");
            skillsStore[3] = weaponOneSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "03");
            skillsStore[4] = weaponOneSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "04");
            skillsStore[5] = weaponOneSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "05");
        }
        else
        {
            skillsStore[0] = weaponTwoSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "00");
            skillsStore[1] = weaponTwoSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "01");
            skillsStore[2] = weaponTwoSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "02");
            skillsStore[3] = weaponTwoSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "03");
            skillsStore[4] = weaponTwoSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "04");
            skillsStore[5] = weaponTwoSkills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "05");
        }
        skill0BtnIcon.overrideSprite = skillsStore[0].skillIcon;
        skill1BtnIcon.overrideSprite = skillsStore[1].skillIcon;
        skill2BtnIcon.overrideSprite = skillsStore[2].skillIcon;
        skill3BtnIcon.overrideSprite = skillsStore[3].skillIcon;
        skill4BtnIcon.overrideSprite = skillsStore[4].skillIcon;
        skill1CDMask.skillInfoAgent = skillsStore[1];
        skill2CDMask.skillInfoAgent = skillsStore[2];
        skill3CDMask.skillInfoAgent = skillsStore[3];
        skill4CDMask.skillInfoAgent = skillsStore[4];
        CheckSkillButtonEnable();
    }

    bool CheckSkillCD(SkillInfoAgent skillInfo)
    {
        if (PlayerInfoManager.Instance.PlayerInfo.Current_MP - skillInfo.subMP < 0 || skillInfo.skillLevel <= 0 || (!skillInfo.isCD && !skillInfo.usableWhenInCD))
            return false;
        return true;
    }

    public void CheckSkillButtonEnable()
    {
        if (skillsStore[0] && skillsStore[0].skillLevel > 0 && Application.isMobilePlatform)
            MyTools.SetActive(skill0BtnIcon.transform.parent.gameObject, true);
        else MyTools.SetActive(skill0BtnIcon.transform.parent.gameObject, false);

        if (skillsStore[1] && skillsStore[1].skillLevel > 0)
            MyTools.SetActive(skill1BtnIcon.transform.parent.gameObject, true);
        else MyTools.SetActive(skill1BtnIcon.transform.parent.gameObject, false);

        if (skillsStore[2] && skillsStore[2].skillLevel > 0)
            MyTools.SetActive(skill2BtnIcon.transform.parent.gameObject, true);
        else MyTools.SetActive(skill2BtnIcon.transform.parent.gameObject, false);

        if (skillsStore[3] && skillsStore[3].skillLevel > 0)
            MyTools.SetActive(skill3BtnIcon.transform.parent.gameObject, true);
        else MyTools.SetActive(skill3BtnIcon.transform.parent.gameObject, false);

        if (skillsStore[4] && skillsStore[4].skillLevel > 0)
            MyTools.SetActive(skill4BtnIcon.transform.parent.gameObject, true);
        else MyTools.SetActive(skill4BtnIcon.transform.parent.gameObject, false);
    }

    public void GetSkills()
    {
        weaponOneSkills = skillsList.FindAll(s => s.skillID.Contains(PlayerWeaponManager.Instance.weaponOneType.ToString()));
        if (weaponOneSkills.Count != 0)
        {
            weaponOneSkills.Sort((x, y) =>
            {
                if (int.Parse(x.skillID.Substring(x.skillID.Length - 2)) > int.Parse(y.skillID.Substring(y.skillID.Length - 2)))
                {
                    return 1;
                }
                else if (int.Parse(x.skillID.Substring(x.skillID.Length - 2)) < int.Parse(y.skillID.Substring(y.skillID.Length - 2)))
                    return -1;
                else return 0;
            });
            foreach (SkillInfoAgent sa in weaponOneSkills)
            {
                sa.isCD = true;
                sa.CheckAndGetUI().transform.SetParent(weaponOneSkillGrid.transform, false);
            }
        }
        weaponTwoSkills = skillsList.FindAll(s => s.skillID.Contains(PlayerWeaponManager.Instance.weaponTwoType.ToString()));
        if (weaponTwoSkills.Count != 0)
        {
            weaponTwoSkills.Sort((x, y) =>
            {
                if (int.Parse(x.skillID.Substring(x.skillID.Length - 2)) > int.Parse(y.skillID.Substring(y.skillID.Length - 2)))
                    return 1;
                else if (int.Parse(x.skillID.Substring(x.skillID.Length - 2)) < int.Parse(y.skillID.Substring(y.skillID.Length - 2)))
                    return -1;
                else return 0;
            });
            foreach (SkillInfoAgent sa in weaponTwoSkills)
            {
                sa.isCD = true;
                sa.CheckAndGetUI().transform.SetParent(weaponTwoSkillGrid.transform);
            }
        }
    }

    public void OnPageOneChange()
    {
        if (pageOne.isOn)
        {
            if (weaponOneSkills.Count > 0) weaponOneSkills[0].OnIconClick();
            pageWeaponOne = true;
        }
    }

    public void OnPageTwoChange()
    {
        if (pageTwo.isOn)
        {
            if (weaponTwoSkills.Count > 0) weaponTwoSkills[0].OnIconClick();
            pageWeaponOne = false;
        }
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
    }

    public void OpenUI()
    {
        GetSkills();
        if (pageOne.isOn)
        {
            if (weaponOneSkills.Count > 0) weaponOneSkills[0].OnIconClick();
            pageWeaponOne = true;
        }
        if (pageTwo.isOn)
        {
            if (weaponTwoSkills.Count > 0) weaponTwoSkills[0].OnIconClick();
            pageWeaponOne = false;
        }
        MyTools.SetActive(UI, true);
    }
}