using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : BaseState
{
    public override void SetValues(ref float accel, ref float speed, ref float sight, ref float fov, ref bool deaf)
    {
        accel =  0.0f;
        speed =  0.0f;
        sight = 15.0f;
        fov   = 75.0f;
        deaf  = false;
    }
}
