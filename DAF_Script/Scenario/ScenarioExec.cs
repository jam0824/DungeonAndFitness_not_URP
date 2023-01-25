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
    //���ꗗ�̃V�i���I���s����
    bool isNowScenarioExec = false;
    //��1�s���s����
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

    //��b�������ꍇ
    void CommandShowMessage(string[] line) {
        ShowWindowCanvas(generalSystem);
        StartCoroutine(ShowMessage(FixMessage(line[0])));
        DebugWindow.instance.DFDebug(line[0]);
    }

    //���b�Z�[�W�̕ҏW
    string FixMessage(string line) {
        line = line.Replace("�z", "�z\n");
        return line;
    }

    //���b�Z�[�W��1�������\������
    IEnumerator ShowMessage(string message) {

        for (int i = 0; i < message.Length; i++) {
            messageText.text += message[i];
            generalSystem.PlayOneShot(audioSource, MESSAGE_TYPE);
            yield return new WaitForSeconds(messageSpeed);
        }
        //��b���I�����
        isNowLineExecuting = false;
    }

    //���b�Z�[�W�L�����o�X�쐬
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

    //���b�Z�[�W�L�����o�X�N���[�Y
    void CloseWindowCanvas() {
        messageText = null;
        scenarioSystem.CloseMessageWindow();
        /*
        Destroy(messageCanvas.gameObject);
        messageCanvas = null;
        */
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

    //slect�����s���ꂽ��
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

    //�X�C�b�`�̐����̌v�Z�B+��-�̂݁B
    //�R�}���h,�X�C�b�`No,����,�l
    void CommandSwitchCalcuration(string[] line) {
        int swno = int.Parse(line[1]);
        string sign = line[2];
        int value = int.Parse(line[3]);
        scenarioSystem.CalcurationSwitch(swno, sign, value);
    }

    //�X�C�b�`�̒l�̃Z�b�g�B
    //�R�}���h,�X�C�b�`No,�l
    void CommandSwitchSet(string[] line) {
        int swno = int.Parse(line[1]);
        int value = int.Parse(line[2]);
        scenarioSystem.SetSwitch(swno, value);
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
        listScenarioCsv = FQCommon.Common.LoadCsvFileFromTextAsset(scenario);
    }

    //���s�\��switch�s��T���B�Ȃ������ꍇ�͖������B�z��i�V�B
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
    //1=1,2=1�̂悤��switchno = value�ŏ����B����=�̂݁B
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
