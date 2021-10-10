using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraControl))]
class DrawCameraMoveRestraintArea : Editor
{
    private void OnSceneGUI()
    {
        CameraControl CC = target as CameraControl;

        Vector2 leftTop = new Vector2(CC.OffsetX - CC.SizeX / 2, CC.OffsetY + CC.SizeY / 2);
        Vector2 rightTop = new Vector2(CC.OffsetX + CC.SizeX / 2, CC.OffsetY + CC.SizeY / 2);
        Vector2 leftBot = new Vector2(CC.OffsetX - CC.SizeX / 2, CC.OffsetY - CC.SizeY / 2);
        Vector2 rightBot = new Vector2(CC.OffsetX + CC.SizeX / 2, CC.OffsetY - CC.SizeY / 2);

        Handles.color = Color.yellow;

        Handles.DrawLine(leftTop, rightTop);
        Handles.DrawLine(rightTop, rightBot);
        Handles.DrawLine(rightBot, leftBot);
        Handles.DrawLine(leftBot, leftTop);
    }
}