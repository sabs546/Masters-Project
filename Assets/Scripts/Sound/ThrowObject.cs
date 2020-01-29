using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [HideInInspector]
    public  float        currentSpeed;
    public  float        accel;
    public  float        cling;
    public  float        gravity;
    private float        currentFall;    // Speed the object is falling at

    public  float        throwStrength;
    public  float        throwAngle;
    public  GameObject   decoy;
    private WallSetup[]  wallProperties;
    private GameObject[] walls;          // Store all the walls here

    private bool         landing;        // Is it hitting a floor
    private int          currentFloor;   // Which floor is it standing on
    private int          hitting;        // -1/0/1 | Is it hitting a wall

    // Start is called before the first frame update
    void Start()
    {
        currentFall = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // YOU NEED TO ADD SOME MOVEMENT TO THIS THING, AND SORT OUT THE POSITION IT SPAWNS IN
            Instantiate(decoy);
        }
        currentFall -= gravity;
        decoy.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
        decoy.transform.Translate(0.0f, currentFall, 0.0f);
    }

    private void CollisionCheck(Vector2 playerPos, Vector2 obstaclePos, Vector2 playerScale, Vector2 obstacleScale, int i)
    {
        if (playerPos.y >= obstaclePos.y - (obstacleScale.y + playerScale.y) && playerPos.y <= obstaclePos.y + (obstacleScale.y + playerScale.y) &&
            playerPos.x >= obstaclePos.x - (obstacleScale.x + playerScale.y) && playerPos.x <= obstaclePos.x + (obstacleScale.x + playerScale.y))
        { /// gET IT TO REACT TO THE WALLS LIKE THE PLAYER DOES
            if (playerPos.y > obstaclePos.y + obstacleScale.y)
            { // Ground
                transform.position = new Vector3(playerPos.x, obstaclePos.y + (obstacleScale.y + playerScale.y), 0.0f);
                landing = true;
                currentFall = 0;
                wallProperties[i].SetVariables(ref accel, ref cling);
                return;
            }
            else if (playerPos.y < obstaclePos.y - obstacleScale.y)
            { // Ceiling
                transform.position = new Vector3(playerPos.x, obstaclePos.y - (obstacleScale.y + playerScale.y), 0.0f);
                currentFall = 0;
                wallProperties[i].SetVariables(ref accel, ref cling);
                return;
            }

            if (playerPos.x > obstaclePos.x)
            { // Left wall
                transform.position = new Vector3(obstaclePos.x + (obstacleScale.x + playerScale.y), playerPos.y, 0.0f);
                hitting = -1;
                currentSpeed = 0;
            }
            else if (playerPos.x < obstaclePos.x)
            { // Right wall
                transform.position = new Vector3(obstaclePos.x - (obstacleScale.x + playerScale.y), playerPos.y, 0.0f);
                hitting = 1;
                currentSpeed = 0;
            }
            wallProperties[i].SetVariables(ref accel, ref cling);
        }
    }
}
