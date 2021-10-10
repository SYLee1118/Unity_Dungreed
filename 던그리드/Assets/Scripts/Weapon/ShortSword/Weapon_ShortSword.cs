using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ShortSword : Weapon_Base
{
    [SerializeField] private Animator swingFXAnimator;
    [SerializeField] private SFXPool sfxPool;
    // 검을 휘두르는 모션 상태. 상태에 따라 각도를 조정한다.
    private enum State
    {
        STATE01,
        STATE02
    }
    private State state;

    private void Start()
    {
        state = State.STATE01;
    }

    protected override void AttackKeyDown()
    {
        base.AttackKeyDown();
        Vector3 scale = transform.localScale;
        switch (state)
        {
            case State.STATE01:
                sfxPool.Play("Swing");
                swingFXAnimator.SetTrigger("Swing");
                transform.localScale = new Vector3(scale.x, -1, scale.z);
                state = State.STATE02;
                break;

            case State.STATE02:
                sfxPool.Play("Swing");
                swingFXAnimator.SetTrigger("Swing");
                transform.localScale = new Vector3(scale.x, 1, scale.z);
                state = State.STATE01;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy.CurrentHP != 0)
                sfxPool.Play("Hit");
            enemy.GetDamage(damage, transform);
        }
    }
}
