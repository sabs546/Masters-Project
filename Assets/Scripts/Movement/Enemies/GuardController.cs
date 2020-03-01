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
    public  Canvas       suspicionText;    // Text shown when they hear something
    public  Canvas       contactingText;   // Text shown when they talk to someone else
    public  Canvas       contactedText;    // Text shown when someone tells them something
    
    private float        currentSpeed;     // Speed the object is running at
    private float        currentFall;      // Speed the object is falling at
    
    // Hit checks -------
    private bool         landing;          // Is it hitting a floor
    private int          hitting;          // -1/0/1 | Is it hitting a wall
    private int          direction;        // -1/+1  | Which way is he facing
    public  int          alertLevel;       // The higher this gets, the more thorough they are
    private Vector3      targetPosition;   // Position of player, if he hears it

    // Storing externals -------
    private bool                alerted;   // Has another guard tried to contact this one
    private Timer               timer;     // Just a timer for how long to stand still when alert
    private GameObject[]        walls;     // Store all the walls here
    private OpponentState       state;     // Script for the current state of the opponent
    private ForesightController foresight; // See the future, or at least the ground ahead of you
    private Hearing             hearing;   // To drop to lower platforms
    private GameObject[]        allies;    // To alert higher guards
    public  Transform           cov;       // Visual vision cone

    // Start is called before the first frame update
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        allies = GameObject.FindGameObjectsWithTag("Guard");
        foresight = GetComponentInChildren<ForesightController>();
        state = GetComponent<OpponentState>();
        hearing = GetComponent<Hearing>();
        timer = GetComponent<Timer>();
        direction = 1;
        alertLevel = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //--------------------------------------------------
        // - Movement -
        //-------
        currentSpeed += accel * Time.deltaTime;
        currentFall -= gravity * Time.deltaTime;
        landing = false;
        hitting = 0;

        if (currentSpeed > topSpeed)
            currentSpeed = topSpeed;
        if (currentFall < terminalV)
            currentFall = terminalV;
        
        transform.Translate(currentSpeed * Time.deltaTime, 0.0f, 0.0f);
        transform.Translate(0.0f, currentFall * Time.deltaTime, 0.0f, Space.World);

        for (int i = 0; i < walls.Length; i++)
        { // Basic collision detection, behind this point, everything thinks it's free to move anywhere
            if (landing && hitting != 0)
                break;

            CollisionCheck(foresight.transform.position, walls[i].transform.position,
                           transform.lossyScale / 2, walls[i].transform.lossyScale / 2, foresight.range, ref foresight.landing);

            CollisionCheck(transform.position, walls[i].transform.position, transform.lossyScale / 2,
                               walls[i].transform.lossyScale / 2, 0.0f, ref landing);
        }

        if (timer.timeUp)
        {
            if (hearing.heard)
            { // To stop multi-state additions, only check for new sounds after 2 seconds has passed
                timer.SetLimit(2.0f); // Start a timer for the next loop
                hearing.FaceTheSound(); // Look at the sound
                state.SetState(4); // Think about it
                alertLevel++; // Be more alert because of it
            }
            else if (state.currentState == 4)
            { // This should be after they hear a sound, the stun should be over, they will approach the sound at this point
                if (alertLevel == 1 || alertLevel == 3)
                { // They aren't very worried, it's just a noise, just patrol a bit
                    state.SetState(3);
                }

                if (alertLevel == 2)
                { // This is suspicious, run to the noise
                    if (hearing.GetYDist() >= 0) // He's below you
                        state.SetState(5);
                    else
                    { // He's above you
                        state.SetState(3);
                        ContactChain();
                    }
                }
            }
        }

        if (state.currentState == 5)
        { // If the enemy is in chase mode
            if (!foresight.landing)
            { // If they find a fall
                state.SetState(4); // Peer over the edge
                timer.SetLimit(2.0f); // Peer over for 2 seconds
                foresight.greenShell = true; // Afterwards, go to state 3 to walk off
                alertLevel = 3; // Special alertLevel for walking off cliffs
            }
        }
        else if (state.currentState == 3)
        { // If the enemy is walking off a cliff, don't walk off of any more cliffs
            if (foresight.greenShell && !landing)
                foresight.greenShell = false;
            else if (!foresight.greenShell && landing && alertLevel == 3)
            {
                hearing.FaceTheSound();
                alertLevel = 1;
            }
        }

        if (landing)
        { // If grounded
            if (!foresight.greenShell)
            { // And capable of jumping off cliffs
                if (hitting != 0 || !foresight.landing)
                { // Then don't
                    TurnAround();
                }
            }
            else
            { // Otherwise only turn at walls
                if (hitting != 0)
                    TurnAround();
            }
        }
    }

    public void TurnAround()
    {
        transform.Rotate(0.0f, 180.0f, 0.0f);
        cov.Rotate(0.0f, 180.0f, 0.0f);
        direction = -direction;
    }

    public void ContactChain()
    {
        for (int i = 0; i < allies.Length; i++)
        { // Check if a guard is below you who is above the noise
            GuardController allyController = allies[i].GetComponent<GuardController>();
            if (allyController.transform.position.y > targetPosition.y)
            { // If they are above you
                for (int j = i + 1; j < allies.Length; j++)
                { // Check if someone is below the guy above
                    GuardController allyController2 = allies[j].GetComponent<GuardController>();
                    if (allyController.transform.position.y > allyController2.transform.position.y &&
                        allyController2.transform.position.y > targetPosition.y)
                    { // Check if that guy is also above the noise
                        i = j;
                        allyController = allies[i].GetComponent<GuardController>();
                        continue;
                    }
                    else if (allyController2.transform.position.y < targetPosition.y ||
                             allyController2.transform.position.y > allyController.transform.position.y)
                    { // If not then you have nobody else to contact, alert this guard
                        allyController.hearing.heard = true;
                        allyController.hearing.sDist = hearing.sDist;
                    }
                }
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

            if (playerPos.x > obstaclePos.x + obstacleScale.x && hitting != 1)
            { // Left wall
                transform.position = new Vector3(obstaclePos.x + (obstacleScale.x + playerScale.x) + range, transform.position.y, 0.0f);
                hitting = -1;
            }
            else if (playerPos.x < obstaclePos.x - obstacleScale.x && hitting != -1)
            { // Right wall
                transform.position = new Vector3(obstaclePos.x - (obstacleScale.x + playerScale.x) - range, transform.position.y, 0.0f);
                hitting = 1;
            }
        }
    }

    public int GetDirection() { return direction; }
    public void SetTargetPos(Vector3 pos) { targetPosition = pos; }
}
