using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcher_DieState : StateMachineBehaviour
{
    SkelArcherBehaviour skel;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skel = animator.GetComponentInParent<SkelArcherBehaviour>();
        skel.Die();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
