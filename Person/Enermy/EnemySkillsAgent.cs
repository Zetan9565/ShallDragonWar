using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillsAgent : MonoBehaviour
{

    public SkillInfoAgent currentSkill;
    public EnemyLocomotionAgent enemyLocomotionAgent;

    public List<SkillInfoAgent> skills;

    // Use this for initialization
    void Start()
    {
        enemyLocomotionAgent = GetComponent<EnemyLocomotionAgent>();
        if (skills == null) skills = new List<SkillInfoAgent>();
        foreach (SkillInfoAgent sa in skills)
        {
            sa.isCD = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            Attack01();
    }

    public void Attack01()
    {
        if (!enemyLocomotionAgent.enemyInfoAgent.IsFighting || !enemyLocomotionAgent.enemyInfoAgent.IsAlive) return;
        SkillInfoAgent skillToUse = skills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "01");
        if (!skillToUse) return;
        //Debug.Log("a");
        if (CheckSkillCD(skillToUse)) enemyLocomotionAgent.enemyAnima.SetTrigger(Animator.StringToHash("Attack1"));
    }

    public void Attack02()
    {
        if (!enemyLocomotionAgent.enemyInfoAgent.IsFighting || !enemyLocomotionAgent.enemyInfoAgent.IsAlive) return;
        SkillInfoAgent skillToUse = skills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "02");
        if (!skillToUse) return;
        if (CheckSkillCD(skillToUse)) enemyLocomotionAgent.enemyAnima.SetTrigger(Animator.StringToHash("Attack2"));
    }

    public void Attack03()
    {
        if (!enemyLocomotionAgent.enemyInfoAgent.IsFighting || !enemyLocomotionAgent.enemyInfoAgent.IsAlive) return;
        SkillInfoAgent skillToUse = skills.Find(s => s.skillID.Substring(s.skillID.Length - 2) == "03");
        if (!skillToUse) return;
        if (CheckSkillCD(skillToUse)) enemyLocomotionAgent.enemyAnima.SetTrigger(Animator.StringToHash("Attack3"));
    }

    bool CheckSkillCD(SkillInfoAgent skillInfo)
    {
        if (!skillInfo.isCD && !skillInfo.usableWhenInCD) return false;
        return true;
    }
}
