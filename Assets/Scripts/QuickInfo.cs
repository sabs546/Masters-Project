using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInfo : MonoBehaviour
{
    public  float timeLimit;
    public  bool  triggered;
    public  int   limit;

    private bool  timeUp;
    [TextArea]
    public string tooltipTextToShow;
    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
        timeUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && !timeUp)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0.0f)
            {
                Destroy(this.GetComponent<QuickInfo>());
            }
        }
    }

    private void OnGUI()
    {
        if (triggered)
        {
            timeUp = false;
            GUI.TextArea(new Rect(0, 0, Screen.width, Screen.height / 10), tooltipTextToShow, limit);
        }
    }
}
