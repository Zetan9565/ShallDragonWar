using System.Collections;
using UnityEngine;
using MalbersAnimations.HAP;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class PlayerLocomotionManager : MonoBehaviour {
    public static PlayerLocomotionManager Instance;

    public PlayerController playerController;
    [HideInInspector]
    public Rigidbody playerRigidbd;
    [HideInInspector]
    public Animator playerAnima;
    public List<GameObject> GatherTools;
    public Transform normalRoot;
    public GameObject normalBips;
    public GameObject normalBody;
    public Transform mountRoot;
    public GameObject mountBips;
    public GameObject mountBody;
    public AnimatorStateInfo AnimatorStateInfo;
    //public AnimatorTransitionInfo AnimatorTransitionInfo;
    public bool isInit;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    // Use this for initialization
    public void Init() {
        PlayerObjectAgent playerObjectAgent = PlayerInfoManager.Instance.Player.GetComponent<PlayerObjectAgent>();
        playerController = playerObjectAgent.playerController;
        playerRigidbd = playerController.m_Rigidbody;
        playerAnima = playerController.m_Animator;
        GatherTools = playerObjectAgent.GatherTools;
        normalRoot = playerObjectAgent.normalRoot;
        normalBips = playerObjectAgent.normalBips;
        normalBody = playerObjectAgent.normalBody;
        mountRoot = playerObjectAgent.mountRoot;
        mountBips = playerObjectAgent.mountBips;
        mountBody = playerObjectAgent.mountBody;
        isInit = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            UpdateAnima();
            if (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown("Fight"))
                SwapFightStatu();
            SetBlock(UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButton("Block"));
            //if (Input.GetKeyDown(KeyCode.V)) SetPlayerRepulsed();
            if (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown("Sit"))
                SetSitAnima();
            PlayerInfoManager.Instance.PlayerInfo.IsSwimming = playerController.IsHeadWatered;
            playerController.Fighting = PlayerInfoManager.Instance.PlayerInfo.IsFighting;
            //playerController.GetComponentInChildren<Rider3rdPerson>().enabled = PlayerInfoManager.Self.PlayerInfo.IsFighting;
        }
    }

    private void FixedUpdate()
    {
        if (isInit)
        {//playerAnima.SetBool(Animator.StringToHash("Miss"), false);
            if (isRepulsed)
            {
                repulsedTime += Time.deltaTime;
                playerController.moveAble = false;
                playerController.rotateAble = false;
                playerRigidbd.velocity = Vector3.ProjectOnPlane(-playerController.transform.forward, playerController.GroundNormal) * 20;
                if (repulsedTime > 0.1f)
                {
                    isRepulsed = false;
                    repulsedTime = 0;
                    playerController.moveAble = true;
                    playerController.rotateAble = true;
                    //playerController.m_Rigidbody.Sleep();
                    playerRigidbd.velocity = Vector3.zero;
                }
            }
            if (autoForward)
            {
                playerRigidbd.velocity = Vector3.ProjectOnPlane(playerController.transform.forward, playerController.GroundNormal) * autoFwdSpeed;
            }
        }
    }

    void UpdateAnima()
    {
        playerAnima.SetBool(Animator.StringToHash("IsAlive"), PlayerInfoManager.Instance.PlayerInfo.IsAlive);
        if (!PlayerInfoManager.Instance.PlayerInfo.IsAlive) return;
        AnimatorStateInfo = playerAnima.GetCurrentAnimatorStateInfo(0);
        //AnimatorTransitionInfo = playerAnima.GetAnimatorTransitionInfo(0);
        PlayerInfoManager.Instance.PlayerInfo.IsBlocking = AnimatorStateInfo.IsTag("Blocking");
        playerAnima.SetBool(Animator.StringToHash("Fighting"), PlayerInfoManager.Instance.PlayerInfo.IsFighting);
        playerAnima.SetBool(Animator.StringToHash("HaveWeapon"), PlayerWeaponManager.Instance.currentWeaponTag != string.Empty);
        playerAnima.SetBool(Animator.StringToHash("IsDown"), PlayerInfoManager.Instance.PlayerInfo.IsDown);
        playerAnima.SetBool(Animator.StringToHash("IsGiddy"), PlayerInfoManager.Instance.PlayerInfo.IsGiddy);
        switch (PlayerWeaponManager.Instance.currentWeaponTag)
        {
            case "Halberd": playerAnima.SetInteger(Animator.StringToHash("WeaponType"), 1); break;
            case "Spear": playerAnima.SetInteger(Animator.StringToHash("WeaponType"), 2); break;
            case "Sword": playerAnima.SetInteger(Animator.StringToHash("WeaponType"), 3); break;
            case "Blade": playerAnima.SetInteger(Animator.StringToHash("WeaponType"), 4); break;
            default: playerAnima.SetInteger(Animator.StringToHash("WeaponType"), -1); break;
        }
    }

    #region 特殊动作相关
    public void SetBlock(bool blocking)
    {
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.IsBlockBroken)
        {
            //NotificationManager.Self.NewNotification("破防状态无法再次格挡");
            playerAnima.SetBool(Animator.StringToHash("Blocking"), false);
            return;
        }
        if (!playerController.IsHeadWatered && playerController.IsGrounded && !playerController.Crouching)
        {
            if (!PlayerInfoManager.Instance.PlayerInfo.IsBlocking) PlayerInfoManager.Instance.PlayerInfo.Current_BlockAmount = PlayerInfoManager.Instance.PlayerInfo.BlockAmount;
            playerAnima.SetBool(Animator.StringToHash("Blocking"), blocking);
        }
        else playerAnima.SetBool(Animator.StringToHash("Blocking"), false);
    }
    public void SetSitAnima()
    {
        if (!isInit) return;
        if ((AnimatorStateInfo.IsTag("Idle") || AnimatorStateInfo.IsTag("Move")) && !PlayerInfoManager.Instance.PlayerInfo.IsFighting && !PlayerInfoManager.Instance.PlayerInfo.IsSitting)
            playerAnima.SetInteger(Animator.StringToHash("ActionType"), 3);
        else if(PlayerInfoManager.Instance.PlayerInfo.IsSitting)
            playerAnima.SetInteger(Animator.StringToHash("ActionType"), 0);
        else
        {
            NotificationManager.Instance.NewNotification("该状态下无法使用");
        }
    }
    [HideInInspector]
    public bool autoForward;
    [HideInInspector]
    public float autoFwdSpeed;
    public void SetDodge()
    {
        if (!isInit) return;
        if ((AnimatorStateInfo.IsTag("Idle") || AnimatorStateInfo.IsTag("Move")) && PlayerInfoManager.Instance.PlayerInfo.IsFighting)
            playerAnima.SetTrigger(Animator.StringToHash("Dodge"));
    }
    #endregion

    #region 采集状态相关
    public void SetPlayerGatherAnima()
    {
        if (!isInit) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsAlive || PlayerInfoManager.Instance.PlayerInfo.IsMounting) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsGathering)
        {
            switch (GatherManager.Instance.gatherInfoAgent.agentType)
            {
                case MyEnums.GatherAgentType.Grass:
                case MyEnums.GatherAgentType.Stone: playerAnima.SetInteger(Animator.StringToHash("GatherType"), 0); break;
                case MyEnums.GatherAgentType.Ore: playerAnima.SetInteger(Animator.StringToHash("GatherType"), 1); break;
                case MyEnums.GatherAgentType.Tree: playerAnima.SetInteger(Animator.StringToHash("GatherType"),2); break;
                case MyEnums.GatherAgentType.HighGrass: playerAnima.SetInteger(Animator.StringToHash("GatherType"), 3); break;
            }
            playerAnima.SetBool(Animator.StringToHash("IsGathering"), true);
        }
    }

    public void StartGather()
    {
        if (!isInit) return;
        //Debug.Log("start");
        playerController.GetComponentInChildren<Rider3rdPerson>().enabled = false;
        PlayerInfoManager.Instance.PlayerInfo.IsGathering = true;
        TimeProgressBarManager.Instance.onEnd.AddListener(FinishGather);
        TimeProgressBarManager.Instance.NewTimeProgress("采集中……", GatherManager.Instance.gatherInfoAgent.gatherTime);
    }

    public void FinishGather()
    {
        if (!isInit) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsGathering) return;
        PlayerInfoManager.Instance.PlayerInfo.IsGathering = false;
        TimeProgressBarManager.Instance.onEnd.RemoveListener(FinishGather);
        GatherManager.Instance.gatherInfoAgent.OnGatherFinished();
        playerController.GetComponentInChildren<Rider3rdPerson>().enabled = true;
        playerAnima.SetBool(Animator.StringToHash("IsGathering"), false);
        //Debug.Log("Finished");
    }

    public void CancelGather()
    {
        if (!isInit) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsGathering) return;
        PlayerInfoManager.Instance.PlayerInfo.IsGathering = false;
        TimeProgressBarManager.Instance.onEnd.RemoveAllListeners();
        TimeProgressBarManager.Instance.CancelProgress();
        if (GatherManager.Instance.gatherInfoAgent) GatherManager.Instance.CanGather(GatherManager.Instance.gatherInfoAgent);
        playerController.GetComponentInChildren<Rider3rdPerson>().enabled = true;
        playerAnima.SetBool(Animator.StringToHash("IsGathering"), false);
    }
    #endregion


    #region 受伤相关
    public void SetPlayerHurtAnima()
    {
        if (!isInit) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsAlive || PlayerInfoManager.Instance.PlayerInfo.IsMounting || playerController.Crouching) return;
        if (!AnimatorStateInfo.IsTag("FallDown") && !AnimatorStateInfo.IsTag("Falling") && !AnimatorStateInfo.IsTag("Jump") && !AnimatorStateInfo.IsTag("Damage") && !playerAnima.IsInTransition(0))
            playerAnima.SetTrigger(Animator.StringToHash("Damage"));
    }
    public void SetPlayerMissAnima()
    {
        if (!isInit) return;
        if (AnimatorStateInfo.IsTag("Idle") && !playerAnima.IsInTransition(0) && !PlayerInfoManager.Instance.PlayerInfo.IsBlocking && !playerController.Crouching)
            playerAnima.SetTrigger(Animator.StringToHash("Miss"));
    }
    bool isRepulsed;
    float repulsedTime;
    public void SetPlayerRepulsed()
    {
        if (!isInit) return;
        //playerController.m_Rigidbody.AddRelativeForce(Vector3.ProjectOnPlane(-playerController.transform.forward, playerController.GroundNormal) * 200);
        isRepulsed = true;
    }
    #endregion

    #region 战斗状态相关
    public void SwapFightStatu()
    {
        if (!isInit) return;
        if (!PlayerInfoManager.Instance.PlayerInfo.IsAlive || PlayerInfoManager.Instance.PlayerInfo.IsMounting 
            || PlayerInfoManager.Instance.PlayerInfo.IsSwimming || (!AnimatorStateInfo.IsTag("Move") && !AnimatorStateInfo.IsTag("Idle"))) return;
        if (PlayerInfoManager.Instance.PlayerInfo.IsFighting)
        {
            if (playerAnima.GetBool("Fighting"))
            {
                //Debug.Log("End fight");
                PlayerInfoManager.Instance.PlayerInfo.IsFighting = false;
                playerAnima.SetBool(Animator.StringToHash("SheathWp"), true);
            }
        }
        else
        {
            if (PlayerInfoManager.Instance.PlayerInfo.equipments.weapon == null)
            {
                NotificationManager.Instance.NewNotification("没有装备武器");
                return;
            }
            if (!PlayerInfoManager.Instance.PlayerInfo.IsFighting && !playerAnima.GetBool("Fighting"))
            {
                //Debug.Log("Start fight");
                PlayerInfoManager.Instance.PlayerInfo.IsFighting = true;
                playerAnima.SetBool(Animator.StringToHash("TakeOutWp"), true);
            }
        }
    }
    #endregion

    #region 乘骑相关
    public void OnMount()
    {
        if (!isInit) return;
        CancelGather();
        if(GatherManager.Instance) GatherManager.Instance.CantGather();
        if(PickUpManager.Instance) PickUpManager.Instance.CantPick();
        playerAnima.SetFloat("Forward", 0);
        playerController.enabled = false;
        playerController.GetComponentInParent<PlayerUserController>().enabled = false;
        playerRigidbd.useGravity = false;
        playerRigidbd.isKinematic = true;
        playerController.Body.enabled = false;
        mountRoot.parent = null;
        normalRoot.SetParent(mountRoot);
        normalRoot.transform.SetPositionAndRotation(mountRoot.position, mountRoot.rotation);
        MyTools.SetActive(normalBips, false);
        MyTools.SetActive(normalBody, false);
        MyTools.SetActive(mountBips, true);
        MyTools.SetActive(mountBody, true);
        PlayerInfoManager.Instance.PlayerInfo.IsMounting = true;
    }
    public void OnDismount()
    {
        if (!isInit) return;
        playerRigidbd.useGravity = true;
        playerRigidbd.isKinematic = false;
        playerController.Body.enabled = true;
        playerController.enabled = true;
        playerController.GetComponentInParent<PlayerUserController>().enabled = true;
        normalRoot.parent = null;
        mountRoot.SetParent(normalRoot);
        normalRoot.transform.SetPositionAndRotation(mountRoot.position, mountRoot.rotation);
        MyTools.SetActive(normalBips, true);
        MyTools.SetActive(normalBody, true);
        MyTools.SetActive(mountBips, false);
        MyTools.SetActive(mountBody, false);
        PlayerInfoManager.Instance.PlayerInfo.IsMounting = false;
    }
    #endregion
}
