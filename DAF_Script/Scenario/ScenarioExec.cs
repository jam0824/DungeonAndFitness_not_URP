using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScenarioExec : MonoBehaviour
{
    public bool isStartCheck = false;
    public TextAsset scenario;
    public List<GameObject> useObjects;

    //アイテムがフルだったときのキー
    string FULL_OF_ITEM_KEY = "FullOfItem";
    string MESSAGE_SE_KEY = "MessageNormal";

    float FADE_TIME = 0.5f;

    public ScenarioSystem scenarioSystem { set; get; }
    public AudioSource audioSource { set; get; }

    LabelFeelIcon labelFeelIcon;
    List<string[]> listScenarioCsv;
    List<GameObject> listSelectBoxCanvas;

    int lineNo = 0;
    //今一覧のシナリオ実行中か
    bool isNowScenarioExec = false;
    //今1行実行中か
    bool isNowLineExecuting = false;
    bool isLookAt = false;

    //スタート時にオートでスタートしたイベントか
    bool isAutoStart { set; get; }
    

    TextMeshProUGUI messageText;
    
    float messageSpeed = 0.05f;

    string nowMessage = "";

    bool isMessageTop = false;

    ScenarioCommand scenarioCommand = new ScenarioCommand();


    // Start is called before the first frame update
    void Start()
    {
        //シーンスタート時に起動する場合
        if (isStartCheck) {
            scenarioSystem = SingletonGeneral.instance.scenarioSystem;
            InitMain(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLookAt) scenarioCommand.CommandLookAt(gameObject);
           
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
        scenarioSystem.SetLock(true);
        InitMain(false);
    }
    void InitMain(bool isAuto) {
        isAutoStart = isAuto;
        LoadScenario();
        lineNo = GetScenarioLineNo();
        isNowScenarioExec = true;
        lineNo = exec(lineNo);
    }

    //行の実行。会話以外は会話などになるまで実行。
    public int exec(int no) {
        isNowLineExecuting = true;
        while (true) {
            DebugWindow.instance.DFDebug("LineNo:" + no);
            string[] line = listScenarioCsv[no];
            string command = line[0].ToLower();
            
            if (command == "") {
                no++;
                continue;
            }
            else if (command[0] == '#') {
                no++;
                continue;
            }
            else if (command[0] == '/') {
                //コメントをログ表示
                DebugWindow.instance.DFDebug(line[0]);
                no++;
                continue;
            }
            else if (command == "look") {
                isLookAt = true;
                scenarioCommand.CommandLookAt(gameObject);
                //lookatはupdateで呼び出されるのでログはココで出す
                DebugWindow.instance.DFDebug("Look");
                no++;
                continue;
            }
            else if ((command == "lookfromcharacter")|| (command == "lookc")) {
                scenarioCommand.ComandLookFromCharacter(line[1]);
                no++;
                continue;
            }
            else if (command == "lookoff") {
                isLookAt = false;
                no++;
                continue;
            }
            else if (command == "lookofffromcharacter") {
                scenarioCommand.CommandLookOffFromCharacter(line[1]);
                no++;
                continue;
            }
            else if ((command == "lookcharactertocharacter")|| (command == "lookc2c")) {
                scenarioCommand.CommandLookCharaceterToCharacter(line[1], line[2]);
                no++;
                continue;
            }
            else if (command == "automessagetop") {
                isMessageTop = scenarioCommand.CommandAutoMessageTop(line[1]);
                no++;
                continue;
            }
            else if (command == "goto") {
                no = CommandGoto(line[1], listScenarioCsv);
                continue;
            }
            else if (command == "if") {
                no = CommandIf(line[1], line[2], line[3],listScenarioCsv);
                continue;
            }
            else if (command == "ifauto") {
                no = CommandIfAuto(line[1], line[2], isAutoStart, listScenarioCsv);
                continue;
            }
            else if (command == "case") {
                no = CommandCase(line[1], no, listScenarioCsv);
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
                scenarioCommand.CommandNormalItemGet(
                    line[1], 
                    SingletonGeneral.instance.itemDb,
                    FULL_OF_ITEM_KEY);
                no++;
                continue;
            }
            else if (command == "se") {
                scenarioCommand.CommandSe(line[1]);
                no++;
                continue;
            }
            else if (command == "changeobject") {
                scenarioCommand.CommandChangeObject(
                    line[1], 
                    useObjects, 
                    int.Parse(line[2]));
                no++;
                continue;
            }
            else if (command == "save") {
                scenarioCommand.CommandSave();
                no++;
                continue;
            }
            else if (command == "feel") {
                labelFeelIcon = CommandFeel(line[1], labelFeelIcon);
                no++;
                continue;
            }
            else if (command == "information") {
                scenarioCommand.CommandInformation(line[1]);
                no++;
                continue;
            }
            else if (command == "nocontroll") {
                scenarioCommand.CommandNoControll();
                no++;
                continue;
            }
            else if (command == "controll") {
                scenarioCommand.CommandControll();
                no++;
                continue;
            }
            else if (command == "face") {
                scenarioCommand.CommandFace(line[1], line[2]);
                no++;
                continue;
            }
            else if (command == "messagemove") {
                CommandMessageMove(line[1], line[2]);
                no++;
                continue;
            }
            else if (command == "animationset") {
                scenarioCommand.CommandAnimationSet(line);
                no++;
                continue;
            }
            else if (command == "destroyobject") {
                CommandDestroyObject(line[1]);
                no++;
                continue;
            }
            else if (command == "destroy") {
                CommandDestroy();
                break;
            }
            else if (command == "scene") {
                CommandScene(line);
                break;
            }
            else if (command == "move") {
                CommandMove(line[1], no);
                break;
            }
            else if (command == "charactermove") {
                CommandCharacterMove(line[1], line[2], no);
                break;
            }
            else if (command == "fadeout") {
                CommandFadeOut(no);
                break;
            }
            else if (command == "fadein") {
                CommandFadeIn(no);
                break;
            }
            else if (command == "wait") {
                CommandWait(line[1], no);
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
        ShowWindowCanvas();
        nowMessage = FixMessage(string.Join(",", line));
        if(isMessageTop) AutoMessageTop(nowMessage);
        StartCoroutine(ShowMessage(nowMessage));
        DebugWindow.instance.DFDebug(line[0]);
    }

    //オートメッセージトップがtrueだったら自動でMessageBoxをtopにする
    void AutoMessageTop(string nowMessage) {
        if((nowMessage.Contains("【フェイ】")) || (nowMessage.Contains("[Fei]"))) {
            CommandMessageMove("Fei", "Top");
            return;
        }
        else if ((nowMessage.Contains("【アリス】")) || (nowMessage.Contains("[Alice]"))) {
            CommandMessageMove("Alice", "Top");
            return;
        }
        else if ((nowMessage.Contains("【ソル】")) || (nowMessage.Contains("[Sol]"))) {
            CommandMessageMove("Sol", "Top");
            return;
        }
    }

    //メッセージの編集
    string FixMessage(string line) {
        line = line.Replace("】", "】\n");
        line = line.Replace("]", "]\n");
        line = line.Replace("<br>", "\n");
        line = line.Replace(":", "");
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

    // /////////////////////////////////////////////////////////////

    //select時の旗にはスクリプトでは#は付けない。飛ぶべきハタと区別がつかなくなるため
    void CommandSelect(int lineNo, List<string[]> scenario) {
        List<GameObject> listSelectBoxCanvas = new List<GameObject>();
        for (int i = 0; i < scenario.Count; i++) {
            string[] line = scenario[lineNo + i];
            if ((line[0] == "selectend")|| (line[0] == "")) break;
            string flagName = "#" + line[0];
            string message = MakeSelectMessage(line);
            GameObject selectBoxCanvas = MakeSelectBox(i);
            selectBoxCanvas.GetComponent<ScenarioSelectBox>().SetValues(gameObject, flagName, message, i+1);
            listSelectBoxCanvas.Add(selectBoxCanvas);
        }
        this.listSelectBoxCanvas = listSelectBoxCanvas;
    }

    //英語でカンマで区切られていた時のセレクトの処理
    string MakeSelectMessage(string[] line) {
        if (line.Length <= 2) return line[1];
        string message = line[1];
        for(int i = 2; i < line.Length; i++) {
            message += "," + line[i];
        }
        return message;
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

    /// <summary>
    /// if,条件,trueの時,falseの時
    /// 条件がヒットしたときはtrueFlagへ、違うときはfalseFlagに飛ぶ
    /// 条件は&でつなげることができる
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="trueFlag"></param>
    /// <param name="falseFlag"></param>
    /// <param name="scenario"></param>
    /// <returns></returns>
    int CommandIf(
        string condition,
        string trueFlag,
        string falseFlag,
        List<string[]> scenario) {
        int no = 0;
        string[] arrayCondition = condition.Split('&');
        bool isOk = scenarioSystem.isSwitch(new List<string>(arrayCondition));
        if (isOk) {
            no = CommandGoto(trueFlag, scenario);
        }
        else {
            no = CommandGoto(falseFlag, scenario);
        }
        return no;
    }

    /// <summary>
    /// ifauto,autoスタートの時,autoスタートじゃない時
    /// シーン読み込み時のシナリオ自動実行判断
    /// </summary>
    /// <param name="trueFlag"></param>
    /// <param name="falseFlag"></param>
    /// <param name="isAuto"></param>
    /// <param name="scenario"></param>
    /// <returns></returns>
    int CommandIfAuto(string trueFlag,string falseFlag,bool isAuto,List<string[]> scenario) {
        int no = 0;
        if (isAuto) {
            no = CommandGoto(trueFlag, scenario);
            DebugWindow.instance.DFDebug("CommandIfAuto:true:" + trueFlag);
        }
        else {
            no = CommandGoto(falseFlag, scenario);
            DebugWindow.instance.DFDebug("CommandIfAuto:false:" + falseFlag);
        }
        return no;
    }

    /// <summary>
    /// case,key名
    /// value,#hata1
    /// value,#hata2
    /// default,#hata_n
    /// </summary>
    /// <param name="key"></param>
    /// <param name="no"></param>
    /// <param name="scenario"></param>
    /// <returns></returns>
    int CommandCase(string key, int no, List<string[]> scenario) {
        int returnNo = 0;
        for (int i = no + 1; i < scenario.Count; i++) {
            string[] line = scenario[i];
            //空だったらブレイク
            if (line[0] == "") break;
            //defaultだったらそのままgoto
            if(line[0] == "default") {
                returnNo = CommandGoto(line[1], scenario);
                break;
            }
            //それ以外なら判定
            bool isOk = scenarioSystem.GetIsOneSwitch(key, line[0]);
            if (isOk) {
                returnNo = CommandGoto(line[1], scenario);
                break;
            }
        }
        return returnNo;
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
        if (line[1].Contains("=")) {
            line[1] = line[1].Replace(" ", "");
            string[] values = line[1].Split('=');
            scenarioSystem.SetSwitch(values[0], values[1]);
        }
        else {
            string key = line[1];
            string value = line[2];
            scenarioSystem.SetSwitch(key, value);
        }
    }

    /// <summary>
    /// FeelIconを出す
    /// Smile,Angry,Sad,Surprise,Tere
    /// </summary>
    /// <param name="iconKey"></param>
    public LabelFeelIcon CommandFeel(string iconKey, LabelFeelIcon labelFeelIcon) {
        if (labelFeelIcon == null) {
            GameObject feelObject = transform.Find("NpcSet/FeelIcon").gameObject;
            labelFeelIcon = feelObject.GetComponent<LabelFeelIcon>();
        }
        labelFeelIcon.SetIcon(iconKey, transform.rotation);
        return labelFeelIcon;
    }

    /// <summary>
    /// キャラクターの同じシーン内の移動。フェード付き
    /// </summary>
    /// <param name="targetObjectName"></param>
    /// <param name="no"></param>
    void CommandMove(string targetObjectName, int no) {
        DebugWindow.instance.DFDebug("移動：" + targetObjectName);
        StartCoroutine(Move(targetObjectName, no));
    }
    //コールチンでフェード処理
    IEnumerator Move(string targetObjectName, int no) {
        scenarioSystem.CameraC.GetComponent<OVRScreenFade>().FadeOut();
        yield return new WaitForSeconds(FADE_TIME);
        MoveToTargetObject(targetObjectName);
        scenarioSystem.CameraC.GetComponent<OVRScreenFade>().FadeIn();
        yield return new WaitForSeconds(FADE_TIME);
        //シナリオはbreakしてるので、フェードが終わった後に再度シナリオを呼び出す
        lineNo = exec(no + 1);
    }

    //Playerを指定したオブジェクトと同じ位置、角度で移動する
    void MoveToTargetObject(string targetObjectName) {
        GameObject targetObject = GameObject.Find(targetObjectName);
        Vector3 pos = targetObject.transform.position;
        DebugWindow.instance.DFDebug("taretPos:" + pos);
        GameObject player = GameObject.Find("Player");
        //キャラクターコントローラーを切らないと移動しないことがある
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = pos;
        player.transform.rotation = targetObject.transform.rotation;
        DebugWindow.instance.DFDebug("playerPos:" + GameObject.Find("Player").transform.position);
        player.GetComponent<CharacterController>().enabled = true;
    }

    /// <summary>
    /// scene,シーン名,(移動先Anchor)
    /// 指定した名前のシーンに切り替える
    /// フェード付き
    /// </summary>
    /// <param name="line"></param>
    public void CommandScene(string[] line) {
        string sceneName = line[1];
        if (line.Length == 3) SetAnchorName(line[2]);

        DebugWindow.instance.DFDebug("シーン切り替え：" + sceneName);
        OVRScreenFade ovrScreenFade = GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>();
        ovrScreenFade.FadeOut();
        StartCoroutine(ChangeScene(sceneName, FADE_TIME));
    }
    void SetAnchorName(string anchorName) {
        scenarioSystem.SetAnchorName(anchorName);
    }

    IEnumerator ChangeScene(string sceneName, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        ResetFlagMain();
        SceneManager.LoadScene(sceneName);
    }


    /// <summary>
    /// ScenarioExecがついたオブジェクトを消去する
    /// </summary>
    void CommandDestroy() {
        DebugWindow.instance.DFDebug("シナリオによるDestroy");
        CloseWindowCanvas();
        ResetFlagMain();
        Destroy(gameObject);
    }

    

    void CommandFadeOut(int no) {
        StartCoroutine(Fade(no, true));
    }
    void CommandFadeIn(int no) {
        StartCoroutine(Fade(no, false));
    }

    //コールチンでフェード処理
    IEnumerator Fade(int no, bool isFadeStart) {
        if (isFadeStart) {
            scenarioSystem.CameraC.GetComponent<OVRScreenFade>().FadeOut();
        }
        else {
            scenarioSystem.CameraC.GetComponent<OVRScreenFade>().FadeIn();
        }
        yield return new WaitForSeconds(FADE_TIME);
        //シナリオはbreakしてるので、フェードが終わった後に再度シナリオを呼び出す
        lineNo = exec(no + 1);
    }

    /// <summary>
    /// wait,waitTime
    /// </summary>
    /// <param name="stringWaitTime"></param>
    /// <param name="no"></param>
    void CommandWait(string stringWaitTime, int no) {
        float waitTime = float.Parse(stringWaitTime);
        StartCoroutine(WaitTime(waitTime, no));
    }

    IEnumerator WaitTime(float waitTime, int no) {
        yield return new WaitForSeconds(waitTime);
        //シナリオはbreakしてるので、フェードが終わった後に再度シナリオを呼び出す
        lineNo = exec(no + 1);
    }

    

    

    /// <summary>
    /// 指定したキャラクターをanchorまで移動させる
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="anchorName"></param>
    /// <param name="no"></param>
    void CommandCharacterMove(string characterName, string anchorName, int no) {
        GameObject anchor = GameObject.Find(anchorName);
        GameObject character = GameObject.Find(characterName);
        character.GetComponent<AnimationSystem>().MoveToObject(this, no, anchor);
        DebugWindow.instance.DFDebug(characterName + "移動開始。" + anchorName + "まで");
    }


    //キャラクター移動時、キャラクター側から呼ばれてストップする
    public void StopCharacterMove(int no) {
        DebugWindow.instance.DFDebug("キャラクター移動完了。シナリオ再開:" + (no + 1));
        //シナリオはbreakしてるので、フェードが終わった後に再度シナリオを呼び出す
        lineNo = exec(no + 1);
    }

    /// <summary>
    /// メッセージウィンドウを移動する
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="positionName"></param>
    void CommandMessageMove(string characterName, string positionName) {
        GameObject obj = GameObject.Find(characterName);
        Quaternion r = SingletonGeneral.instance.GetQuaternionLookAt(
            SingletonGeneral.instance.face, 
            obj);
        positionName = positionName.ToLower();
        Vector3 pos = new Vector3();
        switch (positionName) {
            case "center":
                pos = SingletonGeneral.instance.GetPosBetweenTargetAndFace(obj, Vector3.zero);
                scenarioSystem.MoveMessageWindow(pos, r);
                break;
            case "top":
                GameObject topObject = GameObject.Find(characterName + "Top");
                pos = topObject.transform.position;
                pos.y += 0.2f;
                scenarioSystem.MoveMessageWindow(pos, r);
                break;
        }
    }

    /// <summary>
    /// 指定したオブジェクトをdestroyする
    /// </summary>
    /// <param name="objName"></param>
    void CommandDestroyObject(string objName) {
        GameObject obj = GameObject.Find(objName);
        Destroy(obj);
    }

    //会話終了
    void CommandEnd() {
        CloseWindowCanvas();

        DebugWindow.instance.DFDebug("会話終了:" + gameObject.name);
        //会話終了後、判定と重なっているためすぐ次の話になる。コールチンで待ち時間を入れる
        StartCoroutine("ResetFlag");
    }

    //各種フラグリセット
    IEnumerator ResetFlag() {
        yield return new WaitForSeconds(0.5f);
        ResetFlagMain();
    }

    public void ResetFlagMain() {
        isNowLineExecuting = false;
        isNowScenarioExec = false;
        isLookAt = false;
        isMessageTop = false;
        messageText = null;
        PlayerView.instance.canControll = true;
        scenarioSystem.SetLock(false);
    }

    //シナリオファイルのロード。シナリオはprefabにアタッチされたもの。
    public void LoadScenario() {
        DebugWindow.instance.DFDebug("シナリオ呼び出し:" + gameObject.name);
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
