using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public List<Transform> backgrounds;
    private GameObject player;
    private Controller controller;
    private GameObject mainCamera;
    private CameraFollow cameraController;
    private bool noBG;
    // Start is called before the first frame update
    void Start()
    {
        if (backgrounds.Count == 0)
            noBG = true;
        else
            noBG = false;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<Controller>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = mainCamera.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!noBG)
        {
            if (!mainCamera.GetComponent<CameraFollow>().begin)
            {
                backgrounds[0].Translate(-cameraController.speed.x / 4f * Time.deltaTime, -cameraController.speed.y / 4f * Time.deltaTime, 0.0f);
                backgrounds[1].Translate(-cameraController.speed.x / 3.5f * Time.deltaTime, -cameraController.speed.y / 3.5f * Time.deltaTime, 0.0f);
                backgrounds[2].Translate(-cameraController.speed.x / 4f * Time.deltaTime, -cameraController.speed.y / 4f * Time.deltaTime, 0.0f);
            }
            else
            {
                float launch = 0.0f;
                if (Input.GetKey(KeyCode.W) && controller.landing)
                {
                    launch = controller.launchPwr / 1.2f;
                }
                backgrounds[0].Translate(-controller.currentSpeed / 4f * Time.deltaTime, (-(controller.currentFall - launch) / 4f) * Time.deltaTime, 0.0f);
                backgrounds[1].Translate(-controller.currentSpeed / 3.5f  * Time.deltaTime, (-(controller.currentFall - launch) / 3.5f) * Time.deltaTime, 0.0f);
                backgrounds[2].Translate(-controller.currentSpeed / 4f * Time.deltaTime, (-(controller.currentFall - launch) / 4f) * Time.deltaTime, 0.0f);
            }
        }
    }

    public void ResizeBG(float scaling)
    {
        if (!noBG)
        {
            transform.localScale += new Vector3(scaling * Time.deltaTime, scaling * Time.deltaTime, 0.0f);
            transform.Translate(scaling * Time.deltaTime, scaling * Time.deltaTime, 0.0f);
        }
    }
}
