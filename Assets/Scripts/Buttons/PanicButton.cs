using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicButton : BaseButton
{
    public override void Trigger(Transform obj)
    {
        GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
        float furthestGuard = 0.0f;
        Vector3 dist;
        for (int i = 0; i < guards.Length; i++)
        {
            dist.x = obj.transform.position.x - guards[i].transform.position.x;
            dist.y = obj.transform.position.y - guards[i].transform.position.y;
            dist.z = Mathf.Abs(Mathf.Sqrt((dist.x * dist.x) + (dist.y * dist.y)));
            if (furthestGuard < dist.z)
                furthestGuard = dist.z;
        }
        obj.GetComponentInChildren<NoiseMaker>().MakeNoise(furthestGuard);
    }
}
