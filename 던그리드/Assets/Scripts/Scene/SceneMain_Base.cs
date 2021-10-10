using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain_Base : MonoBehaviour
{
    [SerializeField] protected AudioSource BGM;

    protected  virtual void ChangeBGMVolume(float bgmVolume)
    {
        BGM.volume = bgmVolume;
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.volumeChangeEvent -= ChangeBGMVolume;
    }

    public virtual void Init()
    {
        BGM.Stop();
        BGM.volume = GameManager.Instance.gameData.bgmVolume;
        BGM.Play();
        GameManager.Instance.volumeChangeEvent += ChangeBGMVolume;
    }

    public virtual void StopBGM()
    {
        BGM.Stop();
    }

    public virtual void PlayBGM()
    {
        BGM.Play();
    }
}
