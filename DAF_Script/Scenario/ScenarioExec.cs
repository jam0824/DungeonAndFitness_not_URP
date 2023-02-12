using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioExec : MonoBehaviour
{
    public TextAsset scenario;
    //アイテムがフルだったときのキー
    string FULL_OF_ITEM_KEY = "FullOfItem";
    string MESSAGE_SE_KEY = "MessageNormal";

    public ScenarioSystem scenarioSystem { set; get; }
    public AudioSource audioSource { set; get; }

    List<string[]> listScenarioCsv;
    List<GameObject> listSelectBoxCanvas;

    int lineNo = 0;
    //今一覧のシナリオ実行中か
    bool isNowScenarioExec = false;
    //今1行実行中か
    bool isNowLineExecuting = false;
    bool isLookAt = false;
    

    TextMeshPro messageText;
    
    float messageSpeed = 0.05f;

    string nowMessage = "";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLookAt) CommandLookAt();
           
    }

    //シナリオ実行時にこれを呼ぶ
    public void ScenarioExecution() {
        //1行実行中だったら何もしない。メッセージ重複防ぐ
        if (isNowLineExecuting) return;

        if (!isNowScenarioExec) {
            Init();
        }
        else {
            lineNo = exec(lineNo);
        }
    }

    //初期化
    public void Init() {
        //全体でイベントロック（誰かとイベント中）だったら何もしない
        if (scenarioSystem.GetLock()) return;

        LoadScenario();
        lineNo = GetScenarioLineNo();
        isNowScenarioExec = true;
        scenarioSystem.SetLock(true);
        lineNo = exec(lineNo);
    }

    //行の実行。会話以外は会話などになるまで実行。
    public int exec(int no) {
        isNowLineExecuting = true;
        while (true) {
            DebugWindow.instance.DFDebug("LineNo:" + no);
            string[] line = listScenarioCsv[no];
            string command = line[0];
            
            if (command == "") {
                no++;
                continue;
            }
            else if (command[0] == '#') {
                no++;
                continue;
            }
            else if (command == "look") {
                isLookAt = true;
                CommandLookAt();
                no++;
                continue;
            }
            else if (command == "lookoff") {
                isLookAt = false;
                no++;
                continue;
            }
            else if (command == "goto") {
                no = CommandGoto(line[1], listScenarioCsv);
                continue;
            }
            else if (command == "set") {
                CommandSwitchSet(line);
                no++;
                continue;
            }
            else if (command == "calc") {
                CommandSwitchCalcuration(line);
                no++;
                continue;
            }
            else if (command == "end") {
                CommandEnd();
                break;
            }
            else if (command == "select") {
                CommandSelect(no + 1, listScenarioCsv);
                break;
            }
            else if (command == "itemget") {
                CommandNormalItemGet(line[1], SingletonGeneral.instance.itemDb);
                no++;
                continue;
            }
            else if (command == "se") {
                CommandSe(line[1]);
                no++;
                continue;
            }
            else if (command == "save") {
                CommandSave();
                no++;
                continue;
            }
            else {
                CommandShowMessage(line);
                break;
            }
        }
        return no + 1;
    }

    //会話だった場合
    void CommandShowMessage(string[] line) {
        ShowWindowCanvas();
        nowMessage = FixMessage(line[0]);
        //会話文だった場合は1文字ずつ表示
        if ((line[0][0] == '【') || (line[0][0] == '[')) {
            StartCoroutine(ShowMessage(nowMessage));
        }
        else {
            ShowMessageInstantly();
            isNowLineExecuting = false;
        }
        
        DebugWindow.instance.DFDebug(line[0]);
    }

    //メッセージの編集
    string FixMessage(string line) {
        line = line.Replace("】", "】\n");
        line = line.Replace("]", "]\n");
        line = line.Replace("<br>", "\n");
        return line;
    }

    //メッセージを1文字ずつ表示する
    IEnumerator ShowMessage(string message) {

        for (int i = 0; i < message.Length; i++) {
            if (messageText == null) break;
            if (messageText.text == message) break;
            messageText.text += message[i];
            SingletonGeneral.instance.PlayOneShot(audioSource, MESSAGE_SE_KEY);
            yield return new WaitForSeconds(messageSpeed);
        }
        //会話が終わった
        isNowLineExecuting = false;
    }

    /// <summary>
    /// メッセージの一括表示
    /// </summary>
    /// <param name="message"></param>
    public void ShowMessageInstantly() {
        if(isNowLineExecuting == false) return;
        if (messageText == null) return;
        StopCoroutine(ShowMessage(""));
        messageText.text = nowMessage;
    }

    //メッセージキャンバス作成
    void ShowWindowCanvas() {
        if (messageText == null) {
            Vector3 addPos = new Vector3(0, -0.2f, 0);
            Vector3 pos = SingletonGeneral.instance.GetPosBetweenTargetAndFace(gameObject, addPos);
            messageText = scenarioSystem.ShowMessageWindow(
                pos, 
                SingletonGeneral.instance.GetQuaternionFace());
        }
        else {
            messageText.text = "";
        }
    }

    //メッセージキャンバスクローズ
    void CloseWindowCanvas() {
        messageText = null;
        scenarioSystem.CloseMessageWindow();
    }

    //select時の旗にはスクリプトでは#は付けない。飛ぶべきハタと区別がつかなくなるため
    void CommandSelect(int lineNo, List<string[]> scenario) {
        List<GameObject> listSelectBoxCanvas = new List<GameObject>();
        for (int i = 0; i < scenario.Count; i++) {
            string[] line = scenario[lineNo + i];
            if ((line[0] == "selectend")|| (line[0] == "")) break;
            string flagName = "#" + line[0];
            string message = line[1];
            GameObject selectBoxCanvas = MakeSelectBox(i);
            selectBoxCanvas.GetComponent<ScenarioSelectBox>().SetValues(gameObject, flagName, message);
            listSelectBoxCanvas.Add(selectBoxCanvas);
        }
        this.listSelectBoxCanvas = listSelectBoxCanvas;
    }

    //SELECTBOX作成
    GameObject MakeSelectBox(int boxCount) {
        float geta = 0.3f;
        float boxHeight = 0.2f; 

        Transform windowPosTransform = 
            SingletonGeneral.instance.scenarioSystem.MessageTextObject.transform;
        Vector3 windowPos = windowPosTransform.position;
        Quaternion r = windowPosTransform.rotation;
        windowPos.y += geta + boxCount * boxHeight;
        GameObject selectBoxCanvas = SingletonGeneral.instance.scenarioSystem.GetSelectBoxFromPool();
        selectBoxCanvas.SetActive(true);
        selectBoxCanvas.transform.position = windowPos;
        selectBoxCanvas.transform.rotation = r;

        return selectBoxCanvas;
    }

    /// <summary>
    /// 全てのセレクトボックスを非アクティブにする
    /// </summary>
    void UnenableAllSelectBox() {
        SingletonGeneral.instance.scenarioSystem.UnenableAllSelectBox();
    }

    //slectが実行された時
    public void ExecSelect(string flagName) {
        DebugWindow.instance.DFDebug("select flag name:" + flagName);
        int no = CommandGoto(flagName, this.listScenarioCsv);
        DebugWindow.instance.DFDebug("line:" + no);
        UnenableAllSelectBox();
        lineNo = exec(no);
    }

    //指定したラベルに飛ぶ
    int CommandGoto(string flagName, List<string[]> senario) {
        lineNo = 0;
        for (lineNo = 0; lineNo < senario.Count; lineNo++) {
            string[] checkLine = senario[lineNo];
            if (checkLine[0] == flagName) break;
        }
        if(lineNo == 0) 
            DebugWindow.instance.DFDebug("Goto flag is not found:" + flagName);

        lineNo++;
        return lineNo;
    }

    //Playerを見るようにする
    void CommandLookAt() {
        SingletonGeneral.instance.LookAt(
            SingletonGeneral.instance.face, 
            gameObject);
    }

    //スイッチの数字の計算。+と-のみ。
    //コマンド,スイッチNo,符号,値
    void CommandSwitchCalcuration(string[] line) {
        string key = line[1];
        string sign = line[2];
        string value = line[3];
        scenarioSystem.CalcurationSwitch(key, sign, value);
    }

    //スイッチの値のセット。
    //コマンド,スイッチNo,値
    void CommandSwitchSet(string[] line) {
        string key = line[1];
        string value = line[2];
        scenarioSystem.SetSwitch(key, value);
    }

    //通常枠のアイテムを追加
    void CommandNormalItemGet(string itemNo, ItemDB itemDb) {
        if (itemDb.canAddItem()) {
            DebugWindow.instance.DFDebug("アイテム追加：" + itemNo);
            itemDb.AddItem(itemNo);
        }
        else {
            SingletonGeneral.instance.labelInformationText.SetInformationLabel(FULL_OF_ITEM_KEY);
            Vector3 pos = gameObject.transform.position;
            pos.y += 1f;
            itemDb.MakeItemBag(itemNo, pos, gameObject.transform.rotation);
            DebugWindow.instance.DFDebug("アイテムバッグ作成：" + itemNo);
        }
    }

    //SEを鳴らす
    void CommandSe(string seName) {
        DebugWindow.instance.DFDebug("SE:" + seName);
        SingletonGeneral.instance.PlayOneShotNoAudioSource(seName);
    }

    /// <summary>
    /// Saveする
    /// </summary>
    void CommandSave() {
        DebugWindow.instance.DFDebug("SAVE");
        SingletonGeneral.instance.saveLoadSystem.Save();
        SingletonGeneral.instance.labelInformationText.SetInformationLabel("Save");
    }

    //会話終了
   void CommandEnd() {
        CloseWindowCanvas();
        
        DebugWindow.instance.DFDebug("会話終了");
        //会話終了後、判定と重なっているためすぐ次の話になる。コールチンで待ち時間を入れる
        StartCoroutine("ResetFlag");
    }

    //各種フラグリセット
    IEnumerator ResetFlag() {
        yield return new WaitForSeconds(0.5f);
        isNowLineExecuting = false;
        isNowScenarioExec = false;
        isLookAt = false;
        messageText = null;
        scenarioSystem.SetLock(false);
    }

    //シナリオファイルのロード。シナリオはprefabにアタッチされたもの。
    public void LoadScenario() {
        DebugWindow.instance.DFDebug("シナリオ呼び出し");
        List<string[]> tsvData = FQCommon.Common.LoadTsvFileFromTextAsset(scenario);

        listScenarioCsv = GetLanguageText(tsvData, SingletonGeneral.instance.LanguageMode);
    }

    //日本語と英語はタブ区切りされている。
    //各言語でのパラメーターはcsv区切りされている
    List<string[]> GetLanguageText(List<string[]> tsvData, string language) {
        List<string[]> returnData = new List<string[]>();
        int languageNo = 0;
        if (language == "english") languageNo = 1;

        foreach (string[] data in tsvData) {
            string[] csvLine = data[languageNo].Split(',');
            returnData.Add(csvLine);
        }
        return returnData;
    }


    //実行可能なswitch行を探す。なかった場合は0(最初の行)を返す。
    int GetScenarioLineNo() {
        int no = 0;
        for (int i = 0; i < listScenarioCsv.Count; i++) {
            string[] line = listScenarioCsv[i];
            if(line[0] == "switch") {
                if (isSwitch(line)) {
                    no = i + 1;
                    break;
                }
            }
        }
        return no;
    }

    //switchコマンドが真だった場合。
    //key=1,key=1のようにkey = valueで書く。今は=でアンドのみ。
    bool isSwitch(string[] line) {
        List<string> listValue = new List<string>();
        for (int i = 1; i < line.Length; i++) {
            listValue.Add(line[i]);
        }
        return scenarioSystem.isSwitch(listValue);
    }

    public bool GetIsNowLineExecuting() {
        return isNowLineExecuting;
    }
}
