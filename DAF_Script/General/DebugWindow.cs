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
        //ÉçÉOÇ™î≠çsÇ≥ÇÍÇΩéûÇ…OnReceiveLogÇ™é¿çsÇ≥ÇÍÇÈÇÊÇ§Ç…Ç∑ÇÈ
        Application.logMessageReceived += OnReceiveLog;
        cText = GameObject.Find("DebugText").GetComponent<Text>();
        cText.text = "testtesttest";

        fpsText = GameObject.Find("FpsText").GetComponent<Text>();

    }

    public void DFDebug(string message) {
        Debug.Log(message);
        /*
        string m = cText.text;
        m = message + "\n" + m;
        cText.text = m;
        */
    }

    public void DFFps(float fps) {
        fpsText.text = "FPS : " + fps;
    }

    private void OnReceiveLog(string logText, string stackTrace, LogType logType) {
        string m = cText.text;
        if(stackTrace != "") {
            if (logType == LogType.Error) {
                m = "<color=red>" + logText + "\n" + stackTrace + "</color>\n" + m;
            }
            else {
                m = logText + "\n" + stackTrace + "\n" + m;
            }
            
        }
        else {
            m = logText + "\n" + m;
        }
        
        cText.text = m;
    }
}
