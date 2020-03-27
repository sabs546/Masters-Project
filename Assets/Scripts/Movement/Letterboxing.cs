using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letterboxing : MonoBehaviour
{
    Image[] boxes;
    Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        boxes = GetComponentsInChildren<Image>();
        controller = GameObject.Find("Runner").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    { // When the letterbox appears you're at max speed
        if (Mathf.Abs(controller.currentSpeed) >= controller.topSpeed && boxes[0].rectTransform.anchoredPosition.y > 50.0f)
        { // If box isn't at the limit
            boxes[0].transform.Translate(0.0f, -50.0f * Time.deltaTime, 0.0f);
            boxes[1].transform.Translate(0.0f, 50.0f * Time.deltaTime, 0.0f);
        }
        else if (Mathf.Abs(controller.currentSpeed) < controller.topSpeed && boxes[0].rectTransform.anchoredPosition.y < 150.0f)
        { // If the box isn't at the starting line
            boxes[0].transform.Translate(0.0f, 500.0f * Time.deltaTime, 0.0f);
            boxes[1].transform.Translate(0.0f, -500.0f * Time.deltaTime, 0.0f);
        }
    }
}
