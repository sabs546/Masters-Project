using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseMaker : MonoBehaviour
{
    [HideInInspector]
    public  float soundRadius;
    private float multiplier;

    public  AudioSource[] sound;
    private Controller    controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
        sound = GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { /// PERHAPS GIVE SOUND A DECAY RATE?
        if (controller != null)
        {
            if (controller.landing && controller.currentSpeed > 1.0f && soundRadius < 10.0f)
                soundRadius += controller.currentSpeed * Time.deltaTime;
            else if ((!controller.landing || controller.currentSpeed < 1.0f) && soundRadius > 0.0f)
                soundRadius -= 10.0f * Time.deltaTime;
        }
        else if (soundRadius > 0.0f)
        {
            soundRadius -= 10.0f * Time.deltaTime;
        }
    }

    public void CallSound(uint soundID, int direction = 0)
    {
        sound[soundID].panStereo = direction;
        sound[soundID].Play();
    }

    public void MakeNoise(float radius)
    {
        soundRadius = radius;
    }
}
