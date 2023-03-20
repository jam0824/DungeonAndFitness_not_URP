using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawFix : MonoBehaviour
{
    public GameObject jaw;
    public Vector3 DEFAULT_JAW;

    void LateUpdate() {
        //Ç†Ç≤ÇÃã∏ê≥
        jaw.transform.localEulerAngles = DEFAULT_JAW;
    }
}