using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Controller player;
    private GameObject[] goal;
    private Transform[] trGoal;
    
    public  Vector2 speed;
    public  Vector2 snapPos;
    public  bool    alerted;
    public  bool    begin;   // Has the camera shown the goal yet
    private int     next;    // Which goal is it going to
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Runner").GetComponent<Controller>();
        goal = GameObject.FindGameObjectsWithTag("Goal");
        trGoal = new Transform[goal.Length];
        for (int i = 0; i < goal.Length; ++i)
            trGoal[i] = goal[i].transform;
        transform.position = new Vector3(trGoal[0].position.x, trGoal[0].position.y, transform.position.z);
        begin = false;
        next = 1;
        speed = new Vector2();
        alerted = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!begin)
        {
            if (next != trGoal.Length)
            {
                if (transform.position.x < trGoal[next].position.x - 0.2f)
                    speed.x = 1000.0f * Time.deltaTime;
                else if (transform.position.x > trGoal[next].position.x + 0.2f)
                    speed.x = -1000.0f * Time.deltaTime;
                else
                    speed.x = 0.0f;

                if (transform.position.y < trGoal[next].position.y - 0.2f)
                    speed.y = 1000.0f * Time.deltaTime;
                else if (transform.position.y > trGoal[next].position.y + 0.2f)
                    speed.y = -1000.0f * Time.deltaTime;
                else
                    speed.y = 0.0f;

                transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);

                if (transform.position.x < trGoal[next].position.x + 0.2f && transform.position.x > trGoal[next].position.x - 0.2f &&
                    transform.position.y < trGoal[next].position.y + 0.2f && transform.position.y > trGoal[next].position.y - 0.2f)
                    next++;
            }
            else
            {
                if (transform.position.x < player.transform.position.x - 0.2f)
                    speed.x = 1000.0f * Time.deltaTime;
                else if (transform.position.x > player.transform.position.x + 0.2f)
                    speed.x = -1000.0f * Time.deltaTime;
                else
                    speed.x = 0.0f;

                if (transform.position.y < player.transform.position.y - 0.2f)
                    speed.y = 1000.0f * Time.deltaTime;
                else if (transform.position.y > player.transform.position.y + 0.2f)
                    speed.y = -1000.0f * Time.deltaTime;
                else
                    speed.y = 0.0f;

                transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);

                if (transform.position.x < player.transform.position.x + 0.2f && transform.position.x > player.transform.position.x - 0.2f &&
                    transform.position.y < player.transform.position.y + 0.2f && transform.position.y > player.transform.position.y - 0.2f)
                {
                    begin = true;
                }
            }
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            if (alerted)
                transform.position = new Vector3(snapPos.x, snapPos.y, transform.position.z);
            Camera camera = GetComponent<Camera>();
            ParallaxBackground bg = GetComponentInChildren<ParallaxBackground>();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (camera.orthographicSize < 20.0f)
                {
                    camera.orthographicSize += 10.0f * Time.deltaTime;
                    bg.ResizeBG(0.1f);
                }
                else if (camera.orthographicSize > 20.0f)
                    camera.orthographicSize = 20.0f;
            }
            else
            {
                if (camera.orthographicSize > 10.0f)
                {
                    camera.orthographicSize -= 10.0f * Time.deltaTime;
                    bg.ResizeBG(-0.1f);
                }
                else if (camera.orthographicSize < 10.0f)
                    camera.orthographicSize = 10.0f;
            }
        }
    }
}
