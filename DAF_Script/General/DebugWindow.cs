using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugWindow : MonoBehaviour
{
    public static DebugWindow instance;
    Text cText;
    Text fpsText;

    string logFileName = "log.txt";

    private void Awake() {
        if(instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            //Destroy(gameObject);
        }
        //ÉçÉOÇ™î≠çsÇ≥ÇÍÇΩéûÇ…OnReceiveLogÇ™é¿çsÇ≥ÇÍÇÈÇÊÇ§Ç…Ç∑ÇÈ
        Application.logMessageReceived += OnReceiveLog;
        cText = GameObject.Find("DebugText").GetComponent<Text>();
        cText.text = DataSystem.instance.log;

    }

    private void OnApplicationPause(bool pause) {
        SaveLog();
    }

    public void DFDebug(string message) {
        Debug.Log(message);
    }

    public void DFFps(float fps) {
        fpsText.text = "FPS : " + fps;
    }

    private void OnReceiveLog(string logText, string stackTrace, LogType logType) {
        if (cText == null) return;
        
        string line = "";
        if(stackTrace != "") {
            if (logType == LogType.Error) {
                line = "Error\t<color=#ee4444>" + logText + "\n" + stackTrace + "</color>\n";
                SingletonGeneral.instance.PlayOneShotNoAudioSource("Error");
            }
            else if (logType == LogType.Exception) {
                line = "Exception\t<color=#ee4444>" + logText + "\n" + stackTrace + "</color>\n";
                SingletonGeneral.instance.PlayOneShotNoAudioSource("Error");
            }
            else if (logType == LogType.Warning) {
                line = "Warning\t<color=#ff8c00>" + logText + "\n" + stackTrace + "</color>\n";
            }
            else {
                line = "Log\t" + logText + "\n" + stackTrace + "\n";
            }
        }
        else {
            line = "Log\t" + logText + "\n";
        }
        DataSystem.instance.log = line + DataSystem.instance.log;

        if (!SingletonGeneral.instance.GetDebugMode()) {
            if(gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }
        cText.text = DataSystem.instance.log;
    }

    void SaveLog() {
        string data = DataSystem.instance.log;
        FQCommon.Common.SaveStringToFile(logFileName,data);
    }
}
