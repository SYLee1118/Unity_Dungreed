using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int maxHP;
    [SerializeField] protected int currentHP;
    [SerializeField] protected int damage;
    [Space]
    [SerializeField] protected SpriteRenderer enemySprite;
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected Rigidbody2D enemyRigid;
    [Space]
    [SerializeField] protected PlayerDetection playerDetection;

    protected WaitForSeconds delay01 = new WaitForSeconds(0.1f);
    protected Coroutine blinkCoroutine;

    public int MaxHP { get { return maxHP; } }
    public int CurrentHP { get { return currentHP; } }
    public int Damage { get { return damage; } }

    public virtual void UpdateAnimatorParameter() { }

    public virtual void GetDamage(int damage, Transform attackerTransform)
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
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(BlinkSprite());
            else
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = StartCoroutine(BlinkSprite());
            }
        }
    }

    public virtual void DeActivate()
    {
        gameObject.SetActive(false);
    }

    protected IEnumerator BlinkSprite()
    {
        int count = 0;
        // 2초간 이미지 깜박임.
        while (count < 10)
        {
            if (count++ % 2 == 0)
                enemySprite.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            else
                enemySprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return delay01;
        }
    }
}
