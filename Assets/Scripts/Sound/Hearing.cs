using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    private float           distanceFromNoise;
    private float           xDist;       // X from noise
    private float           yDist;       // Y from noise
    public  bool            deaf;        // Can they hear in their current state
    private GameObject[]    target;      // What makes noises in this scene
    private NoiseMaker      soundSource; // What is making the noise
    private OpponentState   state;       // State script for the guard, for when they hear something
    private SpriteRenderer  sprite;      // Turn red
    private GuardController controller;  // Setting the position of the target

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        state = GetComponent<OpponentState>();
        controller = GetComponent<GuardController>();
    }

    // Update is called once per frame
    void Update()
    { /// NEXT TIME HE NEEDS TO MOVE CLOSER TO THE NOISE, NOT JUST THE OTHER SIDE OF THE PLATFORM
        target = GameObject.FindGameObjectsWithTag("NoiseMaker");
        for (int i = 0; i < target.Length; i++)
        {
            soundSource = target[i].GetComponentInParent<NoiseMaker>();
            if (soundSource.enabled)
            {
                xDist = transform.position.x - target[i].transform.position.x;
                yDist = transform.position.y - target[i].transform.position.y;
                distanceFromNoise = Mathf.Sqrt((xDist * xDist) + (yDist * yDist));

                if (distanceFromNoise - Mathf.Abs(soundSource.soundRadius) <= transform.lossyScale.x)
                { // Make him alert when he hears a noise
                    sprite.color = Color.red;
                    controller.SetTargetPos(target[i].transform.position); // Stores the players position when heard

                    state.SetState(4);
                    FaceTheSound();
                    if (yDist > 0 && controller.alertLevel == 2)
                    { // Let him fall off cliffs if he's above the sound and been bothered before
                        GetComponent<ForesightController>().greenShell = true;
                    }
                }
                else
                {
                    sprite.color = Color.black;
                }
            }
        }
    }

    public void FaceTheSound()
    {
        if (xDist < 1 && controller.GetDirection() != 1)
        {
            controller.TurnAround();
        }
        else if (xDist > -1 && controller.GetDirection() != -1)
        {
            controller.TurnAround();
        }
    }

    public float GetYDist()
    {
        return yDist;
    }
}
