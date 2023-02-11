using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScenarioSystem : MonoBehaviour
{
    public GameObject WindowCanvasPrefab;
    public GameObject SelectBoxCanvasPrefab;
    public GameObject MessageTextObject;
    TextMeshPro messageText;
    public Dictionary<string, string> dictSwitch { set; get; }
    bool isLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ScenarioSystemInit() {
        LoadMessageTextObject();
        dictSwitch = new Dictionary<string, string>();
    }

    /// <summary>
    /// 初回のメッセージウィンドウロード
    /// </summary>
    private void LoadMessageTextObject() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        MessageTextObject = Instantiate(WindowCanvasPrefab, pos, transform.rotation);
        MessageTextObject.transform.parent = SingletonGeneral.instance.dungeonRoot.transform;
        messageText = MessageTextObject.GetComponent<TextMeshPro>();
        MessageTextObject.SetActive(false);
    }

    /// <summary>
    /// メッセージウィンドウの表示
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public TextMeshPro ShowMessageWindow(Vector3 pos, Quaternion r) {
        MessageTextObject.SetActive(true);
        MessageTextObject.transform.position = pos;
        MessageTextObject.transform.rotation = r;
        return messageText;
    }

    /// <summary>
    /// メッセージウィンドウの非表示
    /// </summary>
    public void CloseMessageWindow() {
        messageText.text = "";
        MessageTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// キーと値でswitchを設定する
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetSwitch(string key, string value) {
        dictSwitch[key] = value;
        DebugWindow.instance.DFDebug("***switch set:" + key + "=" + value);
    }

    public void CalcurationSwitch(string key, string sign, string value) {
        switch (sign) {
            case "+":
                dictSwitch[key] = 
                    (int.Parse(dictSwitch[key]) + int.Parse(value)).ToString();
                break;
            case "-":
                dictSwitch[key] = 
                    (int.Parse(dictSwitch[key]) - int.Parse(value)).ToString();
                break;
        }
    }

    public string GetSwitch(string key) {
        return dictSwitch[key];
    }

    public GameObject GetWindowCanvasPrefab() {
        return WindowCanvasPrefab;
    }

    public GameObject GetSelectBoxCanvasPrefab() {
        return SelectBoxCanvasPrefab;
    }

    public void SetLock(bool isValue) {
        isLock = isValue;
    }
    public bool GetLock() {
        return isLock;
    }

    public bool isSwitch(List<string> listValue) {
        bool isOk = false;
        foreach (string value in listValue) {
            if (value == "") continue;
            string tmp = value.Replace(" ", "");
            string[] keyAndValue = tmp.Split("=");
            string key = keyAndValue[0];
            string v = keyAndValue[1];
            if (dictSwitch.ContainsKey(key)) {
                isOk = (dictSwitch[key] == v) ? true : false;
            }
            else {
                isOk = false;
            }
        }
        return isOk;
    }
}
