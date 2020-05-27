using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private bool active;
    public Slider volumeSlider;
    private AudioSource source;
    private float sourceVolume;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        source = GetComponent<AudioSource>();
        sourceVolume = source.volume;
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat(volumeSlider.gameObject.name);
            SetVolume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            SetVolume();
            active = false;
        }
    }

    public void SliderActive()
    {
        active = true;
    }

    private void SetVolume()
    {
        source.volume = sourceVolume * volumeSlider.value;
        PlayerPrefs.SetFloat(volumeSlider.gameObject.name, volumeSlider.value);
    }
}
