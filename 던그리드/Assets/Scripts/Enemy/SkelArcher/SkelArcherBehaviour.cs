using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcherBehaviour : EnemyBase
{
    [SerializeField] SkelArcherBowBehaviour bow;
    [SerializeField] GameObject deadSkull;
    [SerializeField] Animator bowAnimator;
    [SerializeField] Vector2 knockBackPower;

    private void Update()
    {
        UpdateAnimatorParameter();
        UpdateDirection();
    }

    public override void UpdateAnimatorParameter()
    {
        base.UpdateAnimatorParameter();
        bowAnimator.SetBool("isPlayerDetected", playerDetection.isPlayerDetected);
    }

    protected void UpdateDirection()
    {
        Vector2 playerPos = GameManager.Instance.player.transform.position;
        if (transform.position.x > playerPos.x)
            FlipToRight(false);
        else
            FlipToRight(true);
    }
    
    public void Die()
    {
        // 해골이 튀어나와서 굴러가도록
        enemySprite.gameObject.SetActive(false);
        bow.gameObject.SetActive(false);
        deadSkull.SetActive(true);

        Vector2 randomForce = new Vector2(Random.Range(-6.0f, 6.0f), 6.0f);
        deadSkull.GetComponent<Rigidbody2D>().AddForce(randomForce, ForceMode2D.Impulse);

        Invoke("DeActivate", 3.0f);
    }

    public void FlipToRight(bool isRight)
    {
        if (isRight)
            enemySprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else
            enemySprite.gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    }

    public override void GetDamage(int damage, Transform attackerTransform)
    {
        if (currentHP == 0)
            return;

        if (damage >= currentHP)
        {
            currentHP = 0;
            enemyAnimator.SetTrigger("Die");
        }
        else
        {
            currentHP -= damage;
            StopAllCoroutines();
            StartCoroutine(BlinkSprite());
        }

        //넉백
        enemyRigid.velocity = Vector2.zero;

        if (transform.position.x < attackerTransform.position.x)
            enemyRigid.AddForce(new Vector2(knockBackPower.x * -1.0f, knockBackPower.x), ForceMode2D.Impulse);
        else
            enemyRigid.AddForce(knockBackPower, ForceMode2D.Impulse);
    }
}
