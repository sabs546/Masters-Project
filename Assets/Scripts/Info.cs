using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    private bool colliding;
    public int limit;
    [TextArea]
    public string tooltipTextToShow;
    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (colliding)
        {
            GUI.TextArea(new Rect(0, 0, Screen.width, Screen.height / 10), tooltipTextToShow, limit);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }
}
