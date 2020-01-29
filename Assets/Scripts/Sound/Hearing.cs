using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    private float           distanceFromNoise;
    private float           xDist;
    private float           yDist;
    //public  int            suspicionLevel; This can be added in later
    public  bool            deaf;
    private GameObject[]    target;
    private NoiseMaker      soundSource;
    private OpponentState   state;
    private SpriteRenderer  sprite;
    private GuardController controller;
    private Hearing         hearing;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("NoiseMaker");
        sprite = GetComponent<SpriteRenderer>();
        state = GetComponent<OpponentState>();
        controller = GetComponent<GuardController>();
        hearing = GetComponent<Hearing>();
    }

    // Update is called once per frame
    void Update()
    { /// NEXT TIME HE NEEDS TO MOVE CLOSER TO THE NOISE, NOT JUST THE OTHER SIDE OF THE PLATFORM
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

                    if (state.currentState != 4)
                        state.SetState(5);

                    FaceTheSound();
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
        if (hearing.xDist < 1)
        {
            if (controller.GetDirection() != 1)
                controller.TurnAround(1);
            else if (yDist > 0)
                controller.TurnAround();
        }
        else if (hearing.xDist > -1)
        {
            if (controller.GetDirection() != -1)
                controller.TurnAround(1);
            else if (yDist > 0)
                controller.TurnAround();
        }
    }

    public float GetYDist()
    {
        return yDist;
    }
}
