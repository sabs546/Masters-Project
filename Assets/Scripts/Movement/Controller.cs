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
    [HideInInspector]
    public  float        currentFall;    // Speed the object is falling at
    private float        resetGravity;
    private bool         gripSet;        // What surface are you hitting
    
    // Hit checks -------
    [HideInInspector]
    public  bool         landing;        // Is it hitting a floor
    [HideInInspector]
    public  int          hitting;        // -1/0/1 | Is it hitting a wall
    public  int          ammo;           // How many decoys can you store
    public  GameObject   decoy;          // Stores the decoy object you can throw out

    private int          currentFloor;   // Which floor is it standing on
    private GameObject[] walls;          // Store all the walls here
    private WallSetup[]  wallProperties;
    private GameObject   boxCollider;
    private NoiseMaker   sfx;
    private Vector3      oldPos;
    private CameraFollow camera;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.gameObject;
        boxCollider.AddComponent<BoxCollider2D>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        wallProperties = FindObjectsOfType<WallSetup>();
        sfx = GetComponent<NoiseMaker>();
        accel = 10.0f;
        resetGravity = gravity;
        gripSet = false;
        camera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //--------------------------------------------------
        // - Movement -
        //-------
        if (camera.begin)
        {
            if (currentFall < 0.0f && gravity < resetGravity * 1.01f)
                gravity *= 1.01f;
            else if (Input.GetKey(KeyCode.W) && gravity > resetGravity * 0.4f)
            {
                gravity *= 0.95f;
            }

            currentFall -= gravity * Time.deltaTime;
            bool crashLand = landing;
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
                float tempFall = currentFall;
                CollisionCheck(transform.position, walls[i].transform.position, transform.lossyScale / 2, walls[i].transform.lossyScale / 2, i, oldPos);
                if (!crashLand && landing)
                {
                    sfx.MakeNoise(tempFall / 5);
                    AudioSource sound = gameObject.transform.Find("Landing sound").GetComponent<AudioSource>();
                    if (tempFall < -50)
                        sound.pitch = 1.0f;
                    else
                        sound.pitch = 2.0f;

                    sound.volume = -tempFall / 200;
                    sound.Play();
                    crashLand = true;
                }
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
                if (Mathf.Abs(currentSpeed) >= topSpeed)
                    launchPwr *= 1.25f;

                currentFall += launchPwr;

                if (Mathf.Abs(currentSpeed) >= topSpeed)
                    launchPwr /= 1.25f;

                if (sfx.enabled)
                {
                    sfx.MakeNoise(10.0f);
                    sfx.CallSound(0);
                }
            }

            if (landing)
            {
                gravity = resetGravity;
                if (Mathf.Abs(currentSpeed) > 2.0f)
                    GetComponentInChildren<StepTimer>().MakeNoise(2.0f / Mathf.Abs(currentSpeed));
                else
                    GetComponentInChildren<StepTimer>().StopMoving();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Instantiate(decoy, transform);
                decoy.GetComponent<NoiseMaker>().MakeNoise(20.0f);
            }
        }
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
