using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    public bool overlap = false;    // 동시재생지원여부
    private AudioSource audioSource;

    public void SetAudioSource(AudioSource source)
    {
        audioSource = source;
        audioSource.clip = audioClip;
        source.playOnAwake = false;
    }

    public void Play()
    {
        if(overlap == true)
        {
            audioSource.volume = GameManager.Instance.gameData.sfxVolume;
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            audioSource.volume = GameManager.Instance.gameData.sfxVolume;
            audioSource.Play();
        }
    }
}

public class SFXPool : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    void Awake()
    {
        for(int i = 0; i< sounds.Length; i++)
        {
            GameObject go = new GameObject("SFX_" + sounds[i].name);
            go.transform.SetParent(this.transform);
            AudioSource _audioSource = go.AddComponent<AudioSource>();
            sounds[i].SetAudioSource(_audioSource);
        }
    }

    public void Play(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }
        Debug.LogWarning(name + "는 SFXPool에 없는 사운드.");
    }
}
