using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class OutOfTime : Conditional {

    public float outOfTime;
    float currentTime;

    public override void OnStart()
    {
        currentTime = 0;
    }

    public override TaskStatus OnUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > outOfTime)
        {
            currentTime = 0;
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }

    public override void OnBehaviorComplete()
    {
        base.OnBehaviorComplete();
    }
}
