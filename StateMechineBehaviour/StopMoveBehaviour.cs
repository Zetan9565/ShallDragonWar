using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMoveBehaviour : StateMachineBehaviour
{
    public enum FreezeType
    {
        Move,
        Rotate,
        Both
    }
    public bool sleepRigidbdWhenEnter;
    public FreezeType freezeType;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Debug.Log("Enter");
        if (sleepRigidbdWhenEnter)
        {
            //Debug.Log("DASDASD");
            animator.GetComponent<PlayerController>().m_Rigidbody.Sleep();
        }
        if (freezeType == FreezeType.Move || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().moveAble = false;
        if (freezeType == FreezeType.Rotate || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().rotateAble = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).IsTag("Move"))
        {
            if (freezeType == FreezeType.Move || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().moveAble = true;
            if (freezeType == FreezeType.Rotate || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().rotateAble = true;
        }
        else
        {
            if (freezeType == FreezeType.Move || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().moveAble = false;
            if (freezeType == FreezeType.Rotate || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().rotateAble = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (freezeType == FreezeType.Move || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().moveAble = true;
        if (freezeType == FreezeType.Rotate || freezeType == FreezeType.Both) animator.GetComponent<PlayerController>().rotateAble = true;
    }
}
