using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    Rigidbody2D rigid;
    Animator anim;
    Collider2D col2D;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col2D = GetComponent<BoxCollider2D>();
    }

    public void Shot(Vector3 pos, Vector2 dir)
    {
        col2D.enabled = true;
        anim.SetTrigger("Idle");
        transform.position = pos;
        rigid.velocity = dir * speed;
    }

    public void Hit()
    {
        // 부딪히는 판정이 나는 즉시 더이상 충돌이 일어나지 못하도록.
        col2D.enabled = false;
        anim.SetTrigger("Hit");
        rigid.velocity = Vector2.zero;
        Invoke("Deactivate", 2.0f);
    }

   void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌시 데미지 입힘
        if (collision.tag == "Player")
        {
            Hit();
            GameManager.Instance.player.GetDamage(damage, transform);
        }
        // 무기와 부딪혀도 사라지도록
        else if (collision.tag == "Base" || collision.tag == "Weapon")
        {
            Hit();
        }
    }
}
