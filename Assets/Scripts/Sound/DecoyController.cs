using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyController : MonoBehaviour
{
    public Vector2 speed;
    private bool landing;
    private bool used;
    private float accel;
    private float gravity;
    private float resetGravity;
    private NoiseMaker noiseMaker;
    private GameObject[] walls;
    private Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        gravity = 100.0f;
        resetGravity = gravity;
        used = false;
        accel = 10.0f;
        noiseMaker = GetComponent<NoiseMaker>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        timer = GetComponent<Timer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!timer.timeUp)
        {
            if (speed.y < 0.0f && gravity < resetGravity * 1.01f)
                gravity *= 1.01f;
            else if (gravity > resetGravity * 0.4f)
                gravity *= 0.95f;

            if (speed.x > 0.0f)
                speed.x -= accel * Time.deltaTime;
            else if (speed.x < 0.0f)
                speed.x += accel * Time.deltaTime;

            speed.y -= gravity * Time.deltaTime;

            transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f, Space.World);
            for (int i = 0; i < walls.Length; i++)
                CollisionCheck(transform.position, walls[i].transform.position, transform.lossyScale / 2, walls[i].transform.lossyScale / 2);
        }

        if (timer.timeUp && !used)
        {
            noiseMaker.MakeNoise((speed.x + speed.y) / 2);
            used = true;
        }
        if (noiseMaker.soundRadius <= 0.0f && used)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetSpeed(Vector2 playerSpeed)
    {
        speed = playerSpeed;
    }

    private void CollisionCheck(Vector2 playerPos, Vector2 obstaclePos, Vector2 playerScale, Vector2 obstacleScale)
    {
        if (playerPos.y >= obstaclePos.y - (obstacleScale.y + playerScale.y) && playerPos.y <= obstaclePos.y + (obstacleScale.y + playerScale.y) &&
            playerPos.x >= obstaclePos.x - (obstacleScale.x + playerScale.y) && playerPos.x <= obstaclePos.x + (obstacleScale.x + playerScale.y))
        {
            timer.timeUp = true;
        }
    }
}
