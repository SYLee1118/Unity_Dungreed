using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    [SerializeField] Player_Base player;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform playerImageTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody2D playerRigid;
    [Space]
    // 바닥의 경사도 확인용 ray 시작점
    [SerializeField] Transform rayStartPoint1;
    [SerializeField] Transform rayStartPoint2;
    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] Vector2 knockBackImpulse;
    [Space]
    // 캐릭터 스프라이트 피봇이 좌측하단이라, 좌우반전할 때 기준삼을 x 좌표. (여러 캐릭터의 width가 다 다르다)
    [SerializeField] float centerXvalue;
    [Space]
    // 효과음 pool
    [SerializeField] SFXPool sfxPool;

    bool isRun = false;
    // 캐릭터가 발을 딛고있는지
    bool isOnGround = false;
    // 더블점프 체크용
    bool isDoubleJumpPossible = true;
    // 캐릭터가 발을 딛고있는 지면이 아랫점프가 가능한 바닥인지 체크
    bool isOnFloor = false;
    bool isPause = false;
    // 현재 접촉하고 있는 Floor
    Collision2D currentFloor;

    private void Update()
    {
        if (isPause)
            return;

        UpdateInput();
        UpdateAnimatorPrarmeter();
        UpdateImageDirection();
    }

    // 입력 처리.
    void UpdateInput()
    {
        if (player.CurrentHP == 0)
            return;

        // KeyManager에서 좌우, 하단, 점프키로 지정된 키로 동작되도록.
        // 조작키 변경에 대응하기 위함.
        isRun = false;
        if (KeyManager.Instance.GetKey(Key.LEFT) && !KeyManager.Instance.GetKey(Key.RIGHT))
        {
            MoveLeft();
            isRun = true;
        }
        if (KeyManager.Instance.GetKey(Key.RIGHT) && !KeyManager.Instance.GetKey(Key.LEFT))
        {
            MoveRight();
            isRun = true;
        }

        if (KeyManager.Instance.GetKeyDown(Key.JUMP))
        {
            // S키 누른채로 점프하면 아랫점프.
            if (KeyManager.Instance.GetKey(Key.DOWN))
                DownJump();
            else
                Jump();
        }
    }

    // 플레이어 애니메이터 파라미터를 업데이트.
    void UpdateAnimatorPrarmeter()
    {
        playerAnimator.SetBool("isOnGround", isOnGround);
        playerAnimator.SetBool("isRun", isRun);
    }

    // 커서의 위치에 따라서 플레이어 이미지가 바라보는 방향을 변경.
    void UpdateImageDirection()
    {
        //죽어있는 상태에선 flip이 필요없으므로 return.
        if (player.CurrentHP == 0)
            return;

        // 마우스 커서쪽을 바라보도록.
        // 플레이어 이미지의 피봇이 좌측 하단이라 flip에 이어서 위치의 보정도 해야한다.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x <= playerTransform.position.x)
        {
            playerImageTransform.localScale = new Vector3(-1, 1, 1);
            playerImageTransform.localPosition = new Vector3(centerXvalue, 0, 0);
        }
        else if (mousePosition.x > playerTransform.position.x)
        {
            playerImageTransform.localScale = new Vector3(1, 1, 1);
            playerImageTransform.localPosition = new Vector3(centerXvalue * -1.0f, 0, 0);
        }
    }

    public void MoveLeft()
    {
        Vector2 pos = playerTransform.position;

        // 공중일경우 좌우로만 이동
        if (isOnGround == false)   
        {
            Vector2 moveVector = new Vector2(-moveSpeed * Time.deltaTime, 0);
            playerRigid.position = pos + moveVector;
            return;
        }

        // 공중이 아닐경우에는, 현재 평지에 서있는지 경사면에 서있는지를 판별해서 움직여야한다.
        // 경사면에서 미끄러지듯 내려가기위해서.
        // ray의 길이를 지나치게 길게 설정할 경우, 발판에서 한 발을 걸치고 있는 경우 아래에 있는 경사로에 ray가 충돌, 인식해버릴 위험이 있음.
        RaycastHit2D resultHit;
        RaycastHit2D hit1 = Physics2D.Raycast(rayStartPoint1.position, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Tile"));
        RaycastHit2D hit2 = Physics2D.Raycast(rayStartPoint2.position, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Tile"));

        // ray 표시
        Debug.DrawLine(rayStartPoint1.position, (Vector2)rayStartPoint1.position + hit1.normal * 3.0f, Color.green);
        Debug.DrawLine(rayStartPoint2.position, (Vector2)rayStartPoint2.position + hit2.normal * 3.0f, Color.green);

        // 두 레이포인트 중 더 왼쪽에 위치한 것을 식별, normal 벡터로 오르막인지 내리막인지 판별하여
        // 기준이 될 hit를 정한다. (flip할 경우 raypoint도 같이 반전되므로, 이를 체크하기 위함.)
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
            Vector2 moveVector = new Vector2(-moveSpeed * Time.deltaTime, 0);
            playerRigid.position = pos + moveVector;
        }
        // 경사면일경우
        else
        {
            Quaternion rot = resultHit.normal.x > 0 ? Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x))) :  // 오르막
                                                                    Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x)));  // 내리막
            Vector2 moveVector = new Vector2(-moveSpeed * Time.deltaTime, 0);
            Vector2 rotated = rot * moveVector;
            playerRigid.position = pos + rotated;
        }
    }

    // MoveLeft와 동일
    public void MoveRight()
    {
        Vector2 pos = playerTransform.position;

        if (isOnGround == false)   
        {
            Vector2 moveVector = new Vector2(moveSpeed * Time.deltaTime, 0);
            playerRigid.position = pos + moveVector;
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
            Vector2 moveVector = new Vector2(moveSpeed * Time.deltaTime, 0);
            playerRigid.position = pos + moveVector;
        }
        else
        {
            Quaternion rot = resultHit.normal.x < 0 ? Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x))) :
                                                                    Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(resultHit.normal.y), Mathf.Abs(resultHit.normal.x)));
            Vector2 moveVector = new Vector2(moveSpeed * Time.deltaTime, 0);
            Vector2 rotated = rot * moveVector;
            playerRigid.position = pos + rotated;
        }
    }

    public void Jump()
    {
        // 땅에서의 점프. 즉, 최초의 점프.
        if (isOnGround == true)
        {
            Vector2 force = new Vector2(0, jumpPower);
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, 0);
            playerRigid.AddForce(force, ForceMode2D.Impulse);
            isOnGround = false;
            isOnFloor = false;
            sfxPool.Play("Jump");
        }
        // 공중에서, 더블점프를 한 적 없는지 체크.
        else if (isDoubleJumpPossible == true)
        {
            Vector2 force = new Vector2(0, jumpPower);
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, 0);
            playerRigid.AddForce(force, ForceMode2D.Impulse);
            isOnGround = false;
            isOnFloor = false;
            isDoubleJumpPossible = false;
            sfxPool.Play("Jump");
        }
    }

    public void DownJump()
    {
        // Base가 아닌 Floor에 발을 딛고있을 때만 아랫점프 (맵 밖으로 뚫고 나가지 못하도록)
        if (isOnFloor == true && currentFloor != null)
        {
            StartCoroutine(DeactiveFloor());
            isOnFloor = false;
            isOnGround = false;
            sfxPool.Play("Jump");
        }
    }

    // 던전에 입장할 때, 던전입구의 애니메이션이 실행될때 플레이어를 멈추게 하기 위함.
    public void Pause()
    {
        isPause = true;
        playerRigid.bodyType = RigidbodyType2D.Kinematic;
        playerAnimator.speed = 0.0f;
        playerRigid.velocity = Vector2.zero;
    }
    public void Resume()
    {
        isPause = false;
        playerRigid.bodyType = RigidbodyType2D.Dynamic;
        playerAnimator.speed = 1.0f;
    }

    // 데미지를 입었을 때의 넉백처리
    public void KnockBack(Transform attackerTransform)
    {
        playerRigid.velocity = Vector2.zero;

        // 데미지를 준 오브젝트의 반대방향으로 밀리도록.
        if(player.transform.position.x < attackerTransform.position.x)
            playerRigid.AddForce(new Vector2(knockBackImpulse.x * -1.0f, knockBackImpulse.y), ForceMode2D.Impulse);
        else
            playerRigid.AddForce(knockBackImpulse, ForceMode2D.Impulse);
    }

    // 현재 접촉한 floor의 충돌을 일시적으로 비활성화 시킴. 아랫점프용.
    IEnumerator DeactiveFloor()
    {
        if (currentFloor != null)
        {
            TilemapCollider2D temp = currentFloor.gameObject.GetComponent<TilemapCollider2D>();
            temp.enabled = false;
            yield return new WaitForSeconds(0.3f);
            temp.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base") == true)
        {
            isOnGround = true;
            isDoubleJumpPossible = true;
        }
        if (collision.gameObject.CompareTag("Floor") == true)
        {
            isOnGround = true;
            isOnFloor = true;
            isDoubleJumpPossible = true;
            currentFloor = collision;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base") == true)
        {
            isOnGround = false;
        }
        if (collision.gameObject.CompareTag("Floor") == true)
        {
            isOnGround = false;
            isOnFloor = false;
            currentFloor = null;
        }
    }
}
