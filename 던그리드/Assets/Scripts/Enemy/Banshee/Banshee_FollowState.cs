using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banshee_FollowState : StateMachineBehaviour
{
    float timer;
    BansheeBehaviour banshee;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0.0f;
        banshee = animator.GetComponentInParent<BansheeBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        banshee.FollowPlayer();

        if (timer >= 3.0f)
            animator.SetTrigger("Shot");

        timer += Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
