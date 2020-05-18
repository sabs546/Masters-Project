using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTimer : MonoBehaviour
{
    public AudioClip[] stepSound;
    AudioSource stepSource;
    public float timeLimit;
    bool moving;
    bool active;
    private NoiseMaker sfx;
    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        active = false;
        sfx = GetComponentInParent<NoiseMaker>();
        stepSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving && active)
        { // Make step noises while you're moving
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0.0f)
            { // The steps should come out in waves
                sfx.AddSound(); // The noise the player makes comes from those sounds
                stepSource.clip = stepSound[Random.Range(0, stepSound.Length)];
                stepSource.Play(); // They can get repetitive, use multiple
                active = false;
            }
        }
    }

    public void MakeNoise(float limit)
    {
        if (!active)
        {
            active = true;
            moving = true;
            timeLimit = limit;
        }
    }

    public void StopMoving()
    {
        moving = false;
        active = false;
        timeLimit = 0.0f;
    }
}
