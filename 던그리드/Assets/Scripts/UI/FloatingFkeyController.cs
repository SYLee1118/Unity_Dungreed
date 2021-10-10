using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingFkeyController : MonoBehaviour
{
    [SerializeField] GameObject spriteObject;
    // 이벤트의 중복실행등을 방지 + 1회성 이벤트에서의 활용을 위해.
    public bool isEvenetEnabled;

    public delegate void FkeyDownEvent();
    public event FkeyDownEvent keyPressedEvent;

    bool isPlayerDetected;

    private void Start()
    {
        isPlayerDetected = false;
        spriteObject.SetActive(false);
    }

    private void Update()
    {
        // 플레이어가 탐지 범위 내에 들어왔고, isEventEnabled == true일 때, F키가 눌리면 미리 설정된 이벤트를 실행.
        if(Input.GetKeyDown(KeyCode.F) && isPlayerDetected && isEvenetEnabled)
        {
            if (keyPressedEvent != null)
                keyPressedEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            spriteObject.SetActive(true);
            isPlayerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            spriteObject.SetActive(false);
            isPlayerDetected = false;
        }
    }
}
