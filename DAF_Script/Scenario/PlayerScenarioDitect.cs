using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScenarioDitect : MonoBehaviour
{
    ScenarioSystem scenarioSystem;
    GameObject generalSystemObject;
    GameObject isNowScenarioExecPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OnTrigger���ꂽ���ɂ͂��߂�scenarioSystem���擾����
        //�i�I�u�W�F�N�g���}�b�v�ɂ����ς������find���܂��邱�ƂɂȂ邩��j
        if (scenarioSystem == null) return;

        //lock���������Ă���Ȃ�C�x���g���Ȃ̂ŁA�m�ۂ���prefab�̃C�x���g��i�߂�
        if (scenarioSystem.GetLock()) {
            if (isKeyDown()) {
                ScenarioExec scenarioExec = isNowScenarioExecPrefab.gameObject.GetComponent<ScenarioExec>();
                //1�s���s���ɏd���Ŏ��s�����Ȃ�
                if (!scenarioExec.GetIsNowLineExecuting()) {
                    scenarioExec.ScenarioExecution();
                }
                else {
                    //1�s���s����A�������ꂽ�烁�b�Z�[�W�̈ꊇ�\��������
                    scenarioExec.ShowMessageInstantly();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if ((other.gameObject.tag == "NPC") 
            ||(other.gameObject.tag == "Investigate")){
            //����̂݃I�u�W�F�N�g�̃��[�h
            if (scenarioSystem == null) ObjectLoad();
            //trigger��NPC�ŁA����lock���������Ă��Ȃ��Ȃ��b���Ȃ̂Ńg���K�[
            if (!scenarioSystem.GetLock()) {
                if (isKeyDown()) {
                    ExecuteScenario(other);
                }
            }
        }
    }

    /// <summary>
    /// �L�[�������ꂽ��true��Ԃ��B
    /// ���̃L�[�Ŕ������邩�܂Ƃ߂Ă郁�\�b�h
    /// </summary>
    /// <returns></returns>
    bool isKeyDown() {
        return 
            ((Input.GetKeyDown(KeyCode.A)) || (OVRInput.GetDown(OVRInput.RawButton.A)));
    }

    private void ObjectLoad() {
        scenarioSystem = SingletonGeneral.instance.scenarioSystem;
    }

    //���s�{��
    void ExecuteScenario(Collider other) {
        ScenarioExec scenarioExec = other.gameObject.GetComponent<ScenarioExec>();
        //1�s���s���ɏd���Ŏ��s�����Ȃ�
        if (!scenarioExec.GetIsNowLineExecuting()) {
            isNowScenarioExecPrefab = other.gameObject;
            scenarioExec.scenarioSystem = scenarioSystem;
            
            scenarioExec.audioSource = other.GetComponent<AudioSource>();
            other.gameObject.GetComponent<ScenarioExec>().ScenarioExecution();
        }
    }
}
