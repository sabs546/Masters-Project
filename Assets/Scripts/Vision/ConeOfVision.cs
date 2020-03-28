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

    Mesh mesh;

    void Start()
    {
        state = GetComponent<OpponentState>();
        controller = GetComponent<GuardController>();

        mesh = new Mesh();
        GetComponentInChildren<MeshFilter>().mesh = mesh;
        GetComponentInChildren<MeshRenderer>().sortingLayerName = "Main characters";
        GetComponentInChildren<MeshRenderer>().sortingOrder = -1;
    }

    private void Update()
    {
        FindVisibleTargets(); // Does what it says on the tin
        float angle = viewAngle / 2; // This should match up with function above
        Vector3 origin = Vector3.zero;
        int rayCount = 300; // Looks smoother
        float angleIncrease = viewAngle / rayCount; // Where to start the next triangle

        Vector3[] vertices = new Vector3[rayCount + 1 + 1]; // Extras for the edges
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle) * controller.GetDirection(), viewRadius);
            if (raycastHit2D.collider == null)
            { // If it doesn't hit an object cast the full ray
                vertex = origin + (GetVectorFromAngle(angle) * controller.GetDirection()) * viewRadius;
            }
            else
            { // Otherwise, cut it short
                vertex = origin + (GetVectorFromAngle(angle) * controller.GetDirection()) * raycastHit2D.distance;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
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

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180.0f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    void FindVisibleTargets()
    {
        targetInViewRadius = Physics2D.OverlapCircle(transform.position, viewRadius, targetMask);

        if (targetInViewRadius)
        {
            Transform target = targetInViewRadius.transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    playerInSight = true;
                    Debug.Log("We been spooted");
                    GameObject.Find("Main Camera").GetComponentInChildren<DeathTransition>().enabled = true;
                    GameObject.Find("Main Camera").GetComponentInChildren<Letterboxing>().enabled = false;
                    // The game should just restart here
                }
            }
        }
    }
}
