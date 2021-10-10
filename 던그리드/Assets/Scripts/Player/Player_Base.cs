using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Base : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer playerSprite;
    [SerializeField] protected Transform imageTransform;   
    [SerializeField] protected Animator playerAnimator;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected SFXPool sfxPool;
    [Space]
    [SerializeField] protected GameObject playerUICanvas;
    [SerializeField] protected Image hpBarFill;
    [Space]
    [SerializeField] protected int maxHP;  
    [SerializeField] protected int currentHP;
    [SerializeField] protected int attackDamage;

    public Transform ImageTransform { get { return imageTransform; } }
    public int MaxHP { get { return maxHP; } }
    public int CurrentHP { get { return currentHP; } }
    public int AttackDamage { get { return attackDamage; } }

    protected bool isHitable;
    protected WaitForSeconds delay01;

    virtual protected void Start()
    {
        isHitable = true;
        delay01 = new WaitForSeconds(0.1f);
        SetHPBar();
    }

    virtual protected void Update()
    {
        UpdateAnimator();
    }

    // 애니메이터에 플레이어 HP 파라미터 전달 (DIE 체크를 위해)
    protected void UpdateAnimator()
    {
        playerAnimator.SetInteger("PlayerHP", currentHP);
    }

    // 데미지를 입은 후, 플레이어 이미지가 깜박이도록 + 2초간 새로 피격받지 않도록 카운트.
    protected IEnumerator GetDamageCooldown()
    {
        isHitable = false;
        int count = 0;
        
        // 2초간 캐릭터 이미지 깜박임.
        while(count < 10)
        {
            if (count++ % 2 == 0)
                playerSprite.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            else
                playerSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return delay01;
        }
        // 2초 후 다시 데미지를 입을 수 있는 상태로.
        isHitable = true;
    }

    public void SetPosition(Vector2 pos)
    {
        this.transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    public void GetDamage(int damage, Transform attackerTransform)
    {
        if (isHitable == false || CurrentHP == 0)
            return;

        playerController.KnockBack(attackerTransform);
        sfxPool.Play("Hit");
        if (damage >= CurrentHP)
        {
            currentHP = 0;
            GameManager.Instance.GameOver_Fail();
        }
        else
        {
            isHitable = false;
            currentHP -= damage;
            StartCoroutine(GetDamageCooldown());
        }
        SetHPBar();
    }

    public void SetHPBar()
    {
        hpBarFill.fillAmount = (float)currentHP / (float)maxHP;
    }

    public void Pause()
    {
        playerController.Pause();
        GameManager.Instance.weapon.Pause();
    }

    public void Resume()
    {
        playerController.Resume();
        GameManager.Instance.weapon.Resume();
    }
}
