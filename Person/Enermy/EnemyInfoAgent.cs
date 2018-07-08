using MyEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(EnemySkillsAgent))]
public class EnemyInfoAgent : MonoBehaviour {

    public string enermyID;
    public string Name;
    public Sprite headIcon;
    [Range(1, 40)]
    public int Level = 1;
    public bool isBoss;
    public float HP;
    [SerializeField]
    private float current_HP;
    public float Current_HP
    {
        get { return current_HP; }
        set
        {
            if (value > HP) current_HP = HP;
            else if (value < 0) { current_HP = 0; IsAlive = false; }
            else current_HP = value;
        }
    }
    public int BlockAmount;
    private int current_BlockAmount;
    public int Current_BlockAmount
    {
        get { return current_BlockAmount; }
        set
        {
            if (value < 0) current_BlockAmount = 0;
        }
    }

    [Header("主要属性")]
    public int ATK;
    public int DEF;
    public float Hit;
    public float Dodge;
    public float Crit;
    [Header("抗性列表")]
    public float Res_Rigidity;
    //public float Res_Repulsed;
    public float Res_Stuned;
    //public float Res_Floated;
    //public float Res_Blowed;
    public float Res_Falled;
    public List<StatuInfo> status;
    [Header("实时状态")]
    public bool IsAlive;
    //public bool IsPatroling;
    public bool IsFighting;
    public bool IsDown;
    public bool IsGiddy;
    public bool IsRigid;
    //public bool IsFloat;
    public bool IsBlocking;
    public bool IsBlockBroken;
    public bool SuperArmor;
    [Space]
    public int giveEXP;
    [Header("掉落品列表")]
    public GameObject dropItemPrefab;
    [Tooltip("格式：物品ID,最大掉落数量,掉落率")]
    public string[] dropItemsInput;
    [Space]
    EnemyAudioAgent enemyAudioAgent;
    EnemyLocomotionAgent enemyLocmAgent;
    public float deathAnimaTime;
    public GameObject hitEffectPrefab;
    public float hitEffectHeight;
    public Transform HPBarPoint;
    [HideInInspector]
    public HPBarAgent HPBar;
    public UnityEvent onDeath;
    public UnityEvent onRelive;
    [HideInInspector]
    public float timeAfterDeath = 0;
    public EnemySpawner spawner;

    private void Awake()
    {
        enemyLocmAgent = GetComponent<EnemyLocomotionAgent>();
        enemyAudioAgent = GetComponent<EnemyAudioAgent>();
        Relive();
    }

    // Use this for initialization
    /*void Start () {
        
	}*/

    // Update is called once per frame
    void Update () {
        if (status.Count > 0)
        {
            StatusCD();
        }
    }

    IEnumerator OnDeath()
    {
        PlayerInfoManager.Instance.GetEXP(giveEXP);
        GameObject dropItems = Instantiate(dropItemPrefab) as GameObject;
        dropItems.transform.position = transform.position;
        DropItemListAgent dropItemList = dropItems.GetComponent<DropItemListAgent>();
        dropItemList.GetDropItems(dropItemsInput);
        if (dropItemList.dropItemList.Count <= 0) Destroy(dropItemList.gameObject);
        enemyAudioAgent.DeathVoice();
        onDeath.Invoke();
        yield return new WaitForSeconds(deathAnimaTime);
        MyTools.SetActive(gameObject, false);
    }

    public void AutoBorn()
    {
        MyTools.SetActive(gameObject, true);       
    }

    public void Relive()
    {
        if (status != null) status.Clear();
        else status = new List<StatuInfo>();
        Current_HP = HP;
        IsAlive = true;
        IsBlocking = false;
        IsBlockBroken = false;
        IsDown = false;
        IsFighting = false;
        if(IsRigid)
        {
            enemyLocmAgent.enemyAnima.speed = 1;
            enemyLocmAgent.enemyController.controlAble = true;
        }
        IsRigid = false;
        //IsFloat = false;
        IsGiddy = false;
        if (HUDTextManager.Instance && HPBarPoint) HUDTextManager.Instance.NewHPBar(this);
        timeAfterDeath = 0;
        GetComponent<BehaviorDesigner.Runtime.BehaviorTree>().SetVariableValue("Player", GameObject.FindWithTag("Player"));
        onRelive.Invoke();
    }

    public void BeAttack(PlayerInfo playerInfo, SkillInfoAgent skillInfo, bool hurtForwad)
    {
        DamageType damageType;
        StatuInfo statuInfo;
        int damageValue = MyTools.GetDamaged(false , playerInfo, this, skillInfo, out damageType, out statuInfo, hurtForwad);
        //Debug.Log("Enemy is hurt:" + damageType + "||" + damageValue);
        Current_HP -= damageValue;
        if (statuInfo.Statu != Statu.None)
        {
            if (SetStatu(statuInfo))
            {
                switch (statuInfo.Statu)
                {
                    case Statu.Falled: HUDTextManager.Instance.NewStatu(transform, Statu.Falled); break;
                    case Statu.Giddy: HUDTextManager.Instance.NewStatu(transform, Statu.Giddy); break;
                    case Statu.Rigidity: HUDTextManager.Instance.NewStatu(transform, Statu.Rigidity); break;
                }
                HUDTextManager.Instance.NewStatu(transform, statuInfo.Statu);
            }
        }
        switch (damageType)
        {
            case DamageType.Miss:
                HUDTextManager.Instance.NewMiss(transform);break;
            case DamageType.HurtBackward:
                HUDTextManager.Instance.NewBackDamageValue(transform, damageValue);
                if (statuInfo.Statu == Statu.None) enemyLocmAgent.SetEnemyHurtAnima();
                if (hitEffectPrefab) ObjectPoolManager.Instance.Get(hitEffectPrefab, transform.position + Vector3.up * hitEffectHeight, transform.rotation);
                enemyAudioAgent.HitSound();
                enemyAudioAgent.DamageVoice();
                break;
            case DamageType.BlockBroken:
                HUDTextManager.Instance.NewBlockBroken(transform);
                SetStatu(new StatuInfo(Statu.BlockBroken, 5));
                enemyAudioAgent.HitSound();
                enemyAudioAgent.DamageVoice();
                break;
            case DamageType.Block:
                HUDTextManager.Instance.NewBlock(transform); break;
            case DamageType.Crit:
                HUDTextManager.Instance.NewCritDamageValue(transform, damageValue);
                if (statuInfo.Statu == Statu.None) enemyLocmAgent.SetEnemyHurtAnima();
                enemyAudioAgent.HitSound();
                enemyAudioAgent.DamageVoice();
                break;
            default:
                HUDTextManager.Instance.NewDamageValue(transform, damageValue);
                if (statuInfo.Statu == Statu.None) enemyLocmAgent.SetEnemyHurtAnima();
                if(hitEffectPrefab) ObjectPoolManager.Instance.Get(hitEffectPrefab, transform.position + Vector3.up * hitEffectHeight, transform.rotation);
                enemyAudioAgent.HitSound();
                enemyAudioAgent.DamageVoice();
                break;
        }
        IsFighting = true;
        if(isBoss)
        {
            HUDTextManager.Instance.SetBossHPBar(this);
        }
        if (!IsAlive)
        {
            StartCoroutine(OnDeath());
        }
    }

    bool SetStatu(StatuInfo sinfo)
    {
        if (sinfo == null || SuperArmor) return false;
        if (status == null) status = new List<StatuInfo>();
        if (CheckStatuRes(sinfo.Statu)) {/* Debug.Log("抵抗了状态" + sinfo.statu_name);*/ return false; }//如果抵抗了该状态
        //Debug.Log("设置了状态:" + sinfo.Statu.ToString() + ",持续时间为:" + sinfo.Duration);
        if (sinfo.Statu == Statu.Giddy && status.Exists(s => s.Statu == Statu.Falled))
        {
            return false;
        }
        if (sinfo.Statu == Statu.Falled && status.Exists(s => s.Statu == Statu.Giddy))
        {
            CleanStatu(Statu.Giddy);
        }
        if (status.Exists(s => s.Statu == sinfo.Statu))
        {
            return false;
        }
        status.Add(sinfo);
        switch (sinfo.Statu)
        {
            case Statu.BlockBroken: IsBlockBroken = true; break;
            case Statu.Giddy: IsGiddy = true; break;
            case Statu.Falled: IsDown = true; break;
            case Statu.Rigidity:
                IsRigid = true;
                enemyLocmAgent.enemyAnima.speed = 0;
                enemyLocmAgent.enemyController.controlAble = false;
                break;
        }
        status.Sort((x, y) => { if (x.Statu > y.Statu) return -1; else if (x.Statu == y.Statu) return 0; else return 1; });
        return true;
    }

    bool CheckStatuRes(Statu statu)
    {
        switch (statu)
        {
            case Statu.Rigidity: return MyTools.Probability(Res_Rigidity);
            //case Statu.Repulsed: return MyTools.Probability(Res_Repulsed);
            case Statu.Giddy: return MyTools.Probability(Res_Stuned);
            //case Statu.Floated: return MyTools.Probability(Res_Floated);
            //case Statu.Blowed: return MyTools.Probability(Res_Blowed);
            case Statu.Falled: return MyTools.Probability(Res_Falled);
            default: return false;
        }
    }

    void CleanStatu(Statu statu)
    {
        if (status == null) return;
        //Debug.Log("清除了状态:" + statu.ToString());
        if (statu == Statu.BlockBroken) IsBlockBroken = false;
        status.RemoveAll(s => s.Statu == statu);
        switch (statu)
        {
            case Statu.BlockBroken: IsBlockBroken = false; break;
            case Statu.Giddy: IsGiddy = false; break;
            case Statu.Falled: IsDown = false; break;
            case Statu.Rigidity: IsRigid = false; break;
        }
    }

    void StatusCD()
    {
        if (status == null) return;
        for (int i = 0; i < status.Count; i++)
        {
            status[i].SubTime(Time.deltaTime);
            if (status[i].IsEnd())
            {
                if (status[i].Statu == Statu.Rigidity)
                {
                    enemyLocmAgent.enemyAnima.speed = 1;
                    enemyLocmAgent.enemyController.controlAble = true;
                }
                CleanStatu(status[i].Statu);
            }
        }
    }
}
