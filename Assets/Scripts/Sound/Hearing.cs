using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    private float           distanceFromNoise;
    private float           xDist;       // X from player
    private float           yDist;       // Y from player
    public  float           sDist;       // X from noise
    public  bool            deaf;        // Can they hear in their current state
    public  bool            heard;       // Has he heard a noise
    public  bool            contacted;   // Did he hear through contact
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
        heard = false;
        contacted = false;
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
                    sDist = xDist;
                    controller.SetTargetPos(target[i].transform.position); // Stores the players position when heard
                    heard = true;
                    if (state.currentState == 2)
                    {
                        // Noise was heard, let the player know
                        GetComponent<AudioSource>().Play();
                    }
                }
                else
                { // Otherwise he probably didn't hear anything...
                    if (!contacted)
                    { // So you can colour him black...
                        sprite.color = Color.black;
                        heard = false;
                    }
                    else
                    { // Unless of course he was contacted by someone to hear something
                        sDist = xDist;
                    }
                }
            }
        }
    }

    public void FaceTheSound()
    { // Turn to face the direction the noise came from
        if (sDist < 1 && controller.GetDirection() != 1)
        {
            controller.TurnAround();
        }
        else if (sDist > -1 && controller.GetDirection() != -1)
        {
            controller.TurnAround();
        }
    }

    public float GetYDist()
    {
        return yDist;
    }
}
