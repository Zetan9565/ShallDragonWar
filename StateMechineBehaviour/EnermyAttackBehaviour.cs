using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyAttackBehaviour : StateMachineBehaviour {

    public string SkillID;
    public float totalFrame;
    [Space]
    [Header("一段攻击")]
    public float firstAtkStartFrame;
    public float firstAtkEndFrame;
    [Space]
    public bool effectStatuAtFirstAtk;
    public bool superArmorAtFirstAtk;
    public bool frontTriggerAtFirstAtk;
    public bool backTriggerAtFirstAtk;
    /*[Space]
    public bool autoForwardAtFirstAtk;
    public float firstAutoForwadStarFrame;
    public float firstAutoForwadEndFrame;
    public float firstForwardSpeed;*/
    [Space]
    [Header("二段攻击")]
    public float secondAtkStartFrame;
    public float secondAtkEndFrame;
    [Space]
    public bool effectStatuAtSecondAtk;
    public bool superArmorAtSecondAtk;
    public bool frontTriggerAtSecondAtk;
    public bool backTriggerAtSecondAtk;
    /*[Space]
    public bool autoForwardAtSecondAtk;
    public float secondAutoForwadStarFrame;
    public float secondAutoForwadEndFrame;
    public float secondForwardSpeed;*/

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SetSkillAtStart(animator);
        animator.ResetTrigger(Animator.StringToHash("Attack1"));
        animator.ResetTrigger(Animator.StringToHash("Attack2"));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetSkill(animator);
        if (stateInfo.normalizedTime >= firstAtkStartFrame / totalFrame && stateInfo.normalizedTime < firstAtkEndFrame / totalFrame)
        {
            if (frontTriggerAtFirstAtk) animator.GetComponent<EnemyLocomotionAgent>().SetFrontWeaponTrigger(true);
            if (backTriggerAtFirstAtk) animator.GetComponent<EnemyLocomotionAgent>().SetBackWeaponTrigger(true);
            if (effectStatuAtFirstAtk) animator.GetComponent<EnemySkillsAgent>().currentSkill.statuEffcted = true;
            if (superArmorAtFirstAtk) animator.GetComponent<EnemyInfoAgent>().SuperArmor = false;
        }
        else if (stateInfo.normalizedTime >= secondAtkStartFrame / totalFrame && stateInfo.normalizedTime < secondAtkEndFrame / totalFrame)
        {
            if (frontTriggerAtSecondAtk) animator.GetComponent<EnemyLocomotionAgent>().SetFrontWeaponTrigger(true);
            if (backTriggerAtSecondAtk) animator.GetComponent<EnemyLocomotionAgent>().SetBackWeaponTrigger(true);
            if (effectStatuAtSecondAtk) animator.GetComponent<EnemySkillsAgent>().currentSkill.statuEffcted = true;
            if (superArmorAtSecondAtk) animator.GetComponent<EnemyInfoAgent>().SuperArmor = true;
        }
        else
        {
            animator.GetComponent<EnemyLocomotionAgent>().SetFrontWeaponTrigger(false);
            animator.GetComponent<EnemyLocomotionAgent>().SetBackWeaponTrigger(false);
            if(animator.GetComponent<EnemySkillsAgent>().currentSkill) animator.GetComponent<EnemySkillsAgent>().currentSkill.statuEffcted = false;
            animator.GetComponent<EnemyInfoAgent>().SuperArmor = false;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<EnemyLocomotionAgent>().SetFrontWeaponTrigger(false);
        animator.GetComponent<EnemyLocomotionAgent>().SetBackWeaponTrigger(false);
        animator.GetComponent<EnemySkillsAgent>().currentSkill.statuEffcted = false;
        animator.GetComponent<EnemySkillsAgent>().currentSkill = null;
        animator.ResetTrigger(Animator.StringToHash("Attack1"));
        animator.ResetTrigger(Animator.StringToHash("Attack2"));
    }

    void SetSkillAtStart(Animator animator)
    {
        EnemySkillsAgent skillsAgent = animator.GetComponent<EnemySkillsAgent>();
        skillsAgent.currentSkill = skillsAgent.skills.Find(s => s.skillID == SkillID);
        if (!skillsAgent.currentSkill)
        {
            Debug.Log("敌人技能错误：" + SkillID);
        }
        else skillsAgent.currentSkill.OnUsed();
    }

    void SetSkill(Animator animator)
    {
        EnemySkillsAgent skillsAgent = animator.GetComponent<EnemySkillsAgent>();
        skillsAgent.currentSkill = skillsAgent.skills.Find(s => s.skillID == SkillID);
        if (!skillsAgent.currentSkill)
        {
            Debug.Log("敌人技能错误：" + SkillID);
        }
    }
}
