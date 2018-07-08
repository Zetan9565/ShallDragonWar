using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAtkBehaviour : StateMachineBehaviour {

    public string skillID;
    public float totalFrame;
    public float startFrame;
    public float endFrame;
    public float trailSFrame;
    public float trailEFrame;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SetSkill();
        //PlayerLocomotionManager.Self.playerController.m_Rigidbody.Sleep();
        //PlayerLocomotionManager.Self.playerController.moveAble = false;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerLocomotionManager.Instance.playerController.moveAble = false;
        SetSkill();
        if (stateInfo.normalizedTime >= trailSFrame / totalFrame && stateInfo.normalizedTime < trailEFrame / totalFrame)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrail(false, 1.5f, true);
        }
        if (stateInfo.normalizedTime >= startFrame / totalFrame && stateInfo.normalizedTime < endFrame / totalFrame)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
        if (animator.IsInTransition(0) && !animator.GetNextAnimatorStateInfo(0).IsTag("NormalAtk") && !animator.GetNextAnimatorStateInfo(0).IsTag("Skill"))
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            //PlayerLocomotionManager.Self.playerController.moveAble = true;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!animator.GetNextAnimatorStateInfo(0).IsTag("NormalAtk") && !animator.GetNextAnimatorStateInfo(0).IsTag("Skill"))
        {
            //PlayerLocomotionManager.Self.playerController.moveAble = true;
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerWeaponManager.Instance.SetFrontTrail(false);
            PlayerWeaponManager.Instance.SetBackTrigger(false);
            PlayerSkillManager.Instance.skillNowUsing = null;
        }
    }

    void SetSkill()
    {
        PlayerSkillManager.Instance.skillNowUsing = PlayerSkillManager.Instance.weaponOneSkills.Find(s => s.skillID == skillID);
        if (PlayerSkillManager.Instance.skillNowUsing)
            PlayerSkillManager.Instance.skillNowUsing.OnUsed();
        else
        {
            PlayerSkillManager.Instance.skillNowUsing = PlayerSkillManager.Instance.weaponTwoSkills.Find(s => s.skillID == skillID);
            if (PlayerSkillManager.Instance.skillNowUsing)
                PlayerSkillManager.Instance.skillNowUsing.OnUsed();
            else
                NotificationManager.Instance.NewNotification("技能错误" + skillID);
        }
    }
}
