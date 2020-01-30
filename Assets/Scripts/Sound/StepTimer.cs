using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTimer : MonoBehaviour
{
    AudioSource[] stepSound;
    public float timeLimit;
    bool moving;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        stepSound = GetComponentsInChildren<AudioSource>();
        moving = false;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving && active)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0.0f)
            {
                stepSound[Random.Range(0, stepSound.Length)].Play();
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
