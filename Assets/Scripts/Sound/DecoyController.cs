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
    private Transform player;
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        active = true;
        gravity = 100.0f;
        resetGravity = gravity;
        used = false;
        accel = 10.0f;
        noiseMaker = GetComponent<NoiseMaker>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        timer = GetComponent<Timer>();
        player = GameObject.Find("Runner").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!timer.timeUp)
        { // The decoy can't fire off forever, it should just explode after a bit
            if (speed.y < 0.0f && gravity < resetGravity * 1.01f)
                gravity *= 1.01f; // It should fall with weight, just like the player
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
        { // For when you want a noisemaker limit
            noiseMaker.MakeNoise((speed.x + speed.y) / 2);
            used = true;
        }
        if (CollisionCheck() && used)
        { // It's stopped making noise, destroy it
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

    private bool CollisionCheck()
    {
        if (transform.position.y >= player.position.y - (player.lossyScale.y + transform.lossyScale.y) && transform.position.y <= player.position.y + (player.lossyScale.y + transform.lossyScale.y) &&
            transform.position.x >= player.position.x - (player.lossyScale.x + transform.lossyScale.y) && transform.position.x <= player.position.x + (player.lossyScale.x + transform.lossyScale.y))
        {
            return true;
        }
        return false;
    }
}
