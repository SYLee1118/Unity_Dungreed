using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcher_Bow_AimPlayerState : StateMachineBehaviour
{
    SkelArcherBowBehaviour bow;
    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bow = animator.GetComponentInParent<SkelArcherBowBehaviour>();
        timer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bow.AimPlayer();
        timer += Time.deltaTime;
        if(timer >= 2.0f)
        {
            animator.SetTrigger("Ready");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
