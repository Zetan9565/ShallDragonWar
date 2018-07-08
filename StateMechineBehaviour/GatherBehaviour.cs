using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherBehaviour : StateMachineBehaviour
{
    int gatherType;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (animator.GetInteger(Animator.StringToHash("GatherType")))
        {
            case 0: MyTools.SetActive(PlayerLocomotionManager.Instance.GatherTools.Find(g => g.name == "Spade"), true); break;
            case 1: MyTools.SetActive(PlayerLocomotionManager.Instance.GatherTools.Find(g => g.name == "Shovel"), true); break;
            case 2: MyTools.SetActive(PlayerLocomotionManager.Instance.GatherTools.Find(g => g.name == "Hatchet"), true); break;
        }
        PlayerLocomotionManager.Instance.StartGather();
        PlayerLocomotionManager.Instance.playerRigidbd.Sleep();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    /*override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }*/

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (animator.GetInteger(Animator.StringToHash("GatherType")))
        {
            case 0: MyTools.SetActive(PlayerLocomotionManager.Instance.GatherTools.Find(g => g.name == "Spade"), false); break;
            case 1: MyTools.SetActive(PlayerLocomotionManager.Instance.GatherTools.Find(g => g.name == "Shovel"), false); break;
            case 2: MyTools.SetActive(PlayerLocomotionManager.Instance.GatherTools.Find(g => g.name == "Hatchet"), false); break;
        }
        if (TimeProgressBarManager.Instance.isStart) PlayerLocomotionManager.Instance.CancelGather();
    }
}
