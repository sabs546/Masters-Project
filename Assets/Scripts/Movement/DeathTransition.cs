using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathTransition : MonoBehaviour
{
    Image[] boxes;
    public GameObject door;
    private float transitionSpeed;
    // Start is called before the first frame update
    void Start()
    {
        transitionSpeed = 1000.0f;
        boxes = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (door)
        {
            transitionSpeed = 500.0f;
        }
        if (boxes[0].rectTransform.anchoredPosition.y > -150.0f)
        { // If box isn't at the limit
            boxes[0].transform.Translate(0.0f, -transitionSpeed * Time.deltaTime, 0.0f);
            boxes[1].transform.Translate(0.0f, transitionSpeed * Time.deltaTime, 0.0f);
        }
        else
        { // When the boxes shut, reset the game
            if (door == null)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                door.GetComponent<Door>().enabled = true;
        }
    }
}
