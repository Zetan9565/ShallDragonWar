using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapFightBehaviour : StateMachineBehaviour {

    public bool swapToFight;
    public float swapNormalizeTime;
    bool isStart;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isStart = true;
        PlayerLocomotionManager.Instance.playerRigidbd.Sleep();
        PlayerLocomotionManager.Instance.playerController.moveAble = false;
        PlayerLocomotionManager.Instance.playerController.rotateAble = false;
        if (swapToFight)
        {
            animator.SetBool(Animator.StringToHash("TakeOutWp"), false);
            PlayerInfoManager.Instance.PlayerInfo.IsFighting = true;
        }
        else
        {
            animator.SetBool(Animator.StringToHash("SheathWp"), false);
            PlayerInfoManager.Instance.PlayerInfo.IsFighting = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isStart)
        {
            if (stateInfo.normalizedTime >= swapNormalizeTime)
            {
                if (swapToFight)
                {
                    PlayerWeaponManager.Instance.SwapWeaponStatu(true);
                }
                else
                {
                    PlayerWeaponManager.Instance.SwapWeaponStatu(false);
                }
                isStart = false;
            }
        }
        if (animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsTag("Move"))
        {
            PlayerLocomotionManager.Instance.playerController.moveAble = true;
            PlayerLocomotionManager.Instance.playerController.rotateAble = true;
        }
        else
        {
            PlayerLocomotionManager.Instance.playerController.moveAble = false;
            PlayerLocomotionManager.Instance.playerController.rotateAble = false;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerLocomotionManager.Instance.playerController.moveAble = true;
        PlayerLocomotionManager.Instance.playerController.rotateAble = true;
    }
}
