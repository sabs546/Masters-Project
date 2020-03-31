using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathTransition : MonoBehaviour
{
    Image[] boxes;
    public bool door = false;
    // Start is called before the first frame update
    void Start()
    {
        boxes = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (boxes[0].rectTransform.anchoredPosition.y > -150.0f)
        { // If box isn't at the limit
            boxes[0].transform.Translate(0.0f, -1000.0f * Time.deltaTime, 0.0f);
            boxes[1].transform.Translate(0.0f, 1000.0f * Time.deltaTime, 0.0f);
        }
        else
        { // When the boxes shut, reset the game
            if (!door)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                GameObject.Find("Door").GetComponent<Door>().enabled = true;
        }
    }
}
