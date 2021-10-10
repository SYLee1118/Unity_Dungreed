using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance { get { return instance; } }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("씬 컨트롤러 싱글톤 에러!");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public SceneMain_Base currentSceneMain;

    [SerializeField] GameObject loadingCanvas;
    [SerializeField] CanvasGroup loadingCanvasGroup;
    [SerializeField] Image progressBar;

    private void Start()
    {
        currentSceneMain.Init();    // 제일 최초의 씬인 Intro씬의 init()
    }

    public void LoadScene(string sceneName)
    {
        loadingCanvas.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LoadSceneProcess(sceneName));
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(Fade(false));
        currentSceneMain = GameObject.FindObjectOfType<SceneMain_Base>();
        currentSceneMain.Init();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator LoadSceneProcess(string sceneName)
    {
        progressBar.fillAmount = 0.0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            // 씬 로딩이 90% 이상 이뤄지면 fake loading으로 1초간 딜레이를 준다.
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);

                if (progressBar.fillAmount >= 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    // 로딩 캔버스 페이드인용 코루틴
    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3.0f;
            loadingCanvasGroup.alpha = isFadeIn ? Mathf.Lerp(0.0f, 1.0f, timer) : Mathf.Lerp(1.0f, 0.0f, timer);
        }
        if (isFadeIn == false)
            loadingCanvas.SetActive(false);
    }
}
