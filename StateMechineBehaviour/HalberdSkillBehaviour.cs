using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalberdSkillBehaviour : StateMachineBehaviour {

    public string skillID;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SetSkillAtEnter();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SetSkill();
        switch (skillID)
        {
            case "Halberd01":Skill01(stateInfo);break;
            case "Halberd02":Skill02(stateInfo);break;
            case "Halberd03":Skill03(stateInfo);break;
            case "Halberd04":Skill04(stateInfo);break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PlayerWeaponManager.Instance.SetFrontTrigger(false);
        PlayerWeaponManager.Instance.SetBackTrigger(false);
        PlayerSkillManager.Instance.skillNowUsing = null;
    }

    void Skill01(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 10f / 63f && stateInfo.normalizedTime < 52f / 63f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        }
        if (stateInfo.normalizedTime >= 11f / 63f && stateInfo.normalizedTime < 20f / 63f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 22f / 63f && stateInfo.normalizedTime < 28f / 63f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 35f / 63f && stateInfo.normalizedTime < 42f / 63f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 47f / 63f && stateInfo.normalizedTime < 51f / 63f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
        }
    }

    void Skill02(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 18f / 30f && stateInfo.normalizedTime < 27f / 30f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        }
        if (stateInfo.normalizedTime >= 22f / 30f && stateInfo.normalizedTime < 26f / 30f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerWeaponManager.Instance.SetBackTrigger(false);
        }
    }

    void Skill03(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 13f / 30f && stateInfo.normalizedTime < 17f / 30f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        }
        if (stateInfo.normalizedTime >= 16f / 30f && stateInfo.normalizedTime < 22f / 30f)
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
            PlayerWeaponManager.Instance.SetBackTrigger(true);
        }
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerWeaponManager.Instance.SetBackTrigger(false);
        }
    }

    void Skill04(AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= 0f / 55f && stateInfo.normalizedTime < 40f / 55f)
            PlayerWeaponManager.Instance.SetFrontTrail(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrail(false);
        }
        if (stateInfo.normalizedTime >= 16f / 55f && stateInfo.normalizedTime < 21f / 55f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 32f / 55f && stateInfo.normalizedTime < 35f / 55f)
            PlayerWeaponManager.Instance.SetFrontTrigger(true);
        else if (stateInfo.normalizedTime >= 35f / 55f && stateInfo.normalizedTime < 40f / 55f)
            PlayerWeaponManager.Instance.SetBackTrigger(true);
        else
        {
            PlayerWeaponManager.Instance.SetFrontTrigger(false);
            PlayerWeaponManager.Instance.SetBackTrigger(false);
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
