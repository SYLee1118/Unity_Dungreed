using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeldogBehaviour : EnemyBase
{
    [Space]
    [SerializeField] Transform rayStartPoint1;
    [SerializeField] Transform rayStartPoint2;
    [Space]
    [SerializeField] float movePower;
    [SerializeField] float jumpPower;
    [SerializeField] Vector2 knockBackPower;

    bool isOnGround = false;

    private void Update()
    {
        UpdateAnimatorParameter();
    }

    public override void UpdateAnimatorParameter()
    {
        base.UpdateAnimatorParameter();

        enemyAnimator.SetBool("onGround", isOnGround);
        enemyAnimator.SetBool("isPlayerDetected", playerDetection.isPlayerDetected);

        Vector2 enemyPos = enemyAnimator.transform.position;
        Vector2 playerPos = GameManager.Instance.player.transform.position;

        Vector2 enemyToPlayer = playerPos - enemyPos;
        float enemyToPlayerXYRatio = Mathf.Abs(enemyToPlayer.x) / enemyToPlayer.y;
        enemyAnimator.SetFloat("playerDistanceXYRatio", enemyToPlayerXYRatio);
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

    public void MoveLeft()
    {
        Vector2 pos = transform.position;

        // 공중일경우 좌우로만 이동
        if (isOnGround == false)
        {
            enemyRigid.AddForce(new Vector2(-movePower / 2, 0.0f), ForceMode2D.Force);
            return;
        }

        // 경사면 인식용 raycast
        RaycastHit2D resultHit;
        RaycastHit2D hit1 = Physics2D.Raycast(rayStartPoint1.position, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Tile"));
        RaycastHit2D hit2 = Physics2D.Raycast(rayStartPoint2.position, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Tile"));

        // ray 표시
        Debug.DrawLine(rayStartPoint1.position, (Vector2)rayStartPoint1.position + hit1.normal * 3.0f, Color.green);
        Debug.DrawLine(rayStartPoint2.position, (Vector2)rayStartPoint2.position + hit2.normal * 3.0f, Color.green);

        // 기준이 될 ray 판별, 경사 확인
        if (rayStartPoint1.position.x < rayStartPoint2.position.x)
        {
            if (hit1.normal.x > 0)
                resultHit = hit1;
            else
                resultHit = hit2;
        }
        else
        {
            if (hit2.normal.x > 0)
                resultHit = hit2;
            else
                resultHit = hit1;
        }

        // 평지이거나, floor 끝에서 발을 걸치고 있는 상태인 경우 정상적으로 좌우로 이동
        if (resultHit.normal == Vector2.up || resultHit.normal == Vector2.zero)
        {
            enemyRigid.AddForce(new Vector2(-movePower, 0.0f), ForceMode2D.Force);
        }
        else
        {
            Quaternion rot = resultHit.normal.x > 0 ? Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x))) : 
                                                                    Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x)));
            Vector2 forceVector = resultHit.normal.x > 0 ? new Vector2(-movePower * 2, 0) : // 오르막을 오르기위한 보정
                                                                        new Vector2(-movePower, 0);
            Vector2 rotated = rot * forceVector;
            enemyRigid.AddForce(rotated, ForceMode2D.Force);
        }
    }
    
    public void MoveRight()
    {
        Vector2 pos = transform.position;

        if (isOnGround == false)
        {
            enemyRigid.AddForce(new Vector2(movePower, 0.0f), ForceMode2D.Force);
            return;
        }

        RaycastHit2D resultHit;
        RaycastHit2D hit1 = Physics2D.Raycast(rayStartPoint1.position, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Tile"));
        RaycastHit2D hit2 = Physics2D.Raycast(rayStartPoint2.position, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Tile"));

        // ray 표시
        Debug.DrawLine(rayStartPoint1.position, (Vector2)rayStartPoint1.position + hit1.normal * 3.0f, Color.green);
        Debug.DrawLine(rayStartPoint2.position, (Vector2)rayStartPoint2.position + hit2.normal * 3.0f, Color.green);

        if (rayStartPoint1.position.x > rayStartPoint2.position.x)
        {
            if (hit1.normal.x < 0)
                resultHit = hit1;
            else
                resultHit = hit2;
        }
        else
        {
            if (hit2.normal.x < 0)
                resultHit = hit2;
            else
                resultHit = hit1;
        }

        if (resultHit.normal == Vector2.up || resultHit.normal == Vector2.zero)
        {
            enemyRigid.AddForce(new Vector2(movePower, 0.0f), ForceMode2D.Force);
        }
        else
        {
            Quaternion rot = resultHit.normal.x < 0 ? Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x))) :
                                                                    Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x)));
            Vector2 forceVector = resultHit.normal.x > 0 ? new Vector2(movePower, 0) :
                                                                        new Vector2(movePower * 2, 0);
            Vector2 rotated = rot * forceVector;
            enemyRigid.AddForce(rotated, ForceMode2D.Force);
        }
    }

    public void Jump()
    {
        enemyRigid.AddForce(new Vector2(0.0f, jumpPower), ForceMode2D.Impulse);
    }

    public void Die()
    {
        transform.localScale = new Vector3(transform.localScale.x, -1.0f, 1.0f);
        Invoke("DeActivate", 3.0f);
    }

    public void FlipToRight(bool isRight)
    {
        if (isRight)
            enemySprite.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else
            enemySprite.gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Base" || collision.gameObject.tag == "Floor")
            isOnGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Base" || collision.gameObject.tag == "Floor")
            isOnGround = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && currentHP != 0)
            GameManager.Instance.player.GetDamage(damage, transform);
    }
}