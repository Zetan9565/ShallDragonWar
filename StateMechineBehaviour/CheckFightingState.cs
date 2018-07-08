using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFightingState : StateMachineBehaviour {

    bool isChecked;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isChecked) return;
        if (PlayerInfoManager.Instance && PlayerInfoManager.Instance.PlayerInfo != null && PlayerInfoManager.Instance.PlayerInfo.IsFighting)
        {
            PlayerWeaponManager.Instance.SwapWeaponStatu(true);
            isChecked = true;
        }
        else
        {
            if (PlayerWeaponManager.Instance && PlayerWeaponManager.Instance.isInit) PlayerWeaponManager.Instance.SwapWeaponStatu(false);
            isChecked = true;
        }
    }

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        isChecked = false;
	}
}
