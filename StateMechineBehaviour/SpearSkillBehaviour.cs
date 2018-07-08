using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSkillBehaviour : StateMachineBehaviour
{
    public string skillID;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetSkillAtEnter();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetSkill();
        switch (skillID)
        {
            case "Spear01": Skill01(stateInfo); break;
            case "Spear02": Skill02(stateInfo); break;
            case "Spear03": Skill03(stateInfo); break;
            case "Spear04": Skill04(stateInfo); break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetNextAnimatorStateInfo(0).IsTag("NormalAtk") || animator.GetNextAnimatorStateInfo(0).IsTag("Skill")) return;
        PlayerWeaponManager.Instance.SetFrontTrigger(false);
        PlayerWeaponManager.Instance.SetBackTrigger(false);
        PlayerSkillManager.Instance.skillNowUsing = null;
    }

    void Skill01(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 8.0f / 30.0f && stateInfo.normalizedTime < 19.0f / 30.0f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        if (stateInfo.normalizedTime >= 9.0f / 30.0f && stateInfo.normalizedTime < 12.0f / 30.0f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = true;
        }
        else if (stateInfo.normalizedTime >= 14.0f / 30.0f && stateInfo.normalizedTime < 17.0f / 30.0f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = true;
        }
        else if (stateInfo.normalizedTime >= 19.0f / 30.0f && stateInfo.normalizedTime < 22.0f / 30.0f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = true;
        }
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = false;
        }
    }

    void Skill02(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 10.0f / 45.0f && stateInfo.normalizedTime < 17.0f / 45.0f)
            PlayerWeaponManager.Instance.SetBackTrail(true);
        else
            PlayerWeaponManager.Instance.SetBackTrail(false);
        if (stateInfo.normalizedTime >= 21.0f / 45.0f && stateInfo.normalizedTime < 30.0f / 45.0f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        }
        if (stateInfo.normalizedTime >= 13.0f / 45.0f && stateInfo.normalizedTime < 16.0f / 45.0f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 23.0f / 45.0f && stateInfo.normalizedTime < 26.0f / 45.0f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = true;
        }
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = false;
        }
    }

    void Skill03(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 15.0f / 48.0f && stateInfo.normalizedTime < 40.0f / 48.0f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        if (stateInfo.normalizedTime >= 19.0f / 48.0f && stateInfo.normalizedTime < 23.0f / 48.0f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 24.0f / 48.0f && stateInfo.normalizedTime < 31.0f / 48.0f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 33f / 48.0f && stateInfo.normalizedTime < 36.0f / 48.0f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = true;
        }
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = false;
        }
    }

    void Skill04(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 10.0f / 45.0f && stateInfo.normalizedTime < 21.0f / 45.0f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else if (stateInfo.normalizedTime >= 26.0f / 45.0f && stateInfo.normalizedTime < 35.0f / 48.0f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        if (stateInfo.normalizedTime >= 13.0f / 45.0f && stateInfo.normalizedTime < 18.0f / 45.0f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 26.0f / 45.0f && stateInfo.normalizedTime < 30.0f / 45.0f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = true;
        }
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerSkillManager.Instance.skillNowUsing.statuEffcted = false;
        }
    }

    void SetSkillAtEnter()
    {
        PlayerSkillManager.Instance.skillNowUsing = PlayerSkillManager.Instance.weaponOneSkills.Find(s => s.skillID == skillID);
        if (PlayerSkillManager.Instance.skillNowUsing) PlayerSkillManager.Instance.skillNowUsing.OnUsed();
        else
        {
            PlayerSkillManager.Instance.skillNowUsing = PlayerSkillManager.Instance.weaponTwoSkills.Find(s => s.skillID == skillID);
            if (PlayerSkillManager.Instance.skillNowUsing) PlayerSkillManager.Instance.skillNowUsing.OnUsed();
            else NotificationManager.Instance.NewNotification("技能错误" + skillID);
        }
    }

    void SetSkill()
    {
        PlayerSkillManager.Instance.skillNowUsing = PlayerSkillManager.Instance.weaponOneSkills.Find(s => s.skillID == skillID);
        if (PlayerSkillManager.Instance.skillNowUsing)
            return;
        else
        {
            PlayerSkillManager.Instance.skillNowUsing = PlayerSkillManager.Instance.weaponTwoSkills.Find(s => s.skillID == skillID);
            if (PlayerSkillManager.Instance.skillNowUsing)
                return;
            else
                NotificationManager.Instance.NewNotification("技能错误" + skillID);
        }
    }
}
