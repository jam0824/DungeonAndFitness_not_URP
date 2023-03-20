using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawFix : MonoBehaviour
{
    public GameObject jaw;
    Vector3 DEFAULT_JAW = new Vector3(4.757f, -90f, 90.156f);

    private void Start() {
        StartCoroutine(IEJawFix());
    }

    IEnumerator IEJawFix() {
        while (true) {
            yield return new WaitForEndOfFrame();
            jaw.transform.localEulerAngles = DEFAULT_JAW;
        }
    }
}