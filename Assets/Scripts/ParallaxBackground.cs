using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public GameObject[] type;
    private Transform[] stable;
    private Transform[] parallax;
    private CameraFollow camera;
    // Start is called before the first frame update
    void Start()
    {
        stable = type[0].GetComponentsInChildren<Transform>();
        parallax = type[1].GetComponentsInChildren<Transform>();
        camera = GetComponentInParent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < stable.Length; i++)
        {
            stable[i].transform.position = camera.transform.position + transform.localPosition;
        }
        for (int i = 0; i < parallax.Length; i++)
        {
            parallax[i].transform.position = new Vector3(camera.transform.position.x / (i + 1), 0.0f);
        }
    }
}
