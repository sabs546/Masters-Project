using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseMaker : MonoBehaviour
{
    [HideInInspector]
    public  float soundRadius;
    private float multiplier;
    private bool stepped;

    public  AudioSource[] sound;
    private Controller    controller;
    // Start is called before the first frame update
    void Start()
    {
        stepped = false;
        controller = GetComponent<Controller>();
        sound = GetComponentsInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (soundRadius > 0.0f && !stepped)
            soundRadius -= 10.0f * Time.deltaTime;
        gameObject.transform.Find("SoundCircle").localScale = new Vector3(soundRadius * 2, soundRadius * 2, 1.0f);
        stepped = false;
        if (soundRadius < 0.1f)
            soundRadius = 0.0f;
    }

    public void AddSound()
    {
        stepped = true;
        if (controller.landing && Mathf.Abs(controller.currentSpeed) > 1.0f && soundRadius < 10.0f)
                soundRadius = Mathf.Abs(controller.currentSpeed / 2.5f);
    }

    public void CallSound(uint soundID, int direction = 0)
    {
        sound[soundID].panStereo = direction;
        sound[soundID].Play();
    }

    public void MakeNoise(float radius)
    {
        soundRadius = Mathf.Abs(radius);
    }
}
