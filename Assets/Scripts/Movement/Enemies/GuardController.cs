using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    //--------------------------------------------------
    // - Variable setup -
    //-------
    public  float        accel;            // Object acceleration rate
    public  float        gravity;          // Object fall rate
    public  float        terminalV;        // Max falling speed
    public  float        topSpeed;         // Maximum speed of the object
    
    private float        currentSpeed;     // Speed the object is running at
    private float        currentFall;      // Speed the object is falling at
    
    // Hit checks -------
    private bool         landing;          // Is it hitting a floor
    private int          hitting;          // -1/0/1 | Is it hitting a wall
    private int          direction;        // -1/+1  | Which way is he facing
    private int          alertLevel;       // The higher this gets, the more thorough they are
    private Vector3      targetPosition;   // Position of player, if he hears it

    // Storing externals -------
    private bool                alerted;   // Has another guard tried to contact this one
    private Timer               timer;     // Just a timer for how long to stand still when alert
    private GameObject[]        walls;     // Store all the walls here
    private OpponentState       state;     // Script for the current state of the opponent
    private ForesightController foresight; // See the future, or at least the ground ahead of you
    private Hearing             hearing;   // To drop to lower platforms
    private GameObject[]        allies;    // To alert higher guards

    // Start is called before the first frame update
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        allies = GameObject.FindGameObjectsWithTag("Guard");
        foresight = GetComponentInChildren<ForesightController>();
        state = GetComponent<OpponentState>();
        hearing = GetComponent<Hearing>();
        direction = 1;
        alertLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------------------------
        // - Movement -
        //-------
        currentSpeed += accel;
        currentFall -= gravity;
        landing = false;
        hitting = 0;

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        if (currentFall < terminalV)
            currentFall = terminalV;

        transform.Translate(currentSpeed * Time.deltaTime, 0.0f, 0.0f);
        transform.Translate(0.0f, currentFall * Time.deltaTime, 0.0f, Space.World);

        for (int i = 0; i < walls.Length; i++) // Basic collision detection, behind this point, everything thinks it's free to move anywhere
        {
            CollisionCheck(foresight.transform.position, walls[i].transform.position,
                           transform.lossyScale / 2, walls[i].transform.lossyScale / 2, foresight.range, ref foresight.landing);

            CollisionCheck(transform.position, walls[i].transform.position, transform.lossyScale / 2,
                               walls[i].transform.lossyScale / 2, 0.0f, ref landing);
        }

        if (landing)
        {
            if (hitting != 0 || (!foresight.landing && !foresight.greenShell))
            {
                TurnAround();
            }
        }
        else
            foresight.greenShell = false;

        if (state.currentState == 4)
        {
            if (gameObject.GetComponent<Timer>() == null)
            {
                gameObject.AddComponent<Timer>();
                timer = GetComponent<Timer>();
                timer.SetLimit(2.0f);
            }
            else
            {
                if (timer.timeUp)
                {
                    if (alertLevel < 2)
                        alertLevel++;
                    // Finding higher up allies to contact
                    for (int i = 0; i < allies.Length; i++)
                    {
                        if (transform.position.y < targetPosition.y &&
                            allies[i].transform.position.y > transform.position.y)
                        {
                            GuardController allyController = allies[i].GetComponent<GuardController>();
                            allyController.SetTargetPos(targetPosition);
                            allyController.Contact();
                        }
                    }

                    switch (alertLevel)
                    {
                        case 1:
                            state.SetState(3);
                            break;
                        case 2:
                            state.SetState(3);
                            if (hearing.GetYDist() > 0)
                                foresight.greenShell = true;
                            break;
                        case 3:
                            break;
                    }
                    Destroy(gameObject.GetComponent<Timer>());
                }
            }
        }
    }

    public void TurnAround(int mode = 0)
    {
        if (mode == 0)
        {
            if (state.currentState < 4)
            {
                transform.Rotate(0.0f, 180.0f, 0.0f);
                direction = -direction;
            }
            else if (state.currentState == 5)
            {
                state.SetState(4);
            }
        }
        else if (mode == 1)
        {
            transform.Rotate(0.0f, 180.0f, 0.0f);
            direction = -direction;
            state.SetState(5);
        }
    }

    public void Contact()
    {
        for (int i = 0; i < allies.Length; i++)
        { // Check if a guard is below you who is above the noise
            GuardController allyController = allies[i].GetComponent<GuardController>();
            if (allyController.transform.position.y < transform.position.y)
            {
                return;
            }
            else
            {
                state.SetState(4);
                hearing.FaceTheSound();
            }
        }
    }

    public void CollisionCheck(Vector2 playerPos, Vector2 obstaclePos, Vector2 playerScale, Vector2 obstacleScale, float range, ref bool grounded)
    {
        if (playerPos.y >= obstaclePos.y - (obstacleScale.y + playerScale.y) && playerPos.y <= obstaclePos.y + (obstacleScale.y + playerScale.y) &&
            playerPos.x >= obstaclePos.x - (obstacleScale.x + playerScale.y) && playerPos.x <= obstaclePos.x + (obstacleScale.x + playerScale.y))
        {
            if (playerPos.y > obstaclePos.y + obstacleScale.y)
            { // Ground
                transform.position = new Vector3(transform.position.x, obstaclePos.y + (obstacleScale.y + playerScale.y), 0.0f);
                grounded = true;
                currentFall = 0;
                return;
            }
            else if (playerPos.y < obstaclePos.y - obstacleScale.y)
            { // Ceiling
                transform.position = new Vector3(transform.position.x, obstaclePos.y - (obstacleScale.y + playerScale.y), 0.0f);
                currentFall = 0;
                return;
            }

            if (playerPos.x > obstaclePos.x)
            { // Left wall
                transform.position = new Vector3(obstaclePos.x + (obstacleScale.x + playerScale.x) + range, transform.position.y, 0.0f);
                hitting = -1;
            }
            else if (playerPos.x < obstaclePos.x)
            { // Right wall
                transform.position = new Vector3(obstaclePos.x - (obstacleScale.x + playerScale.x) - range, transform.position.y, 0.0f);
                hitting = 1;
            }
        }
    }

    public int GetDirection() { return direction; }
    public void SetTargetPos(Vector3 pos) { targetPosition = pos; }
}
