using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSetup : MonoBehaviour
{
    public  int        wallType;
    private GameObject player;
    private Controller controller;

    private float      density;
    private GameObject boxCollider;

    private float      grip;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.gameObject;
        if (wallType != 4)
            boxCollider.AddComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<Controller>();
    }

    public void SetVariables(ref float run, ref float cling)
    { /// MAKE THIS WORK WITH ANY OBJECT IN THE GAME
        switch (wallType)
        {
            case -1: // Slippy toad
                grip = 0.1f;
                break;
            case 0: // Normal
                grip = 1.0f;
                break;
            case 1: // Rough
                grip = 10.0f;
                break;
            case 2: // Silence
                grip = 1.0f;
                if (player.GetComponent<NoiseMaker>().enabled == true)
                    player.GetComponent<NoiseMaker>().enabled = false;
                break;
            case 3: // For tutorial purposes, it gives the player sound
                grip = 1.0f;
                if (player.GetComponent<NoiseMaker>().enabled == false)
                    player.GetComponent<NoiseMaker>().enabled = true;
                break;
            case 4: // 0 but guards can see through it
                grip = 1.0f;
                break;
        }
        run *= grip;
        cling *= grip;
    }
}