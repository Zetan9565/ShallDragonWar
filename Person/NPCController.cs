using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour {

    public NavMeshAgent navMeshAgent;
    public Animator m_Animator;
    [SerializeField]
    Transform target;

    public bool controlAble;

    // Use this for initialization
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
       // Movement();
        UpdateAnima();
    }

    void Movement()
    {
        if(target!=null && controlAble)
        {
            navMeshAgent.SetDestination(target.position);
        }
        UpdateAnima();
    }

    void UpdateAnima()
    {
        m_Animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        controlAble = true;
    }

    public void ResetTarget()
    {
        target = null;
    }
}
