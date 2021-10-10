using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcher_Bow_ShotState : StateMachineBehaviour
{
    SkelArcherBowBehaviour bow;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bow = animator.GetComponentInParent<SkelArcherBowBehaviour>();
        bow.Shot();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
