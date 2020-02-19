using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public int limit;
    private bool colliding;
    private GameObject player;
    private new GenericCollider collider;

    [TextArea]
    public string tooltipTextToShow;
    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
        player = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<GenericCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (collider.CollisionCheck(transform.position, player.transform.position, transform.lossyScale / 2, player.transform.lossyScale / 2))
        {
            GUI.TextArea(new Rect(0, 0, Screen.width, Screen.height / 10), tooltipTextToShow, limit);
        }
    }
}
