using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain_Village : SceneMain_Base
{
    public override void Init()
    {
        base.Init();
        GameManager.Instance.LoadPlayer();
        GameManager.Instance.player.SetPosition(new Vector2(11.0f, 0.0f));
        GameManager.Instance.player.gameObject.SetActive(true);
        GameManager.Instance.weapon.gameObject.SetActive(true);
    }
}
