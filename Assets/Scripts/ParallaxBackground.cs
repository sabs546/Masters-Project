using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    public Slider slider;
    private bool  active;
    private float strength;

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
        for (int i = 1; i < stable.Length; i++)
        { // For layers that may not need to move
            stable[i].transform.position = camera.transform.position + transform.localPosition;
        }
        int j = parallax.Length;
        for (int i = 1; i < parallax.Length; i++)
        { // Most of the layers, these all have parallax
            parallax[i].transform.position = new Vector3((transform.position.x - ((camera.transform.position.x / j) * strength)) + cameraScale * strength,
                                                         (transform.position.y - ((camera.transform.position.y / j) * strength)) + cameraScale * strength);
            j--;
        }

        if (active)
        {
            SetParallax();
            active = false;
        }
    }

    public void ResizeBG(float scaling)
    {
        cameraScale += scaling * 2;
        type[1].transform.localScale += new Vector3(scaling * Time.deltaTime, scaling * Time.deltaTime, 0.0f);
    }

    public void SliderActive()
    {
        active = true;
    }

    private void SetParallax()
    {
        strength = slider.value;
    }
}
