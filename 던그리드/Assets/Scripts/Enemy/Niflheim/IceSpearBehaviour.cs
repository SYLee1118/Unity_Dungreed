using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpearBehaviour : MonoBehaviour
{
    [SerializeField] Animator warningAC;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float speed;
    [SerializeField] int damage;
    [SerializeField] SFXPool sfxPool;

    public void Aim()
    {
        sfxPool.Play("CastSpear");

        gameObject.SetActive(true);
        rigid.velocity = Vector2.zero;

        // 랜덤하게 창이 천장에서부터 떨어질지, 바닥에서 솟구칠지 결정.
        int upOrDown = Random.Range(0, 2);
        // 창이 천장에서 떨어질 경우
        if(upOrDown == 0)
            transform.position = new Vector3(Random.Range(25.0f, 59.0f), 24.0f, transform.position.z);
        // 창이 바닥에서 솟구칠 경우
        else if (upOrDown == 1)
            transform.position = new Vector3(Random.Range(25.0f, 59.0f), -12.0f, transform.position.z);

        Vector2 playerPos = GameManager.Instance.player.transform.position;
        // 플레이어의 몸 중심을 타겟팅하기위한 보정
        playerPos = new Vector2(playerPos.x, playerPos.y + 1.0f);

        // 플레이어를 조준하도록 회전.
        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;
        transform.up = direction;

        // 위험표시 애니메이션 시작
        warningAC.SetTrigger("Activate");

        // 1초뒤 발사
        Invoke("Shot", 1.0f);
    }

    public void Shot()
    {
        sfxPool.Play("ShotSpear");

        Vector2 speedVector = transform.up * speed;
        rigid.velocity = speedVector;

        Invoke("Deactivate", 3.0f);
    }

    void Deactivate()
    {
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.player.GetDamage(damage, transform);
        }
    }
}
