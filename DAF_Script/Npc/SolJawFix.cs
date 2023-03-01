using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolJawFix : MonoBehaviour
{
    public GameObject jaw;
    Vector3 DEFAULT_JAW = new Vector3(8f, -90f, 90.154f);

    void Update()
    {
        //Ç†Ç≤ÇÃã∏ê≥
        jaw.transform.localEulerAngles = DEFAULT_JAW;
    }


}
