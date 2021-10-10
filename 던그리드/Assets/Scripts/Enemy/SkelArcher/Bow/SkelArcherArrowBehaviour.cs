using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcherArrowBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Shot(Vector3 pos ,Vector2 dir, int damage)
    {
        transform.position = pos;
        transform.up = dir;
        this.damage = damage;
        rigid.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.player.GetDamage(damage, transform);
            gameObject.SetActive(false);
        }
        else if(collision.tag == "Base")
        {
            gameObject.SetActive(false);
        }
    }
}
