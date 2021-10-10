using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain_BossRoom : SceneMain_Base
{
    // 보스전 시작시 보스룸을 닫기위한 벽
    [SerializeField] GameObject wall;
    // 보스전 시작시 카메라이동범위를 재설정하기 위함.
    [SerializeField] CameraControl cameraControl;
    [Space]
    [SerializeField] AudioSource blizzardBGM;
    [SerializeField] ObjectPool snowPool;
    [SerializeField] Vector2 snowFallingSpeed;

    WaitForSeconds delay1 = new WaitForSeconds(1.0f);
    Coroutine snowCoroutine;

    public override void Init()
    {
        base.Init();

        blizzardBGM.Stop();
        blizzardBGM.volume = GameManager.Instance.gameData.bgmVolume;

        Player_Base player = GameManager.Instance.player;
        player.gameObject.SetActive(true);
        player.Resume();
        player.SetPosition(new Vector2(-5.0f, 0.0f));

        // 보스전 브금은 통로를 지나고 나서 재생되도록 우선은 정지.
        BGM.Stop();
    }

    public void PlayBGM()
    {
        BGM.Play();
    }

    protected override void ChangeBGMVolume(float bgmVolume)
    {
        base.ChangeBGMVolume(bgmVolume);
        blizzardBGM.volume = bgmVolume;
    }

    public override void StopBGM()
    {
        base.StopBGM();
        blizzardBGM.Stop();
    }

    public void ActivateBlizzard()
    {
        blizzardBGM.Play();
        snowCoroutine = StartCoroutine(CreateSnow());
        // 카메라 이동범위 재설정
        cameraControl.SetCameraRestraint(42f, 6f, 40f, 23f);
        wall.SetActive(true);
    }

    public void DeactivateBlizzard()
    {
        StopCoroutine(snowCoroutine);
        blizzardBGM.Stop();
    }

    IEnumerator CreateSnow()
    {
        while(true)
        {
            yield return delay1;
            StartCoroutine(SnowFall());
        }
    }

    IEnumerator SnowFall()
    {
        GameObject snow = snowPool.GetObject();
        // 초기 위치 랜덤설정
        snow.transform.position = new Vector3(Random.Range(38f, 55f), 16.5f, -4);
        while (true)
        {
            yield return null;
            // 눈을 천천히 하강시킴
            Vector3 currentPos = snow.transform.position;
            snow.transform.position = new Vector3(currentPos.x + snowFallingSpeed.x * Time.deltaTime,
                                                                            currentPos.y + snowFallingSpeed.y * Time.deltaTime, -4);
            // 일정 아래로 내려가면 비활성화
            if (snow.transform.position.y <= -15f)
            {
                snow.SetActive(false);
                break;
            }
        }
    }
}
