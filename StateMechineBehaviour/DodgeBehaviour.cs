using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBehaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerInfoManager.Instance.PlayerInfo.Current_MP -= 50;
        PlayerLocomotionManager.Instance.playerController.moveAble = false;
        PlayerLocomotionManager.Instance.playerController.rotateAble = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerLocomotionManager.Instance.autoForward = true;
        PlayerLocomotionManager.Instance.autoFwdSpeed = 10.0f;
        PlayerInfoManager.Instance.PlayerInfo.SuperArmor = true;
        if (animator.IsInTransition(0) &&  animator.GetNextAnimatorStateInfo(0).IsName("Dodge-Finished"))
        {
            PlayerLocomotionManager.Instance.autoFwdSpeed = 3.0f;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!animator.GetNextAnimatorStateInfo(0).IsName("Dodge-Finished"))
        {
            PlayerLocomotionManager.Instance.playerController.moveAble = true;
            PlayerLocomotionManager.Instance.playerController.rotateAble = true;
        }
        PlayerLocomotionManager.Instance.autoForward = false;
        PlayerLocomotionManager.Instance.autoFwdSpeed = 0;
        PlayerInfoManager.Instance.PlayerInfo.SuperArmor = false;
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
