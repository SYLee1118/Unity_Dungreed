using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("카메라 이동영역 설정")]
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float sizeX;
    [SerializeField] private float sizeY;

    public float OffsetX { get { return offsetX; } }
    public float OffsetY { get { return offsetY; } }
    public float SizeX { get { return sizeX; } }
    public float SizeY { get { return sizeY; } }

    Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    public void SetCameraRestraint(float offsetX, float offsetY, float sizeX, float sizeY)
    {
        this.offsetX = offsetX;
        this.offsetY = offsetY;
        this.sizeX = sizeX;
        this.sizeY = sizeY;
    }

    void FollowPlayer()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;

        float newX = playerPos.x + 0.5f;
        float newY = playerPos.y;

        float cameraHeight = camera.orthographicSize * 2.0f;
        float cameraWidth = cameraHeight * camera.aspect;

        // 카메라 이동제한
        if (newX + cameraWidth / 2 > offsetX + sizeX / 2)
            newX = offsetX + sizeX / 2 - cameraWidth / 2;
        if (newX - cameraWidth / 2 < offsetX - sizeX / 2)
            newX = offsetX - sizeX / 2 + cameraWidth / 2;

        if (newY + cameraHeight / 2 > offsetY + sizeY / 2)
            newY = offsetY + sizeY / 2 - cameraHeight / 2;
        if (newY - cameraHeight / 2 < offsetY - sizeY / 2)
            newY = offsetY - sizeY / 2 + cameraHeight / 2;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void Update()
    {
        FollowPlayer();
    }
}
