using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoozeButton : BaseButton
{
    public override void Trigger(Transform obj)
    {
        GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");
        for (int i = 0; i < guards.Length; i++)
        {
            guards[i].GetComponent<OpponentState>().SetState(1);
        }
    }
}
