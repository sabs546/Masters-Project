using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public GameObject[] type;
    private Transform[] stable;
    private Transform[] parallax;
    private new CameraFollow camera;
    private float cameraScale;
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
        int j = parallax.Length;
        for (int i = 0; i < parallax.Length; i++)
        {
            parallax[i].transform.position = new Vector3((transform.position.x - (camera.transform.position.x / j) + cameraScale),
                                                         (transform.position.y - (camera.transform.position.y / j) + cameraScale));
            j--;
        }
    }

    public void ResizeBG(float scaling)
    {
        cameraScale += scaling * 2;
        type[1].transform.localScale += new Vector3(scaling * Time.deltaTime, scaling * Time.deltaTime, 0.0f);
    }
}
