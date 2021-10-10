using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NiflheimBehaviour : EnemyBase
{
    [SerializeField] IceSpearBehaviour spear1;
    [SerializeField] IceSpearBehaviour spear2;
    [Space]
    [SerializeField] IcePillarBehaviour[] pillars;
    [SerializeField] GameObject stunFX;
    // 기둥들의 전체 회전용 Transform
    [SerializeField] Transform parentOfPillars;
    [SerializeField] SFXPool sfxPool;
    [Space]
    [SerializeField] GameObject niflheimUICanvas;
    [SerializeField] Image hpBarFillImage;
    [Space]
    [SerializeField] SceneMain_BossRoom sceneMain_BossRoom;

    Collider2D collider2D;

    enum AttackPattern : int
    {
        IceSpear = 0,
        BulletSpread = 1,
        BulletConcentration = 2
    }

    private void Start()
    {
        foreach(IcePillarBehaviour pillar in pillars)
            pillar.pillarDestroyedEvent += CheckStun;

        collider2D = GetComponent<BoxCollider2D>();
        // 4개의 기둥을 전부 부쉈을때만 데미지가 들어오도록.
        collider2D.enabled = false;

        StartCoroutine(SpinPillars());
        sceneMain_BossRoom.ActivateBlizzard();
    }

    private void Update()
    {
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        hpBarFillImage.fillAmount = (float)currentHP / (float)maxHP;
    }

    public void Attack()
    {
        // 세가지 공격패턴중 하나를 발동.
        AttackPattern pattern = (AttackPattern)Random.Range(0, 3);

        switch (pattern)
        {
            case AttackPattern.IceSpear:
                spear1.Aim();
                spear2.Aim();
                break;

            case AttackPattern.BulletSpread:
                foreach(IcePillarBehaviour pillar in pillars)
                {
                    if (pillar.gameObject.activeSelf == true)
                        pillar.Shot_Spread();
                }
                break;

            case AttackPattern.BulletConcentration:
                foreach (IcePillarBehaviour pillar in pillars)
                {
                    if (pillar.gameObject.activeSelf == true)
                        pillar.Shot_Concentration();
                }
                break;
        }
    }

    public void Reset()
    {
        // 터지는 이펙트가 끝나고, 초기 위치인 중앙으로 복귀.
        transform.position = new Vector3(42f, 7.5f, transform.position.z);
        enemySprite.color = new Color(1, 1, 1, 1);
        StopCoroutine(blinkCoroutine);

        // 4개의 얼음기둥 재생성
        foreach (IcePillarBehaviour pillar in pillars)
            pillar.Regen();
    }

    public void Regen()
    {
        sfxPool.Play("Regen");
    }

    public void Die()
    {
        sceneMain_BossRoom.DeactivateBlizzard();
        niflheimUICanvas.SetActive(false);
        sfxPool.Play("IcePillarDestroyed");
        stunFX.SetActive(false);
        enemySprite.color = new Color(1, 1, 1, 1);
        StopAllCoroutines();
        parentOfPillars.gameObject.SetActive(false);
        spear1.gameObject.SetActive(false);
        spear2.gameObject.SetActive(false);
        Invoke("ClearGame", 3.0f);
    }

    void ClearGame()
    {
        GameManager.Instance.GameOver_Clear();
    }

    // pillar가 부서질때마다 모든 pillar가 부서졌는지 체크, 다 부서졌다면 본체에 데미지를 줄 수 있는 stun상태에 돌입.
    private void CheckStun()
    {
        bool isAllPillarDown = true;

        foreach(IcePillarBehaviour pillar in pillars)
        {
            if (pillar.isDestroyed == false)
                isAllPillarDown = false;
        }

        if (isAllPillarDown == false)
            return;
        else
            StartCoroutine(Stun());
    }

    IEnumerator Stun()
    {
        // 스턴상태동안 데미지가 들어오도록 콜라이더를 enable
        collider2D.enabled = true;
        stunFX.SetActive(true);
        enemyAnimator.SetTrigger("Stun");
        yield return new WaitForSeconds(5.0f);

        // 5초후 터지는 이펙트와 함께, Regen
        collider2D.enabled = false;
        stunFX.SetActive(false);
        enemyAnimator.SetTrigger("Regen");
    }

    IEnumerator SpinPillars()
    {
        while (true)
        {
            yield return null;
            Vector3 rotation = parentOfPillars.localRotation.eulerAngles;
            // 초당 0.25회 회전 속도로 
            rotation.z += 90.0f * Time.deltaTime;
            if (rotation.z > 360.0f)
                rotation.z -= 360.0f;
            parentOfPillars.localRotation = Quaternion.Euler(rotation);
        }
    }
}
