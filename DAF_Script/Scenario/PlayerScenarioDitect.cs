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
        //OnTriggerされた時にはじめてscenarioSystemを取得する
        //（オブジェクトがマップにいっぱいあるとfindしまくることになるから）
        if (scenarioSystem == null) return;

        //lockがかかっているならイベント中なので、確保したprefabのイベントを進める
        if (scenarioSystem.GetLock()) {
            if (isKeyDown()) {
                ScenarioExec scenarioExec = isNowScenarioExecPrefab.gameObject.GetComponent<ScenarioExec>();
                //1行実行中に重複で実行させない
                if (!scenarioExec.GetIsNowLineExecuting()) {
                    scenarioExec.ScenarioExecution();
                }
                else {
                    //1行実行中にAをおされたらメッセージの一括表示をする
                    scenarioExec.ShowMessageInstantly();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if ((other.gameObject.tag == "NPC") 
            ||(other.gameObject.tag == "Investigate")){
            //初回のみオブジェクトのロード
            if (scenarioSystem == null) ObjectLoad();
            //triggerがNPCで、かつlockがかかっていないなら会話初なのでトリガー
            if (!scenarioSystem.GetLock()) {
                if (isKeyDown()) {
                    ExecuteScenario(other);
                }
            }
        }
    }

    /// <summary>
    /// キーがおされたらtrueを返す。
    /// 何のキーで反応するかまとめてるメソッド
    /// </summary>
    /// <returns></returns>
    bool isKeyDown() {
        return 
            ((Input.GetKeyDown(KeyCode.A)) || (OVRInput.GetDown(OVRInput.RawButton.A)));
    }

    private void ObjectLoad() {
        scenarioSystem = SingletonGeneral.instance.scenarioSystem;
    }

    //実行本体
    void ExecuteScenario(Collider other) {
        ScenarioExec scenarioExec = other.gameObject.GetComponent<ScenarioExec>();
        //1行実行中に重複で実行させない
        if (!scenarioExec.GetIsNowLineExecuting()) {
            isNowScenarioExecPrefab = other.gameObject;
            scenarioExec.scenarioSystem = scenarioSystem;
            
            scenarioExec.audioSource = other.GetComponent<AudioSource>();
            other.gameObject.GetComponent<ScenarioExec>().ScenarioExecution();
        }
    }
}
