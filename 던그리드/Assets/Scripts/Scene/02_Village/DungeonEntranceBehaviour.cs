using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntranceBehaviour : MonoBehaviour
{
    [SerializeField] SFXPool sfxPool;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            animator.SetTrigger("Activate");
            GameManager.Instance.player.Pause();
        }
    }

    // 던전 입구 애니메이션이, 플레이어를 집어삼키는 순간 플레이어가 사라지도록
    private void ClipMethod_DeactivatePlayer()
    {
        GameManager.Instance.player.gameObject.SetActive(false);
    }

    // 던전 입구 애니메이션이 끝나면, Dungeon씬으로 이동
    private void ClipMethod_ChangeScene()
    {
        SceneController.Instance.LoadScene("Dungeon");
    }

    private void ClipMethod_PlaySFX_DungeonEntranceOut()
    {
        sfxPool.Play("DungeonEntranceOut");
    }

    private void ClipMethod_PlaySFX_DungeonEntranceIn()
    {
        sfxPool.Play("DungeonEntranceIn");
    }
}
