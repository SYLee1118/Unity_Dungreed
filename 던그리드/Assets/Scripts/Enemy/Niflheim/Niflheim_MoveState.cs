using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niflheim_MoveState : StateMachineBehaviour
{
    Transform niflheimTransform;
    // 니플하임이 도달할 수 있는 위치들
    Vector3[] destinations = new Vector3[5] {new Vector3(42f, 7.5f, -1f), new Vector3(53.5f, 10.5f, -1f), new Vector3(53.5f, 2f, -1f),
                                                              new Vector3(30.5f, 10.5f, -1f), new Vector3(30.5f, 2f, -1f) };
    // 선택된 목표점
    Vector3 destinationPos;
    Vector3 startPos;
    float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        niflheimTransform = animator.GetComponentInParent<NiflheimBehaviour>().transform;
        startPos = niflheimTransform.position;
        destinationPos = destinations[Random.Range(0, 5)];
        timer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
            timer = 1f;

        // 일정하게 이동.
        niflheimTransform.position = Vector3.Lerp(startPos, destinationPos, timer);

        // 목표지점에 도달했다면 공격상태로 넘어가기.
        if (timer == 1f)
            animator.SetTrigger("Attack");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
