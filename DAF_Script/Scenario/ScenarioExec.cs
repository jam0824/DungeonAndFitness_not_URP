using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioExec : MonoBehaviour
{
    public TextAsset scenario;
    //�A�C�e�����t���������Ƃ��̃L�[
    string FULL_OF_ITEM_KEY = "FullOfItem";
    string MESSAGE_SE_KEY = "MessageNormal";

    public ScenarioSystem scenarioSystem { set; get; }
    public AudioSource audioSource { set; get; }

    List<string[]> listScenarioCsv;
    List<GameObject> listSelectBoxCanvas;

    int lineNo = 0;
    //���ꗗ�̃V�i���I���s����
    bool isNowScenarioExec = false;
    //��1�s���s����
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

        LoadScenario();
        lineNo = GetScenarioLineNo();
        isNowScenarioExec = true;
        scenarioSystem.SetLock(true);
        lineNo = exec(lineNo);
    }

    //�s�̎��s�B��b�ȊO�͉�b�ȂǂɂȂ�܂Ŏ��s�B
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

    //��b�������ꍇ
    void CommandShowMessage(string[] line) {
        ShowWindowCanvas();
        nowMessage = FixMessage(line[0]);
        //��b���������ꍇ��1�������\��
        if ((line[0][0] == '�y') || (line[0][0] == '[')) {
            StartCoroutine(ShowMessage(nowMessage));
        }
        else {
            ShowMessageInstantly();
            isNowLineExecuting = false;
        }
        
        DebugWindow.instance.DFDebug(line[0]);
    }

    //���b�Z�[�W�̕ҏW
    string FixMessage(string line) {
        line = line.Replace("�z", "�z\n");
        line = line.Replace("]", "]\n");
        line = line.Replace("<br>", "\n");
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

    //Player������悤�ɂ���
    void CommandLookAt() {
        SingletonGeneral.instance.LookAt(
            SingletonGeneral.instance.face, 
            gameObject);
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
        string key = line[1];
        string value = line[2];
        scenarioSystem.SetSwitch(key, value);
    }

    //�ʏ�g�̃A�C�e����ǉ�
    void CommandNormalItemGet(string itemNo, ItemDB itemDb) {
        if (itemDb.canAddItem()) {
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
        isNowLineExecuting = false;
        isNowScenarioExec = false;
        isLookAt = false;
        messageText = null;
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
