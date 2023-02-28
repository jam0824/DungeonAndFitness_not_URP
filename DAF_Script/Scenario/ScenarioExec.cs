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
    //�A�C�e�����t���������Ƃ��̃L�[
    string FULL_OF_ITEM_KEY = "FullOfItem";
    string MESSAGE_SE_KEY = "MessageNormal";

    float FADE_TIME = 0.5f;

    public ScenarioSystem scenarioSystem { set; get; }
    public AudioSource audioSource { set; get; }

    LabelFeelIcon labelFeelIcon;
    List<string[]> listScenarioCsv;
    List<GameObject> listSelectBoxCanvas;

    int lineNo = 0;
    //���ꗗ�̃V�i���I���s����
    bool isNowScenarioExec = false;
    //��1�s���s����
    bool isNowLineExecuting = false;
    bool isLookAt = false;
    List<GameObject> listLookCharacter = new List<GameObject>();
    

    TextMeshProUGUI messageText;
    
    float messageSpeed = 0.05f;

    string nowMessage = "";


    // Start is called before the first frame update
    void Start()
    {
        //�V�[���X�^�[�g���ɋN������ꍇ
        if (isStartCheck) {
            scenarioSystem = SingletonGeneral.instance.scenarioSystem;
            InitMain();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLookAt) CommandLookAt(gameObject);
        if (listLookCharacter.Count != 0) LookFromObject(listLookCharacter);
           
    }

    //�V�i���I���s���ɂ�����Ă�
    public void ScenarioExecution() {
        //1�s���s���������牽�����Ȃ��B���b�Z�[�W�d���h��
        if (isNowLineExecuting) return;

        if (!isNowScenarioExec) {
            Init();
        }
        else {
            lineNo = exec(lineNo);
        }
    }

    //������
    public void Init() {
        //�S�̂ŃC�x���g���b�N�i�N���ƃC�x���g���j�������牽�����Ȃ�
        if (scenarioSystem.GetLock()) return;
        scenarioSystem.SetLock(true);
        InitMain();
    }
    void InitMain() {
        LoadScenario();
        lineNo = GetScenarioLineNo();
        isNowScenarioExec = true;
        lineNo = exec(lineNo);
    }

    //�s�̎��s�B��b�ȊO�͉�b�ȂǂɂȂ�܂Ŏ��s�B
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
            else if (command == "look") {
                isLookAt = true;
                CommandLookAt(gameObject);
                DebugWindow.instance.DFDebug("Look");
                no++;
                continue;
            }
            else if (command == "lookfromobject") {
                ComandLookFromObject(line[1]);
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
            else if (command == "if") {
                no = CommandIf(line[1], line[2], line[3],listScenarioCsv);
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
            else if (command == "feel") {
                CommandFeel(line[1]);
                no++;
                continue;
            }
            else if (command == "information") {
                CommandInformation(line[1]);
                no++;
                continue;
            }
            else if (command == "nocontroll") {
                CommandNoControll();
                no++;
                continue;
            }
            else if (command == "face") {
                CommandFace(line[1], line[2]);
                no++;
                continue;
            }
            else if (command == "messagemove") {
                CommandMessageMove(line[1], line[2]);
                no++;
                continue;
            }
            else if (command == "destroy") {
                CommandDestroy();
                break;
            }
            else if (command == "scene") {
                CommandScene(line[1]);
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
            else {
                CommandShowMessage(line);
                break;
            }
        }
        return no + 1;
    }

    //��b�������ꍇ
    void CommandShowMessage(string[] line) {
        ShowWindowCanvas();
        nowMessage = FixMessage(string.Join(",", line));
        StartCoroutine(ShowMessage(nowMessage));
        DebugWindow.instance.DFDebug(line[0]);
    }

    //���b�Z�[�W�̕ҏW
    string FixMessage(string line) {
        line = line.Replace("�z", "�z\n");
        line = line.Replace("]", "]\n");
        line = line.Replace("<br>", "\n");
        line = line.Replace(":", "");
        return line;
    }

    //���b�Z�[�W��1�������\������
    IEnumerator ShowMessage(string message) {

        for (int i = 0; i < message.Length; i++) {
            if (messageText == null) break;
            if (messageText.text == message) break;
            messageText.text += message[i];
            SingletonGeneral.instance.PlayOneShot(audioSource, MESSAGE_SE_KEY);
            yield return new WaitForSeconds(messageSpeed);
        }
        //��b���I�����
        isNowLineExecuting = false;
    }

    /// <summary>
    /// ���b�Z�[�W�̈ꊇ�\��
    /// </summary>
    /// <param name="message"></param>
    public void ShowMessageInstantly() {
        if(isNowLineExecuting == false) return;
        if (messageText == null) return;
        StopCoroutine(ShowMessage(""));
        messageText.text = nowMessage;
    }

    //���b�Z�[�W�L�����o�X�쐬
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

    //���b�Z�[�W�L�����o�X�N���[�Y
    void CloseWindowCanvas() {
        messageText = null;
        scenarioSystem.CloseMessageWindow();
    }

    //select���̊��ɂ̓X�N���v�g�ł�#�͕t���Ȃ��B��Ԃׂ��n�^�Ƌ�ʂ����Ȃ��Ȃ邽��
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

    //SELECTBOX�쐬
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
    /// �S�ẴZ���N�g�{�b�N�X���A�N�e�B�u�ɂ���
    /// </summary>
    void UnenableAllSelectBox() {
        SingletonGeneral.instance.scenarioSystem.UnenableAllSelectBox();
    }

    //slect�����s���ꂽ��
    public void ExecSelect(string flagName) {
        DebugWindow.instance.DFDebug("select flag name:" + flagName);
        int no = CommandGoto(flagName, this.listScenarioCsv);
        DebugWindow.instance.DFDebug("line:" + no);
        UnenableAllSelectBox();
        lineNo = exec(no);
    }

    //�w�肵�����x���ɔ��
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
    /// �������q�b�g�����Ƃ���trueFlag�ցA�Ⴄ�Ƃ���falseFlag�ɔ��
    /// ������&�łȂ��邱�Ƃ��ł���
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

    //Player������悤�ɂ���
    void CommandLookAt(GameObject obj) {
        SingletonGeneral.instance.LookAt(
            SingletonGeneral.instance.face, 
            obj);
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g��Player������悤�ɂ���
    /// </summary>
    /// <param name="objName"></param>
    void ComandLookFromObject(string objName) {
        GameObject obj = GameObject.Find(objName);
        listLookCharacter.Add(obj);
        DebugWindow.instance.DFDebug("LookFromObject : " + objName);
    }

    //�o�^����Ă���S�L�����N�^�[��Player������悤�ɂ���
    void LookFromObject(List<GameObject> listLookCharacter) {
        foreach(GameObject obj in listLookCharacter) {
            CommandLookAt(obj);
        }
    }

    //�X�C�b�`�̐����̌v�Z�B+��-�̂݁B
    //�R�}���h,�X�C�b�`No,����,�l
    void CommandSwitchCalcuration(string[] line) {
        string key = line[1];
        string sign = line[2];
        string value = line[3];
        scenarioSystem.CalcurationSwitch(key, sign, value);
    }

    //�X�C�b�`�̒l�̃Z�b�g�B
    //�R�}���h,�X�C�b�`No,�l
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

    

    //�ʏ�g�̃A�C�e����ǉ�
    void CommandNormalItemGet(string itemNo, ItemDB itemDb) {
        if (itemDb.canAddItem(itemNo)) {
            DebugWindow.instance.DFDebug("�A�C�e���ǉ��F" + itemNo);
            itemDb.AddItem(itemNo);
        }
        else {
            SingletonGeneral.instance.labelInformationText.SetInformationLabel(FULL_OF_ITEM_KEY);
            Vector3 pos = gameObject.transform.position;
            pos.y += 1f;
            itemDb.MakeItemBag(itemNo, pos, gameObject.transform.rotation);
            DebugWindow.instance.DFDebug("�A�C�e���o�b�O�쐬�F" + itemNo);
        }
    }

    //SE��炷
    void CommandSe(string seName) {
        DebugWindow.instance.DFDebug("SE:" + seName);
        SingletonGeneral.instance.PlayOneShotNoAudioSource(seName);
    }

    /// <summary>
    /// Save����
    /// </summary>
    void CommandSave() {
        DebugWindow.instance.DFDebug("SAVE");
        SingletonGeneral.instance.saveLoadSystem.Save();
        SingletonGeneral.instance.labelInformationText.SetInformationLabel("Save");
    }

    /// <summary>
    /// FeelIcon���o��
    /// Smile,Angry,Sad,Surprise,Tere
    /// </summary>
    /// <param name="iconKey"></param>
    void CommandFeel(string iconKey) {
        if(labelFeelIcon == null) {
            GameObject feelObject = transform.Find("NpcSet/FeelIcon").gameObject;
            labelFeelIcon = feelObject.GetComponent<LabelFeelIcon>();
        }
        labelFeelIcon.SetIcon(iconKey, transform.rotation);
    }

    /// <summary>
    /// ��ʉ��̃C���t�H���[�V�����Ƀ��b�Z�[�W��\������
    /// </summary>
    /// <param name="labelKey"></param>
    void CommandInformation(string labelKey) {
        SingletonGeneral.instance.labelInformationText.SetInformationLabel(labelKey);
    }

    /// <summary>
    /// ScenarioExec�������I�u�W�F�N�g����������
    /// </summary>
    void CommandDestroy() {
        DebugWindow.instance.DFDebug("�V�i���I�ɂ��Destroy");
        CloseWindowCanvas();
        ResetFlagMain();
        Destroy(gameObject);
    }

    /// <summary>
    /// �w�肵�����O�̃V�[���ɐ؂�ւ���
    /// �t�F�[�h�t��
    /// </summary>
    /// <param name="sceneName"></param>
    void CommandScene(string sceneName) {
        DebugWindow.instance.DFDebug("�V�[���؂�ւ��F" + sceneName);
        OVRScreenFade ovrScreenFade = GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>();
        ovrScreenFade.FadeOut();
        StartCoroutine(ChangeScene(sceneName, FADE_TIME));
    }

    IEnumerator ChangeScene(string sceneName, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        ResetFlagMain();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// �L�����N�^�[�̓����V�[�����̈ړ��B�t�F�[�h�t��
    /// </summary>
    /// <param name="targetObjectName"></param>
    /// <param name="no"></param>
    void CommandMove(string targetObjectName, int no) {
        DebugWindow.instance.DFDebug("�ړ��F" + targetObjectName);
        StartCoroutine(Move(targetObjectName, no));
    }
    //�R�[���`���Ńt�F�[�h����
    IEnumerator Move(string targetObjectName, int no) {
        scenarioSystem.CameraC.GetComponent<OVRScreenFade>().FadeOut();
        yield return new WaitForSeconds(FADE_TIME);
        MoveToTargetObject(targetObjectName);
        scenarioSystem.CameraC.GetComponent<OVRScreenFade>().FadeIn();
        yield return new WaitForSeconds(FADE_TIME);
        //�V�i���I��break���Ă�̂ŁA�t�F�[�h���I�������ɍēx�V�i���I���Ăяo��
        lineNo = exec(no + 1);
    }

    //Player���w�肵���I�u�W�F�N�g�Ɠ����ʒu�A�p�x�ňړ�����
    void MoveToTargetObject(string targetObjectName) {
        GameObject targetObject = GameObject.Find(targetObjectName);
        Vector3 pos = targetObject.transform.position;
        DebugWindow.instance.DFDebug("taretPos:" + pos);
        GameObject player = GameObject.Find("Player");
        //�L�����N�^�[�R���g���[���[��؂�Ȃ��ƈړ����Ȃ����Ƃ�����
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = pos;
        player.transform.rotation = targetObject.transform.rotation;
        DebugWindow.instance.DFDebug("playerPos:" + GameObject.Find("Player").transform.position);
        player.GetComponent<CharacterController>().enabled = true;
    }

    /// <summary>
    /// Player�̃R���g���[���[�ł̈ړ����֎~����
    /// </summary>
    void CommandNoControll() {
        PlayerView.instance.canControll = false;
        DebugWindow.instance.DFDebug("Player�̑��얳����");
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g���̕\���ς���
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="emotion"></param>
    void CommandFace(string characterName, string emotion) {
        //�\��͑啶���ŁB
        emotion = emotion.ToUpper();

        GameObject.Find(characterName)
            .GetComponent<AnimationSystem>()
            .SetFace(characterName, emotion);
        DebugWindow.instance.DFDebug("�\��ύX:" + characterName + "->" + emotion);
    }

    /// <summary>
    /// �w�肵���L�����N�^�[��anchor�܂ňړ�������
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="anchorName"></param>
    /// <param name="no"></param>
    void CommandCharacterMove(string characterName, string anchorName, int no) {
        GameObject anchor = GameObject.Find(anchorName);
        GameObject character = GameObject.Find(characterName);
        character.GetComponent<AnimationSystem>().MoveToObject(this, no, anchor);
        DebugWindow.instance.DFDebug(characterName + "�ړ��J�n�B" + anchorName + "�܂�");
    }


    //�L�����N�^�[�ړ����A�L�����N�^�[������Ă΂�ăX�g�b�v����
    public void StopCharacterMove(int no) {
        DebugWindow.instance.DFDebug("�L�����N�^�[�ړ������B�V�i���I�ĊJ:" + (no + 1));
        //�V�i���I��break���Ă�̂ŁA�t�F�[�h���I�������ɍēx�V�i���I���Ăяo��
        lineNo = exec(no + 1);
    }

    /// <summary>
    /// ���b�Z�[�W�E�B���h�E���ړ�����
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="positionName"></param>
    void CommandMessageMove(string characterName, string positionName) {
        GameObject obj = GameObject.Find(characterName);
        Quaternion r = SingletonGeneral.instance.GetQuaternionFace();
        positionName = positionName.ToLower();
        switch (positionName) {
            case "center":
                Vector3 pos = SingletonGeneral.instance.GetPosBetweenTargetAndFace(obj, Vector3.zero);
                scenarioSystem.MoveMessageWindow(pos, r);
                break;
        }
    }



    //��b�I��
    void CommandEnd() {
        CloseWindowCanvas();

        DebugWindow.instance.DFDebug("��b�I��");
        //��b�I����A����Əd�Ȃ��Ă��邽�߂������̘b�ɂȂ�B�R�[���`���ő҂����Ԃ�����
        StartCoroutine("ResetFlag");
    }

    //�e��t���O���Z�b�g
    IEnumerator ResetFlag() {
        yield return new WaitForSeconds(0.5f);
        ResetFlagMain();
    }

    void ResetFlagMain() {
        isNowLineExecuting = false;
        isNowScenarioExec = false;
        isLookAt = false;
        messageText = null;
        listLookCharacter = new List<GameObject>();
        PlayerView.instance.canControll = true;
        scenarioSystem.SetLock(false);
    }

    //�V�i���I�t�@�C���̃��[�h�B�V�i���I��prefab�ɃA�^�b�`���ꂽ���́B
    public void LoadScenario() {
        DebugWindow.instance.DFDebug("�V�i���I�Ăяo��");
        List<string[]> tsvData = FQCommon.Common.LoadTsvFileFromTextAsset(scenario);

        listScenarioCsv = GetLanguageText(tsvData, SingletonGeneral.instance.LanguageMode);
    }

    //���{��Ɖp��̓^�u��؂肳��Ă���B
    //�e����ł̃p�����[�^�[��csv��؂肳��Ă���
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


    //���s�\��switch�s��T���B�Ȃ������ꍇ��0(�ŏ��̍s)��Ԃ��B
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

    //switch�R�}���h���^�������ꍇ�B
    //key=1,key=1�̂悤��key = value�ŏ����B����=�ŃA���h�̂݁B
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
