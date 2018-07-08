using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyInfoAgent))]
[RequireComponent(typeof(NPCController))]
public class EnemyLocomotionAgent : MonoBehaviour {

    public EnemyInfoAgent enemyInfoAgent;
    public BehaviorDesigner.Runtime.BehaviorTree behaviorTree;
    public NPCController enemyController;
    public Animator enemyAnima;
    public AnimatorStateInfo AnimatorStateInfo;
    [Space]
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;
    [Space]
    public Collider frontAtkTrigger;
    public Collider backAtkTrigger;
    public Collider bodyCollider;
    [Space]
    public GameObject bodyMesh;
    public GameObject bipsRoot;

    // Use this for initialization
    void Start () {
        enemyInfoAgent = GetComponent<EnemyInfoAgent>();
        enemyController = GetComponent<NPCController>();
        enemyAnima = enemyController.m_Animator;
        if (frontAtkTrigger)
        {
            frontAtkTrigger.enabled = false;
            frontAtkTrigger.gameObject.tag = "EnemyWeapon";
            if (!frontAtkTrigger.isTrigger) frontAtkTrigger.isTrigger = true;
        }
        if (backAtkTrigger)
        {
            backAtkTrigger.enabled = false;
            backAtkTrigger.gameObject.tag = "EnemyWeapon";
            if (!backAtkTrigger.isTrigger) backAtkTrigger.isTrigger = true;
        }
        bodyCollider = enemyController.GetComponent<Collider>();
        behaviorTree = GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnima();
        bodyCollider.enabled = enemyInfoAgent.IsAlive;
        if (behaviorTree) behaviorTree.enabled = enemyInfoAgent.IsAlive;
        if (enemyController.navMeshAgent.isOnNavMesh) enemyController.navMeshAgent.isStopped = !enemyInfoAgent.IsAlive || (enemyInfoAgent.IsAlive && enemyInfoAgent.IsRigid);
        if (PlayerLocomotionManager.Instance && PlayerLocomotionManager.Instance.isInit)
        {
            if (Vector3.Distance(transform.position, PlayerLocomotionManager.Instance.playerController.transform.position) > 20)
            {
                MyTools.SetActive(bodyMesh, false);
                MyTools.SetActive(bipsRoot, false);
            }
            else
            {
                MyTools.SetActive(bodyMesh, true);
                MyTools.SetActive(bipsRoot, true);
            }
            if (Vector3.Distance(enemyInfoAgent.transform.position, PlayerLocomotionManager.Instance.playerController.transform.position) > 10)
            {
                if (enemyInfoAgent.HPBar) enemyInfoAgent.HPBar.HideBar();
            }
            else
            {
                if (enemyInfoAgent.HPBar) enemyInfoAgent.HPBar.ShowBar();
            }
        }
        if (behaviorTree) behaviorTree.SetVariableValue("MoveSpeed", !enemyInfoAgent.IsFighting ? walkSpeed : runSpeed);
    }

    /*public void FixedUpdate()
    {
    
    }*/

    public void UpdateAnima()
    {
        AnimatorStateInfo = enemyAnima.GetCurrentAnimatorStateInfo(0);
        enemyAnima.SetBool(Animator.StringToHash("Fighting"), enemyInfoAgent.IsFighting);
        enemyAnima.SetBool(Animator.StringToHash("IsDown"), enemyInfoAgent.IsDown);
        enemyAnima.SetBool(Animator.StringToHash("IsGiddy"), enemyInfoAgent.IsGiddy);
        enemyAnima.SetBool(Animator.StringToHash("IsAlive"), enemyInfoAgent.IsAlive);
    }

    public void SetEnemyHurtAnima()
    {
        enemyAnima.SetTrigger("Damaged");
    }

    [HideInInspector]
    public bool autoForward;
    [HideInInspector]
    public float forwardSpeed;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerWeapon")
        {
            enemyInfoAgent.BeAttack(PlayerInfoManager.Instance.PlayerInfo, PlayerSkillManager.Instance.skillNowUsing,
                Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(other.transform.root.position - transform.position, Vector3.up).normalized) < 90);
        }
    }

    public void SetFrontWeaponTrigger(bool state)
    {
        if (frontAtkTrigger) frontAtkTrigger.enabled = state;
    }

    public void SetBackWeaponTrigger(bool state)
    {
        if (backAtkTrigger) backAtkTrigger.enabled = state;
    }
}
