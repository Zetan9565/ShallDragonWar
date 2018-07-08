using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskCategory("Movement")]
    public class WithoutDistance : Conditional
    {

        public SharedGameObject target;
        public SharedFloat distance;
        float sqrDistance;

        public override void OnStart()
        {
            sqrDistance = distance.Value * distance.Value;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value)
            {
                if ((target.Value.transform.position - transform.position).sqrMagnitude > sqrDistance)
                    return TaskStatus.Success;
                return TaskStatus.Failure;
            }
            return TaskStatus.Failure;
        }

    }
}
