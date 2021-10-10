using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcher_Bow_ReadyToShotState : StateMachineBehaviour
{
    SkelArcherBowBehaviour bow;
    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bow = animator.GetComponentInParent<SkelArcherBowBehaviour>();
        timer = 0.0f;
        bow.Ready();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f)
            animator.SetTrigger("Shot");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
