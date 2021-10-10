using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeBehaviour : EnemyBase
{
    [SerializeField] float moveVelocity;
    [SerializeField] SFXPool sfxPool;
    [SerializeField] ObjectPool bulletPool;

    private void Update()
    {
        UpdateAnimatorParameter();
    }

    public override void UpdateAnimatorParameter()
    {
        base.UpdateAnimatorParameter();

        enemyAnimator.SetBool("isPlayerDetected", playerDetection.isPlayerDetected);
    }

    public void FollowPlayer()
    {
        Vector2 bansheeToPlayer = GameManager.Instance.player.transform.position - transform.position + new Vector3(0, 1.0f, 0);
        Vector2 direction = bansheeToPlayer.normalized;

        enemyRigid.velocity = direction * moveVelocity;
    }

    public void Shot()
    {
        enemyRigid.velocity = Vector2.zero;
        sfxPool.Play("Attack");
        // 30도 간격으로 12방향 bullet을 발사
        for(int i = 0; i <12; i++)
        {
            Vector2 shotDirection = new Vector2(Mathf.Cos(i * 30.0f * Mathf.Deg2Rad), Mathf.Sin(i * 30.0f * Mathf.Deg2Rad));
            bulletPool.GetObject().GetComponent<EnemyBulletBehaviour>().Shot(transform.position, shotDirection);
        }
    }

    public void Die()
    {
        enemyRigid.velocity = Vector2.zero;
        Invoke("DeActivate", 2.0f);
    }
}
