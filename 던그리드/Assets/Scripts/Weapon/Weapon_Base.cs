using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
    [SerializeField] protected int damage;
    public float Damage
    {
        get
        {
            return damage;
        }
    }

    Player_Base player;

    // 던전 입장때 일시적으로 플레이어, 무기의 움직임을 멈추기 위함
    bool isPause = false;

    public virtual void Awake()
    {
        player = GameManager.Instance.player;
        // playerImage 오브젝트 아래로 이동.
        transform.SetParent(player.ImageTransform);
        // 플레이어의 손 위치로 초기위치 설정
        transform.localPosition = new Vector3(0.9f, 0.35f, 0.0f);
    }
    
    protected virtual void Update()
    {
        if (player.CurrentHP == 0)
            return;

        if (isPause)
            return;

        UpdateInput();
    }

    // 물리연산과 동일한 주기로 회전을 해주지 않으면, Update와 FixedUpdate의 주기가 달라서 회전의 떨림이 일어난다.
    protected virtual void FixedUpdate()
    {
        if (player.CurrentHP == 0)
            return;

        if (isPause)
            return;

        UpdateRotation();
    }

    protected virtual void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            AttackKeyDown();
        if (Input.GetKeyUp(KeyCode.Mouse0))
            AttackKeyUp();
        if (Input.GetKey(KeyCode.Mouse0))
            AttackKey();
    }

    protected virtual void CheckPlayerDead()
    {
        if (GameManager.Instance.player.CurrentHP == 0)
            gameObject.SetActive(false);
    }

    // 마우스의 움직임에 따라 무기가 회전할 수 있도록.
    protected virtual void UpdateRotation()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 direction = (mouse - pos).normalized;

        // 좌측, 우측으로 반전되어 있는지에 따라서 방향 변경
        transform.right = player.ImageTransform.localScale.x * direction;
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
    }

    protected virtual void AttackKeyDown() { }
    protected virtual void AttackKeyUp() { }
    protected virtual void AttackKey() { }
}
