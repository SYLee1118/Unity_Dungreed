using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSoundControl : MonoBehaviour
{
    [SerializeField] private SFXPool sfxPool;
    private int size;
    private int cnt;

    private void Awake()
    {
        size = 4;
        cnt = 0;
    }

    private void PlayStepSound()
    {
        sfxPool.Play("Step" + cnt.ToString());
        cnt++;
        if (cnt >= size)
            cnt = 0;
    }
}
