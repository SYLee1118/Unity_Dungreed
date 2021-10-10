using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMain_Intro : SceneMain_Base
{
    // 배경을 날아다니는 bird의 ObjectPool
    [SerializeField] ObjectPool birdPool;

    Coroutine generateBird;
    Coroutine changeScene;

    WaitForSeconds delay2 = new WaitForSeconds(2.0f);

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void Init()
    {
        base.Init();

        birdPool.DeactivateAll();
        // generateBird 코루틴이 2개 이상 실행되지 않도록 방지.
        if (generateBird != null)
            StopCoroutine(generateBird);
        generateBird = StartCoroutine(GenerateBird());
    }

    // 2초간격으로 배경을 날아다니는 bird 배치.
    IEnumerator GenerateBird()
    {
        while (true)
        {
            GameObject bird;
            bird = birdPool.GetObject();

            // 생성제한에 걸리지 않았으면 위치 초기화.
            if (bird != null)
                bird.GetComponent<IntroBirdBehaviour>().SetPosition();

            yield return delay2;
        }
    }

    public void UI_ButtonQuitMethod()
    {
        Application.Quit();
    }

    public void UI_ButtonOptionMethod()
    {
        GameManager.Instance.OpenSoundOption();
    }

    public void UI_ButtonGameStartMethod()
    {
        SceneController.Instance.LoadScene("Village");
    }
}
