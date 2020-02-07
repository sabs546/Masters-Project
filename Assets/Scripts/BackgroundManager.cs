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
        transform.position = new Vector3(mainCamera.transform.position.x + (transform.lossyScale.x * 2), mainCamera.transform.position.y + transform.lossyScale.y / 2.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!noBG)
        {
            if (!mainCamera.GetComponent<CameraFollow>().begin)
            {
                backgrounds[0].transform.Translate(cameraController.speed.x / 1.1f * Time.deltaTime, cameraController.speed.y / 1.1f * Time.deltaTime, 0.0f);
                backgrounds[1].transform.Translate(cameraController.speed.x / 1.2f * Time.deltaTime, cameraController.speed.y / 1.1f * Time.deltaTime, 0.0f);
                backgrounds[2].transform.Translate(cameraController.speed.x / 1.1f * Time.deltaTime, cameraController.speed.y / 1.1f * Time.deltaTime, 0.0f);
            }

            backgrounds[0].transform.Translate(controller.currentSpeed / 1.1f * Time.deltaTime, controller.currentFall / 1.1f * Time.deltaTime, 0.0f);
            backgrounds[1].transform.Translate(controller.currentSpeed / 1.2f * Time.deltaTime, controller.currentFall / 1.1f * Time.deltaTime, 0.0f);
            backgrounds[2].transform.Translate(controller.currentSpeed / 1.1f * Time.deltaTime, controller.currentFall / 1.1f * Time.deltaTime, 0.0f);
        }
    }

    public void ResizeBG(float scaling)
    {
        if (!noBG)
            transform.localScale += new Vector3(scaling * Time.deltaTime, scaling * Time.deltaTime, 0.0f);
    }
}
