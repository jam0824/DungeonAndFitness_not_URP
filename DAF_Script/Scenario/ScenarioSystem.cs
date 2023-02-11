using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScenarioSystem : MonoBehaviour
{
    public GameObject WindowCanvasPrefab;
    public GameObject SelectBoxCanvasPrefab;
    public GameObject MessageTextObject;
    List<GameObject> poolSelectBox;
    int SELECT_BOX_MAX = 5;
    TextMeshPro messageText;
    public Dictionary<string, string> dictSwitch { set; get; }
    bool isLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ScenarioSystemInit() {
        LoadMessageTextObject();
        LoadSelectBoxObject();
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
    /// 初回のセレクトボックスロード（poolしておく)
    /// </summary>
    void LoadSelectBoxObject() {
        poolSelectBox = LoadObjects(SelectBoxCanvasPrefab, SELECT_BOX_MAX);
    }

    List<GameObject> LoadObjects(GameObject obj, int max) {
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < max; i++) {
            GameObject poolObject = Instantiate(obj);
            poolObject.transform.position = new Vector3(-10f, -10f, -10f);
            poolObject.SetActive(false);
            pool.Add(poolObject);
        }
        return pool;
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

    /// <summary>
    /// 非アクティブになっているSelectBoxが使われてないのでそれを返す。
    /// </summary>
    /// <returns></returns>
    public GameObject GetSelectBoxFromPool() {

        foreach (GameObject obj in poolSelectBox) {
            if (obj.activeSelf == false) return obj;
        }
        return null;
    }

    /// <summary>
    /// SelectBoxを使い終わったら全部非アクティブにする。
    /// </summary>
    public void UnenableAllSelectBox() {
        foreach (GameObject obj in poolSelectBox) {
            if (obj.activeSelf == true) obj.SetActive(false);
        }
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

    public GameObject GetMessageTextObject() {
        return MessageTextObject;
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
