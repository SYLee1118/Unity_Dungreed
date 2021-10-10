using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelDog_FollowState : StateMachineBehaviour
{
    SkeldogBehaviour skelDog;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skelDog = animator.GetComponentInParent<SkeldogBehaviour>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 enemyPos = animator.transform.position;
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        if (enemyPos.x >= playerPos.x)
        {
            skelDog.MoveLeft();
            skelDog.FlipToRight(false);
        }
        else
        {
            skelDog.MoveRight();
            skelDog.FlipToRight(true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
