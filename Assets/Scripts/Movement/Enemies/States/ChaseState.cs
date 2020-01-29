﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    public override void SetValues(ref float accel, ref float speed, ref float sight, ref float fov, ref bool deaf)
    {
        accel =  0.2f;
        speed = 15.0f;
        sight = 10.0f;
        fov   = 75.0f;
        deaf  = false;
    }
}
