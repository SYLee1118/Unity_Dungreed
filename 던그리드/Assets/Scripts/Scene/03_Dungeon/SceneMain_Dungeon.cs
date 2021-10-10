using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain_Dungeon : SceneMain_Base
{
    public override void Init()
    {
        base.Init();

        Player_Base player = GameManager.Instance.player;
        player.gameObject.SetActive(true);
        player.Resume();
        player.SetPosition(new Vector2(-5.7f, -2.0f));
    }
}
