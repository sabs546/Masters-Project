using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOfVision : MonoBehaviour
{
    [Range(0,360)]
    public float viewAngle;
    [HideInInspector]
    public float viewRadius;

    public  bool            playerInSight;
    public  LayerMask       targetMask;
    public  LayerMask       obstacleMask;
    public  Collider2D      targetInViewRadius;

    private OpponentState   state;
    private GuardController controller;

    void Start()
    {
        state = GetComponent<OpponentState>();
        controller = GetComponent<GuardController>();
    }

    void FindVisibleTargets()
    {
        targetInViewRadius = Physics2D.OverlapCircle(transform.position, viewRadius, targetMask);

        if (targetInViewRadius)
        {
            Transform target = targetInViewRadius.transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.right, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    playerInSight = true;
                    Debug.Log("We been spooted");
                    // The game should just restart here
                }
            }
        }
    }

    private void Update()
    {
        FindVisibleTargets();
    }

    public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SetViewRadius(float radius)
    {
        viewRadius = radius;
    }
}
