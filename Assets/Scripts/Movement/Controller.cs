using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //--------------------------------------------------
    // - Variable setup -
    //-------
    public  float        accel;          // Acceleration rate
    public  float        launchPwr;      // Jumping power
    public  float        gravity;        // Object fall rate
    public  float        cling;          // Object fall rate when hugging a wall
    public  float        topSpeed;       // Max speed
    public  float        terminalV;      // Max falling speed
    
    [HideInInspector]
    public  float        currentSpeed;   // Speed the object is running at
    private float        currentFall;    // Speed the object is falling at
    private float        resetGravity;
    private bool         gripSet;        // What surface are you hitting
    
    // Hit checks -------
    [HideInInspector]
    public  bool         landing;        // Is it hitting a floor
    private int          currentFloor;   // Which floor is it standing on
    private int          hitting;        // -1/0/1 | Is it hitting a wall
    private GameObject[] walls;          // Store all the walls here
    private WallSetup[]  wallProperties;
    private GameObject   boxCollider;
    private NoiseMaker   sfx;
    private Vector3      oldPos;
    private Camera       camera;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.gameObject;
        boxCollider.AddComponent<BoxCollider2D>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        wallProperties = FindObjectsOfType<WallSetup>();
        sfx = GetComponent<NoiseMaker>();
        camera = GetComponentInChildren<Camera>();
        accel = 10.0f;
        resetGravity = gravity;
        gripSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------------------------
        // - Movement -
        //-------
        if (currentFall < 0.0f && gravity < resetGravity * 1.01f)
            gravity *= 1.01f;
        else if (Input.GetKey(KeyCode.W) && gravity > resetGravity * 0.4f)
        {
            gravity *= 0.95f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (camera.orthographicSize < 20.0f)
                camera.orthographicSize += accel * Time.deltaTime;
            else if (camera.orthographicSize > 20.0f)
                camera.orthographicSize = 20.0f;
        }
        else
        {
            if (camera.orthographicSize > 10.0f)
                camera.orthographicSize -= accel * Time.deltaTime;
            else if (camera.orthographicSize < 10.0f)
                camera.orthographicSize = 10.0f;
        }

        currentFall -= gravity * Time.deltaTime;
        landing = false;
        hitting = 0;

        if (hitting > -1)
        { // Move left
            if (Input.GetKey(KeyCode.A))
                currentSpeed -= accel * Time.deltaTime;
            else currentSpeed += accel * Time.deltaTime;
        }

        if (hitting < 1)
        { // Move right
            if (Input.GetKey(KeyCode.D))
                currentSpeed += accel * Time.deltaTime;
            else currentSpeed -= accel * Time.deltaTime;
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        { // Deceleration
            if (currentSpeed > 0)
                currentSpeed -= accel * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += accel * Time.deltaTime;
            if (currentSpeed < 0.2f && currentSpeed > -0.2f) // Correcting floating point inaccuracy near 0
                currentSpeed = 0;
        }

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        if (currentSpeed < -topSpeed)
            currentSpeed = -topSpeed;
        if (currentFall < terminalV)
            currentFall = terminalV;

        oldPos = transform.position;
        transform.Translate(currentSpeed * Time.deltaTime, currentFall * Time.deltaTime, 0.0f);

        gripSet = false;
        for (int i = 0; i < walls.Length; i++) // Basic collision detection, behind this point, everything thinks it's free to move anywhere
        {
            if (landing && hitting != 0)
                break;

            if (!gripSet)
            {
                accel = 10.0f;
                cling = 0.05f;
            }
            CollisionCheck(transform.position, walls[i].transform.position, transform.lossyScale / 2, walls[i].transform.lossyScale / 2, i, oldPos);
        }

        if (hitting != 0)
        { // Wall jump mechanics
            currentFall += cling;
            if (Input.GetKey(KeyCode.W))
            {
                currentFall += launchPwr / 2;
                if (hitting == -1)
                    currentSpeed = topSpeed;
                else
                    currentSpeed = -topSpeed;

                if (sfx.enabled)
                {
                    sfx.MakeNoise(10.0f);
                    sfx.CallSound(1, hitting);
                }
            }
        }

        if (Input.GetKey(KeyCode.W) && landing)
        {
            currentFall += launchPwr;
            if (sfx.enabled)
            {
                sfx.MakeNoise(10.0f);
                sfx.CallSound(0);
            }
        }

        if (landing)
            gravity = resetGravity;
    }

    private void CollisionCheck(Vector2 playerPos, Vector2 obstaclePos, Vector2 playerScale, Vector2 obstacleScale, int i, Vector3 oldPos)
    {
        if (playerPos.y >= obstaclePos.y - (obstacleScale.y + playerScale.y) && playerPos.y <= obstaclePos.y + (obstacleScale.y + playerScale.y) &&
            playerPos.x >= obstaclePos.x - (obstacleScale.x + playerScale.y) && playerPos.x <= obstaclePos.x + (obstacleScale.x + playerScale.y))
        {
            if (oldPos.y > obstaclePos.y + obstacleScale.y)
            { // Ground
                transform.position = new Vector3(playerPos.x, obstaclePos.y + (obstacleScale.y + playerScale.y), 0.0f);
                landing = true;
                currentFall = 0;
                wallProperties[i].SetVariables(ref accel, ref cling);
                gripSet = true;
                return;
            }
            else if (oldPos.y < obstaclePos.y - obstacleScale.y)
            { // Ceiling
                transform.position = new Vector3(playerPos.x, obstaclePos.y - (obstacleScale.y + playerScale.y), 0.0f);
                currentFall = 0;
                wallProperties[i].SetVariables(ref accel, ref cling);
                gripSet = true;
                return;
            }

            if (oldPos.x > obstaclePos.x + obstacleScale.x && hitting != 1)
            { // Left wall
                transform.position = new Vector3(obstaclePos.x + (obstacleScale.x + playerScale.y), playerPos.y, 0.0f);
                hitting = -1;
                currentSpeed = 0;
            }
            else if (oldPos.x < obstaclePos.x - obstacleScale.x && hitting != -1)
            { // Right wall
                transform.position = new Vector3(obstaclePos.x - (obstacleScale.x + playerScale.y), playerPos.y, 0.0f);
                hitting = 1;
                currentSpeed = 0;
            }
            wallProperties[i].SetVariables(ref accel, ref cling);
            gripSet = true;
        }
    }
}
