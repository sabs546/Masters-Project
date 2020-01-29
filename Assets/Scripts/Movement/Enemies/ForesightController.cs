using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForesightController : MonoBehaviour
{
    //--------------------------------------------------
    // - Variable setup -
    //-------
    public  float range;
    public  bool  greenShell; // Will the AI walk off cliffs
    public  bool  landing;
    private GuardController parent;   // Get this stuff from the parent

    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<GuardController>();
    }

    // Update is called once per frame
    void Update()
    {
        landing = false;
    }
}
