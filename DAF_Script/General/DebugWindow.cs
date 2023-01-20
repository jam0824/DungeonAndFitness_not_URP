using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugWindow : MonoBehaviour
{
    public static DebugWindow instance;
    Text cText;
    Text fpsText;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        cText = GameObject.Find("DebugText").GetComponent<Text>();
        cText.text = "testtesttest";

        fpsText = GameObject.Find("FpsText").GetComponent<Text>();

    }

    public void DFDebug(string message) {
        Debug.Log(message);
        string m = cText.text;
        m = message + "\n" + m;
        cText.text = m;
    }

    public void DFFps(float fps) {
        fpsText.text = "FPS : " + fps;
    }
}
