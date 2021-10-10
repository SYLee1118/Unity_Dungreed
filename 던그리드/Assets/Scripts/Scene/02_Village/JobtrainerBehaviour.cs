using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobtrainerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject floatingFKey;
    [SerializeField] private GameObject playerChangeUI;

    // 플레이어가 범위 내에 들어와있는지 체크
    private bool isPlayerContact = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(isPlayerContact)
                OpenPlayerChangeUI();
        }
    }

    private void OpenPlayerChangeUI()
    {
        playerChangeUI.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            floatingFKey.SetActive(true);
            isPlayerContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            floatingFKey.SetActive(false);
            isPlayerContact = false;
        }
    }
}
