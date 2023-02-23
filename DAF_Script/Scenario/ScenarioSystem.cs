using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScenarioSystem : MonoBehaviour
{
    DataScenario dataScenario;
    public GameObject WindowCanvasPrefab;
    public GameObject SelectBoxCanvasPrefab;
    public GameObject MessageTextObject;
    List<GameObject> poolSelectBox;
    int SELECT_BOX_MAX = 5;
    TextMeshProUGUI messageText;
    bool isLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ScenarioSystemInit() {
        dataScenario = DataSystem.instance.dataScenario;
        GameObject player = GameObject.Find("Player");
        LoadMessageTextObject(player);
        LoadSelectBoxObject(player);
    }

    /// <summary>
    /// 初回のメッセージウィンドウロード
    /// </summary>
    private void LoadMessageTextObject(GameObject player) {
        Vector3 pos = player.transform.position;
        MessageTextObject = Instantiate(WindowCanvasPrefab, pos, transform.rotation);
        MessageTextObject.transform.SetParent(SingletonGeneral.instance.dungeonRoot.transform);
        messageText = MessageTextObject.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
        MessageTextObject.SetActive(false);
    }

    /// <summary>
    /// 初回のセレクトボックスロード（poolしておく)
    /// </summary>
    void LoadSelectBoxObject(GameObject player) {
        poolSelectBox = LoadObjects(SelectBoxCanvasPrefab, SELECT_BOX_MAX, player);
    }

    List<GameObject> LoadObjects(GameObject obj, int max, GameObject player) {
        Vector3 pos = player.transform.position;
        pos.y -= 2f;
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < max; i++) {
            GameObject poolObject = Instantiate(obj);
            poolObject.transform.position = pos;
            poolObject.transform.SetParent(SingletonGeneral.instance.dungeonRoot.transform);
            //poolObject.SetActive(false);
            StartCoroutine(UnenablePoolObject(poolObject, 0.5f));
            pool.Add(poolObject);
        }
        return pool;
    }

    /// <summary>
    /// 少し表示させてから非アクティブにすることによってメモリーする
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator UnenablePoolObject(GameObject obj, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        obj.SetActive(false);
    }

    /// <summary>
    /// メッセージウィンドウの表示
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public TextMeshProUGUI ShowMessageWindow(Vector3 pos, Quaternion r) {
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
        dataScenario.dictSwitch[key] = value;
        DebugWindow.instance.DFDebug("***switch set:" + key + "=" + value);
    }

    public void CalcurationSwitch(string key, string sign, string value) {
        switch (sign) {
            case "+":
                dataScenario.dictSwitch[key] = 
                    (int.Parse(dataScenario.dictSwitch[key]) + int.Parse(value)).ToString();
                break;
            case "-":
                dataScenario.dictSwitch[key] = 
                    (int.Parse(dataScenario.dictSwitch[key]) - int.Parse(value)).ToString();
                break;
        }
    }

    public string GetSwitch(string key) {
        return dataScenario.dictSwitch[key];
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

    /// <summary>
    /// スイッチが指定した値を含んでいるか、複数のスイッチのセットを確認
    /// and条件
    /// </summary>
    /// <param name="listValue"></param>
    /// <returns></returns>
    public bool isSwitch(List<string> listValue) {
        bool isOk = false;
        foreach (string value in listValue) {
            if (value == "") continue;
            string tmp = value.Replace(" ", "");
            string[] keyAndValue = tmp.Split("=");
            string key = keyAndValue[0];
            string v = keyAndValue[1];
            //keyがitemだった場合は所持itemを検索
            if(key == "item") {
                isOk = SingletonGeneral.instance.itemDb.HasItem(v);
                if (isOk) continue;
                break;
            }
            if (dataScenario.dictSwitch.ContainsKey(key)) {
                if(dataScenario.dictSwitch[key] == v) {
                    isOk = true;
                }
                else {
                    //条件に1回でもマッチしなければfalseを返す。
                    isOk = false;
                    break;
                }
            }
            else {
                //そもそもキーが存在しない場合はfalseを返す
                isOk = false;
                break;
            }
        }
        return isOk;
    }
}
