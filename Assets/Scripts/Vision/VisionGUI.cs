//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor (typeof(ConeOfVision))]

//public class VisionGUI : Editor
//{
//    private void OnSceneGUI()
//    {
//        ConeOfVision cov = (ConeOfVision)target;
//        Handles.color = Color.white;
//        Handles.DrawWireArc(cov.transform.position, Vector3.forward, Vector3.right, 360, cov.viewRadius);
//        Vector3 viewAngleA = cov.DirFromAngle(-cov.viewAngle / 2, false);
//        Vector3 viewAngleB = cov.DirFromAngle(cov.viewAngle / 2, false);

//        Handles.DrawLine(cov.transform.position, cov.transform.position + viewAngleA * cov.viewRadius);
//        Handles.DrawLine(cov.transform.position, cov.transform.position + viewAngleB * cov.viewRadius);
//    }
//}
