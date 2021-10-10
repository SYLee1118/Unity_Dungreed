using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor_Exit_Behaviour : MonoBehaviour
{
    [SerializeField] FloatingFkeyController floatingFkey;
    [SerializeField] GameObject[] enemies;
    [SerializeField] Animator animator;

    private WaitForSeconds delay1 = new WaitForSeconds(1.0f);
    private bool isDoorOpen = false;

    private void Start()
    {
        StartCoroutine(checkEnemyAllDie());
        floatingFkey.keyPressedEvent += Event_GoToBossRoom;
    }

    private void Event_GoToBossRoom()
    {
        SceneController.Instance.LoadScene("BossRoom");
    }

    // 일정 주기로 적들이 전부 적었는지 체크.
    IEnumerator checkEnemyAllDie()
    {
        while(true)
        {
            yield return delay1;

            bool isAllEnemyDie = true;
            // 적들중 하나라도 살아있다면, 1초후 다시확인.
            foreach(GameObject go in enemies)
            {
                if (go.activeSelf == true)
                    isAllEnemyDie = false;
            }

            if (isAllEnemyDie == true)
            {
                // 적들이 전부 죽어있는 상태라면,
                animator.SetTrigger("DoorOpen");
                floatingFkey.gameObject.SetActive(true);
            }
        }
    }
}
