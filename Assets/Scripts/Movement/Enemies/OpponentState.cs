using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentState : MonoBehaviour
{
    public  int                currentState; // What state is it in
    private ConeOfVision       visionSense;
    private GuardController    controller;   // Movement speed
    private BaseState          state;
    private Hearing            hearing;

    private void Start()
    {
        visionSense = GetComponentInChildren<ConeOfVision>();
        controller = GetComponent<GuardController>();
        hearing = GetComponent<Hearing>();
        SetState(currentState);
    }

    public void SetState(int current)
    { // Time for a remake
        currentState = current;
        switch (current)
        {
            case 0: // Dead
                state = new DeadState();
                state.SetValues(ref controller.accel, ref controller.topSpeed, ref visionSense.viewRadius, ref visionSense.viewAngle, ref hearing.deaf);
                break;
            case 1: // Down
                state = new DownState();
                state.SetValues(ref controller.accel, ref controller.topSpeed, ref visionSense.viewRadius, ref visionSense.viewAngle, ref hearing.deaf);
                break;
            case 2: // Guard
                state = new GuardState();
                state.SetValues(ref controller.accel, ref controller.topSpeed, ref visionSense.viewRadius, ref visionSense.viewAngle, ref hearing.deaf);
                break;
            case 3: // Patrol
                state = new PatrolState();
                state.SetValues(ref controller.accel, ref controller.topSpeed, ref visionSense.viewRadius, ref visionSense.viewAngle, ref hearing.deaf);
                break;
            case 4: // Alert
                state = new AlertState();
                state.SetValues(ref controller.accel, ref controller.topSpeed, ref visionSense.viewRadius, ref visionSense.viewAngle, ref hearing.deaf);
                break;
            case 5: // Chase
                state = new ChaseState();
                state.SetValues(ref controller.accel, ref controller.topSpeed, ref visionSense.viewRadius, ref visionSense.viewAngle, ref hearing.deaf);
                break;
        }
    }

    /*public void SetState(int state)
    {
        switch (state)
        {
            case 0: // Dead - Does what the tin says
                controller.currentSpeed = 0.0f;
                visionSense.viewRadius = 0.0f;
                visionSense.viewAngle = 0.0f;
                //noiseSense = 0.0f;
                break;

            case 1: // Down - He's just asleep, touching him will wake him up, he also remembers his alert level when he wakes up
                controller.currentSpeed = 0.0f;
                visionSense.viewRadius = 0.0f;
                //noiseSense = 5.0f;
                break;

            case 2: // Guard - Very focused on one point, so his vision is decent
                controller.currentSpeed = 0.0f;
                visionSense.viewRadius = 15.0f;
                //noiseSense = 10.0f;
                break;

            case 3: // Patrol - Standard senses, not particularly focused but still watching
                controller.currentSpeed = 5.0f;
                visionSense.viewRadius = 10.0f;
                //noiseSense = 10.0f;
                break;

            case 4: // Alert - Senses sharpened and on high alert, will attempt to get to noise location
                controller.currentSpeed = 5.0f;
                visionSense.viewRadius = 15.0f;
                //noiseSense = 15.0f;
                break;

            case 5: /// Chase - Runs faster and chases the player "hopefully"
                controller.currentSpeed = 15.0f;
                visionSense.viewRadius = 10.0f;
                //noiseSense = 15.0f;
                break;
        }
    }*/
}