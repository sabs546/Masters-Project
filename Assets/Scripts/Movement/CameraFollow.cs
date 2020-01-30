﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Controller player;
    private GameObject[] goal;
    private Transform[] trGoal;

    public  bool begin; // Has the camera shown the goal yet
    private int next; // Which goal is it going to
    private Vector2 speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Runner").GetComponent<Controller>();
        goal = GameObject.FindGameObjectsWithTag("Goal");
        trGoal = new Transform[goal.Length];
        for (int i = 0; i < goal.Length; ++i)
            trGoal[i] = goal[i].transform;
        transform.position = new Vector3(trGoal[0].position.x, trGoal[0].position.y, transform.position.z);
        begin = false;
        next = 1;
        speed = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        if (!begin)
        {
            if (next != trGoal.Length)
            {
                if (transform.position.x < trGoal[next].position.x)
                    speed.x = 20.0f;
                else if (transform.position.x > trGoal[next].position.x)
                    speed.x = -20.0f;
                if (transform.position.x < trGoal[next].position.y)
                    speed.y = 20.0f;
                else if (transform.position.y > trGoal[next].position.y)
                    speed.y = -20.0f;

                transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);

                if (transform.position.x <= trGoal[next].position.x + 0.1f && transform.position.x >= trGoal[next].position.x - 0.1f &&
                    transform.position.y <= trGoal[next].position.y + 0.1f && transform.position.y >= trGoal[next].position.y - 0.1f)
                    next++;
            }
            else
            {
                if (transform.position.x < player.transform.position.x)
                    speed.x = 10.0f;
                else if (transform.position.x > player.transform.position.x)
                    speed.x = -10.0f;
                if (transform.position.y < player.transform.position.y)
                    speed.y = 10.0f;
                else if (transform.position.y > player.transform.position.y)
                    speed.y = -10.0f;

                transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);

                if (transform.position.x < player.transform.position.x + 0.1f && transform.position.x > player.transform.position.x - 0.1f &&
                    transform.position.y < player.transform.position.y + 0.1f && transform.position.y < player.transform.position.y + 0.1f)
                {
                    begin = true;
                }
            }
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (GetComponent<Camera>().orthographicSize < 20.0f)
                    GetComponent<Camera>().orthographicSize += 10.0f * Time.deltaTime;
                else if (GetComponent<Camera>().orthographicSize > 20.0f)
                    GetComponent<Camera>().orthographicSize = 20.0f;
            }
            else
            {
                if (GetComponent<Camera>().orthographicSize > 10.0f)
                    GetComponent<Camera>().orthographicSize -= 10.0f * Time.deltaTime;
                else if (GetComponent<Camera>().orthographicSize < 10.0f)
                    GetComponent<Camera>().orthographicSize = 10.0f;
            }
        }
    }
}
