using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SetEnemyFighting : Action{

    EnemyInfoAgent enemyInfoAgent;
    public bool fighting;

    public override void OnAwake()
    {
        enemyInfoAgent = GetComponent<EnemyInfoAgent>();
    }

    public override void OnStart()
    {
        if (enemyInfoAgent) enemyInfoAgent.IsFighting = fighting;
    }
}
