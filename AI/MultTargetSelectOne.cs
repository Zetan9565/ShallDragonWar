using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultTargetSelectOne : Action {

    public SharedGameObjectList transformList;
    public SharedGameObject resultTransform;

    public override void OnStart()
    {
        resultTransform.Value = transformList.Value[Random.Range(0, transformList.Value.Count - 1)];
    }
}
