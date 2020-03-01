using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector]
    public  bool  timeUp;
    private float timeLimit;

    // Start is called before the first frame update
    void Start()
    {
        timeUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeUp)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0)
            {
                timeUp = true;
            }
        }
    }

    public void SetLimit(float time)
    {
        timeLimit = time;
        timeUp = false;
    }
}
