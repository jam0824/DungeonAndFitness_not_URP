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
    public int[] SWITCH;
    int MAX_SWITCH_NUM = 200;
    bool isLock = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        LoadMessageTextObject();
    }

    private void Init() {
        SWITCH = new int[MAX_SWITCH_NUM];
        for (int i = 0; i < MAX_SWITCH_NUM; i++) {
            SWITCH[i] = 0;
        }
    }

    //初回のメッセージウィンドウロード
    private void LoadMessageTextObject() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        MessageTextObject = Instantiate(WindowCanvasPrefab, pos, transform.rotation);
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

    public void SetSwitch(int swno, int value) {
        SWITCH[swno] = value;
    }
    public void CalcurationSwitch(int swno, string sign, int value) {
        switch (sign) {
            case "+":
                SWITCH[swno] += value;
                break;
            case "-":
                SWITCH[swno] -= value;
                break;
        }
    }

    public int GetSwitch(int swno) {
        return SWITCH[swno];
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
            string[] swnoAndValue = tmp.Split("=");
            int swno = int.Parse(swnoAndValue[0]);
            int setValue = int.Parse(swnoAndValue[1]);
            isOk = (SWITCH[swno] == setValue) ? true : false;
        }
        return isOk;
    }
}
