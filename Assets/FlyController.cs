using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    private Vector2 speed;
    private Vector2 accel;
    private Vector2Int direction;
    // Start is called before the first frame update
    void Start()
    {
        accel.x = 0.25f;
        accel.y = 0.5f;
        speed.x = 1.0f;
        speed.y = 1.0f;
        direction.x = 1;
        direction.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.x > 0)
            speed.x += accel.x * Time.deltaTime;
        else if (direction.x < 0)
            speed.x -= accel.x * Time.deltaTime;
        if (direction.y > 0)
            speed.y += accel.y * Time.deltaTime;
        else if (direction.y < 0)
            speed.y -= accel.y * Time.deltaTime;

        if (speed.x > 1.0f || speed.x < -1.0f)
            direction.x = -direction.x;
        if (speed.y > 1.0f || speed.y < -1.0f)
            direction.y = -direction.y;

        transform.Translate(speed * Time.deltaTime);
    }
}
