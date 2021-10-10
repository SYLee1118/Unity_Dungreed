using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    [SerializeField] int damage;
    bool isPlayerContact = false;

    private void Update()
    {
        if(isPlayerContact)
            GameManager.Instance.player.GetDamage(damage, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            isPlayerContact = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            isPlayerContact = false;
    }
}
