using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public override void SetValues(ref float accel, ref float speed, ref float sight, ref float fov, ref bool deaf)
    {
        accel =  0.0f;
        speed =  0.0f;
        sight =  0.0f;
        fov   =  0.0f;
        deaf  =  true;
    }
}
