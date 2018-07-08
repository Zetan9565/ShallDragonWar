using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class AttackTask : Action
{
    EnemySkillsAgent enemySkillsAgent;
    [Range(1, 3)]
    public SharedInt AtkTypeCount;

    public override void OnAwake()
    {
        enemySkillsAgent = GetComponent<EnemySkillsAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!enemySkillsAgent) return TaskStatus.Failure;
        int selectAtk = Random.Range(0, AtkTypeCount.Value);
        if (selectAtk == 0)
            enemySkillsAgent.Attack01();
        else if (selectAtk == 1)
            enemySkillsAgent.Attack02();
        else
            enemySkillsAgent.Attack03();
        return TaskStatus.Success;
    }
}
