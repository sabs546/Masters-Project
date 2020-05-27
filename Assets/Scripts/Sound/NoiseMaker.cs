using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class NoiseMaker : MonoBehaviour
{
    [HideInInspector]
    public  float soundRadius;
    private bool  stepped;

    public  AudioSource[] sound;
    private float[]       sourceVolume; // Store the original value of the noise
    private Controller    controller;
    public  Slider        volumeSlider; // The slider that affects the noise
    private bool          active;
    // Start is called before the first frame update
    void Start()
    {
        stepped = false;
        active = false;
        controller = GetComponent<Controller>();
        sound = GetComponentsInChildren<AudioSource>();
        sourceVolume = new float[sound.Length];
        for (int i = 0; i < sound.Length; ++i)
        {
            sourceVolume[i] = sound[i].volume;
        }
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat(volumeSlider.gameObject.name);
            SetVolume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (soundRadius > 0.0f && !stepped)
            soundRadius -= 10.0f * Time.deltaTime; // Make the noise decay
        gameObject.transform.Find("SoundCircle").localScale = new Vector3(soundRadius * 2, soundRadius * 2, 1.0f);
        stepped = false;
        if (soundRadius < 0.1f)
            soundRadius = 0.0f;

        if (active)
        {
            SetVolume();
            active = false;
        }
    }

    public void AddSound()
    {
        stepped = true;
        if (controller.landing && Mathf.Abs(controller.currentSpeed) > 1.0f && soundRadius < 10.0f)
        { // Add sounds to the sound radius
            float sound = Mathf.Abs(controller.currentSpeed) / 2.5f;
            if (sound > soundRadius)
                soundRadius = sound; // It should only make a sound louder if the new sound is louder than before
        }
    }

    public void CallSound(uint soundID, int direction = 0)
    {
        sound[soundID].panStereo = direction;
        sound[soundID].Play();
    }

    public void MakeNoise(float radius)
    {
        if (soundRadius < Mathf.Abs(radius))
            soundRadius = Mathf.Abs(radius);
    }

    public void SliderActive()
    {
        active = true;
    }

    private void SetVolume()
    {
        for (int i = 0; i < sound.Length; ++i)
        {
            sound[i].volume = sourceVolume[i] * volumeSlider.value;
            PlayerPrefs.SetFloat(volumeSlider.gameObject.name, volumeSlider.value);
        }
    }
}
