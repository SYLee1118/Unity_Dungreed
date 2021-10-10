using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudControl : MonoBehaviour
{
    public Transform transform_Cloud1;
    public Transform transform_Cloud2;
    public float speed;
    public float xSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform_Cloud1.position = new Vector2(transform_Cloud1.position.x - speed * Time.deltaTime, 0);
        transform_Cloud2.position = new Vector2(transform_Cloud2.position.x - speed * Time.deltaTime, 0);

        if (transform_Cloud1.position.x <= -xSize)
            transform_Cloud1.position = new Vector2(transform_Cloud2.position.x + xSize, 0);

        if (transform_Cloud2.position.x <= -xSize)
            transform_Cloud2.position = new Vector2(transform_Cloud1.position.x + xSize, 0);
    }
}
