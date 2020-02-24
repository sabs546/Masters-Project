using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public int limit;
    private bool colliding;
    private GameObject player;
    private new GenericCollider collider;
    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
        player = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<GenericCollider>();
        canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collider.CollisionCheck(transform.position, player.transform.position, transform.lossyScale / 2, player.transform.lossyScale / 2))
            canvas.enabled = true;
        else
            canvas.enabled = false;
    }
}
