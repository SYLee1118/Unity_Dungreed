using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niflheim_DieState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Transform>().position = new Vector3(42f, 6f, -1f);    // 중앙의 발판으로 위치를 이동시킴.
        animator.GetComponentInParent<NiflheimBehaviour>().Die();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
