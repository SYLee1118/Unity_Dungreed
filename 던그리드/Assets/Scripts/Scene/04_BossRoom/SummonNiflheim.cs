using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonNiflheim : MonoBehaviour
{
    [SerializeField] FloatingFkeyController floatingFkey;
    [SerializeField] GameObject niflheim;
    [SerializeField] Animator anim;
    [SerializeField] SceneMain_BossRoom sceneMain;
    [SerializeField] SFXPool sfxPool;

    private void Start()
    {
        floatingFkey.keyPressedEvent += SummonNiflheimEvent; 
    }

    void SummonNiflheimEvent()
    {
        sfxPool.Play("Regen");
        floatingFkey.gameObject.SetActive(false);
        anim.SetTrigger("Summon");
        Invoke("Summon", 1.5f);
    }

    void Summon()
    {
        niflheim.SetActive(true);
        gameObject.SetActive(false);
        sceneMain.PlayBGM();
    }
}
