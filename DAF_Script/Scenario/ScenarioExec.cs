using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioExec : MonoBehaviour
{
    public TextAsset scenario;
    public string MESSAGE_TYPE = "MessageNormal";
    public float MESSAGE_Y;
    float SELECTBOX_Y = 1.2f;

    public ScenarioSystem scenarioSystem { set; get; }
    public GeneralSystem generalSystem { set; get; }
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
            else {
                CommandShowMessage(line);
                break;
            }
        }
        return no + 1;
    }

    //会話だった場合
    void CommandShowMessage(string[] line) {
        ShowWindowCanvas(generalSystem);
        StartCoroutine(ShowMessage(FixMessage(line[0])));
        DebugWindow.instance.DFDebug(line[0]);
    }

    //メッセージの編集
    string FixMessage(string line) {
        line = line.Replace("】", "】\n");
        return line;
    }

    //メッセージを1文字ずつ表示する
    IEnumerator ShowMessage(string message) {

        for (int i = 0; i < message.Length; i++) {
            messageText.text += message[i];
            generalSystem.PlayOneShot(audioSource, MESSAGE_TYPE);
            yield return new WaitForSeconds(messageSpeed);
        }
        //会話が終わった
        isNowLineExecuting = false;
    }

    //メッセージキャンバス作成
    void ShowWindowCanvas(GeneralSystem generalSystem) {
        if (messageText == null) {
            Vector3 addPos = new Vector3(0, -0.2f, 0);
            Vector3 pos = generalSystem.GetPosBetweenTargetAndFace(gameObject, addPos);
            messageText = scenarioSystem.ShowMessageWindow(
                pos, 
                generalSystem.GetQuaternionFace());
        }
        else {
            messageText.text = "";
        }
    }

    //メッセージキャンバスクローズ
    void CloseWindowCanvas() {
        messageText = null;
        scenarioSystem.CloseMessageWindow();
        /*
        Destroy(messageCanvas.gameObject);
        messageCanvas = null;
        */
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
        Vector3 addPos = gameObject.transform.forward;
        addPos.x *= 0.5f;
        addPos.y = MESSAGE_Y + SELECTBOX_Y - (float)boxCount * 0.35f;
        addPos.z *= 0.5f;
        GameObject selectBoxCanvas = generalSystem.MakeInstanceFromTarget(
            gameObject,
            scenarioSystem.GetSelectBoxCanvasPrefab(),
            addPos);
        return selectBoxCanvas;
    }

    void DeleteSelectBox() {
        foreach(GameObject selectBox in this.listSelectBoxCanvas) {
            Destroy(selectBox);
        }
        this.listSelectBoxCanvas.Clear();
    }

    //slectが実行された時
    public void ExecSelect(string flagName) {
        DebugWindow.instance.DFDebug("select flag name:" + flagName);
        int no = CommandGoto(flagName, this.listScenarioCsv);
        DebugWindow.instance.DFDebug("line:" + no);
        DeleteSelectBox();
        lineNo = exec(no);
    }

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

    void CommandLookAt() {
        generalSystem.LookAt(generalSystem.GetPlayerPrefab(), gameObject);
    }

    //スイッチの数字の計算。+と-のみ。
    //コマンド,スイッチNo,符号,値
    void CommandSwitchCalcuration(string[] line) {
        int swno = int.Parse(line[1]);
        string sign = line[2];
        int value = int.Parse(line[3]);
        scenarioSystem.CalcurationSwitch(swno, sign, value);
    }

    //スイッチの値のセット。
    //コマンド,スイッチNo,値
    void CommandSwitchSet(string[] line) {
        int swno = int.Parse(line[1]);
        int value = int.Parse(line[2]);
        scenarioSystem.SetSwitch(swno, value);
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
        listScenarioCsv = FQCommon.Common.LoadCsvFileFromTextAsset(scenario);
    }

    //実行可能なswitch行を探す。なかった場合は未実装。想定ナシ。
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
    //1=1,2=1のようにswitchno = valueで書く。今は=のみ。
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
