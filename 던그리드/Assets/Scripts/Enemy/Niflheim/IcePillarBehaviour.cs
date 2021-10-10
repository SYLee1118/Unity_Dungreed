using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePillarBehaviour : EnemyBase
{
    [SerializeField] NiflheimBehaviour niflheim;
    [SerializeField] ObjectPool iceBulletPool;
    [SerializeField] SFXPool sfxPool;

    // 얼음기둥이 부서질때마다 니플헤임에서 전부 부서졌는지 체크, 스턴 상황에 돌입하기 위해서.
    public delegate void delegateEvent();
    public event delegateEvent pillarDestroyedEvent;

    BoxCollider2D collider2D;

    public bool isDestroyed { get; private set; }
    
    private void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        StartCoroutine(SpinPillar());
        isDestroyed = false;
    }

    // 무작위방향으로 사격
    public void Shot_Spread()
    {
        StartCoroutine(ShotSpread());
    }

    // 플레이어에게 집중사격
    public void Shot_Concentration()
    {
        StartCoroutine(ShotConcentration());
    }

    public void Die()
    {
        sfxPool.Play("IcePillarDestroyed");
        isDestroyed = true;
        enemySprite.color = new Color(1, 1, 1, 1);
        StopAllCoroutines();
        collider2D.enabled = false;
        pillarDestroyedEvent();
        enemyAnimator.SetTrigger("Die");
        Invoke("DeActivate", 0.5f);
    }

    public void Regen()
    {
        isDestroyed = false;
        gameObject.SetActive(true);
        StartCoroutine(SpinPillar());
        collider2D.enabled = true;
        currentHP = maxHP;
        enemyAnimator.SetTrigger("Regen");
    }

    public override void GetDamage(int damage, Transform attackerTransform)
    {
        if (currentHP == 0)
            return;

        if (damage >= currentHP)
        {
            currentHP = 0;
            Die();
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

    IEnumerator SpinPillar()
    {
        while (true)
        {
            yield return null;
            Vector3 rotation = transform.localRotation.eulerAngles;
            // 초당 1회 회전
            rotation.z += 360.0f * Time.deltaTime;
            if (rotation.z > 360.0f)
                rotation.z -= 360.0f;
            transform.localRotation = Quaternion.Euler(rotation);
        }
    }

    IEnumerator ShotSpread()
    {
        for(int i = 0; i < 20; i++)
        {
            yield return delay01;
            sfxPool.Play("CastIceBullet");
            float shotAngle = Random.Range(0.0f, 360.0f);
            Vector2 shotDirection = new Vector2(Mathf.Cos(shotAngle * Mathf.Deg2Rad), Mathf.Sin(shotAngle * Mathf.Deg2Rad));
            GameObject iceBullet = iceBulletPool.GetObject();
            iceBullet.transform.up = shotDirection;
            iceBullet.GetComponent<EnemyBulletBehaviour>().Shot(transform.position, shotDirection);
        }
    }

    IEnumerator ShotConcentration()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return delay01;
            sfxPool.Play("CastIceBullet");
            Vector2 playerPos = GameManager.Instance.player.transform.position;
            playerPos = new Vector2(playerPos.x, playerPos.y + 1.0f);

            Vector2 shotDirection = (playerPos - (Vector2)transform.position).normalized;

            GameObject iceBullet = iceBulletPool.GetObject();
            iceBullet.transform.up = shotDirection;
            iceBullet.GetComponent<EnemyBulletBehaviour>().Shot(transform.position, shotDirection);
        }
    }
}
