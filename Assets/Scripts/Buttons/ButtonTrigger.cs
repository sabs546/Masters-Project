﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    private int currentState;
    private GameObject player;
    public int buttonType;
    public bool startup;
    public bool door;
    GameObject[] guards;
    private Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        currentState = 2;
        player = GameObject.FindGameObjectWithTag("Player");
        guards = GameObject.FindGameObjectsWithTag("Guard");
        if (!startup)
            timer = null;
        else
        {
            if (GetComponent<QuickInfo>() != null)
                GetComponent<QuickInfo>().triggered = true;
            gameObject.AddComponent<Timer>().SetLimit(5.0f);
            timer = GetComponent<Timer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CollisionCheck(player.transform.position, transform.position, player.transform.lossyScale / 2, transform.lossyScale / 2);
        if (timer != null && timer.timeUp)
            GetComponentInChildren<Canvas>().enabled = false;
    }

    public void OnPress(int type)
    {
        switch (type)
        {
            case -2: // Kill
                GameObject DT = GameObject.Find("Main Camera");
                DT.GetComponentInChildren<DeathTransition>().enabled = true;
                if (GetComponent<Door>() != null)
                {
                    DT.GetComponentInChildren<DeathTransition>().door = gameObject;
                    DT.GetComponentInChildren<AudioSource>().Play();
                }
                DT.GetComponentInChildren<Letterboxing>().enabled = false;
                break;
            case -1:
                // Base button
                break;
            case 0: // Snooze button
                BaseButton snooze = new SnoozeButton();
                snooze.Trigger(transform);
                break;
            case 1: // Panic button
                BaseButton panic = new PanicButton();
                panic.Trigger(transform);
                break;
            case 2: // Empty
                BaseButton empty = new Empty();
                empty.Trigger(transform);
                break;
        }
    }

    //public void OnPress(int type)
    //{
    //    switch (type)
    //    {
    //        case -1:
    //            // Base button
    //            break;
    //        case 0: // Snooze button
    //            for (int i = 0; i < guards.Length; i++)
    //            {
    //                OpponentState state = guards[i].GetComponent<OpponentState>();
    //                state.SetState(1);
    //            }
    //            break;
    //        case 1: // Panic button
    //            float furthestGuard = 0.0f;
    //            Vector3 dist;
    //            for (int i = 0; i < guards.Length; i++)
    //            {
    //                OpponentState state = guards[i].GetComponent<OpponentState>();
    //                dist.x = transform.position.x - guards[i].transform.position.x;
    //                dist.y = transform.position.y - guards[i].transform.position.y;
    //                dist.z = Mathf.Abs(Mathf.Sqrt((dist.x * dist.x) + (dist.y * dist.y)));
    //                if (furthestGuard < dist.z)
    //                    furthestGuard = dist.z;
    //            }
    //            GetComponent<NoiseMaker>().MakeNoise(furthestGuard);
    //            break;
    //    }
    //}

    private void CollisionCheck(Vector2 playerPos, Vector2 obstaclePos, Vector2 playerScale, Vector2 obstacleScale)
    {
        if (playerPos.y >= obstaclePos.y - (obstacleScale.y + playerScale.y) && playerPos.y <= obstaclePos.y + (obstacleScale.y + playerScale.y) &&
            playerPos.x >= obstaclePos.x - (obstacleScale.x + playerScale.y) && playerPos.x <= obstaclePos.x + (obstacleScale.x + playerScale.y))
        {
            if (currentState == 2)
            { // press the button down
                currentState = 0;
                OnPress(buttonType);
                if (GetComponent<QuickInfo>() != null)
                    GetComponent<QuickInfo>().triggered = true;
                else if (GetComponentInChildren<Canvas>() != null)
                {
                    GetComponentInChildren<Canvas>().enabled = true;
                    gameObject.AddComponent<Timer>().SetLimit(5.0f);
                    timer = GetComponent<Timer>();
                }
            }
        }
    }
}
