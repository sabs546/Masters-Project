using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public    int    currentState; // Useless, might be removed later
    public    bool[] options; // Useless as of yet
    protected float  speed;
    protected float  sight;
    protected float  fov;
    protected bool   deaf;

    public virtual void SetValues(ref float accel, ref float speed, ref float sight, ref float fov, ref bool deaf) {}
}
