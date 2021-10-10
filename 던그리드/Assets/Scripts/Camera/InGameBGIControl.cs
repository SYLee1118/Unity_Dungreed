using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBGIControl : MonoBehaviour
{
    [Tooltip("상대적으로 느리게 움직이는 BG Layer")]
    public Transform bgTransform1;
    [Tooltip("상대적으로 빠르게 움직이는 BG Layer")]
    public Transform bgTransform2;

    // 산, 나무 배경 이미지가 서로 다른 비율로 이동하게 하여 
    // 입체감이 들도록 컨트롤.
    void SetBGIsPosition()
    {
        Vector3 position = transform.position;
        bgTransform1.localPosition = new Vector3(position.x * -0.1f, (position.y + 5) * -0.1f);
        bgTransform2.localPosition = new Vector3(position.x * -0.2f, (position.y + 5) * -0.2f);
    }

    void Start()
    {
        SetBGIsPosition();
    }

    void Update()
    {
        SetBGIsPosition();
    }
}
