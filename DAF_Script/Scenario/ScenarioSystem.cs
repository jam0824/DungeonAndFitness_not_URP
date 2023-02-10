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
    public Dictionary<string, string> dictSwitch;
    bool isLock = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        LoadMessageTextObject();
    }

    private void Init() {
        dictSwitch = new Dictionary<string, string>();
    }

    //初回のメッセージウィンドウロード
    private void LoadMessageTextObject() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        MessageTextObject = Instantiate(WindowCanvasPrefab, pos, transform.rotation);
        MessageTextObject.transform.parent = SingletonGeneral.instance.dungeonRoot.transform;
        messageText = MessageTextObject.GetComponent<TextMeshPro>();
        MessageTextObject.SetActive(false);
    }

    //メッセージウィンドウの表示
    public TextMeshPro ShowMessageWindow(Vector3 pos, Quaternion r) {
        MessageTextObject.SetActive(true);
        MessageTextObject.transform.position = pos;
        MessageTextObject.transform.rotation = r;
        return messageText;
    }

    //メッセージウィンドウの非表示
    public void CloseMessageWindow() {
        messageText.text = "";
        MessageTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSwitch(string key, string value) {
        dictSwitch[key] = value;
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
