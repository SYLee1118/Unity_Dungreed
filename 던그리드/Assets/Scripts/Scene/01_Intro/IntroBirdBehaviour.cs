using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBirdBehaviour : MonoBehaviour
{
    float speed = 1.0f;

    // 좌측 or 우측 랜덤한 위치로 시작위치 설정.
    public void SetPosition()
    {
        if (Random.Range(0, 2) == 0)
        {
            transform.position = new Vector3(-9.5f, Random.Range(-4.0f, 4.0f));
            transform.localScale = new Vector3(6, 6, 6);
            speed = 1.0f;
        }
        else
        {
            transform.position = new Vector3(9.5f, Random.Range(-4.0f, 4.0f));
            transform.localScale = new Vector3(-6, 6, 6);
            speed = -1.0f;
        }
    }

    void Update()
    {
        UpdatePosition();
        CheckFinish();
    }

    // speed만큼 이동
    void UpdatePosition()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }

    // 화면 밖으로 벗어났는지 체크
    void CheckFinish()
    {
        if (transform.position.x < -10.0f || 10.0f < transform.position.x)
        {
            gameObject.SetActive(false);
        }
    }
}
