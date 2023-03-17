using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioSelectBox : MonoBehaviour
{
    GameObject character;
    string flag;
    int selectNo = 0;

    /// <summary>
    /// セレクトボックスに各種値を設定する
    /// </summary>
    /// <param name="charGameObject"></param>
    /// <param name="flagName"></param>
    /// <param name="message"></param>
    /// <param name="no"></param>
    public void SetValues(GameObject charGameObject, string flagName, string message, int no) {
        character = charGameObject;
        this.flag = flagName;
        this.selectNo = no;
        Transform box = transform.Find("SelectBoxText");
        box.gameObject.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void OnClick() {
        ScenarioExec scenarioExec = character.GetComponent<ScenarioExec>();
        scenarioExec.ExecSelect(flag);
    }

    private void Update() {
        KeyCheck();
    }

    void KeyCheck() {
        if (!gameObject.activeSelf) return;
        if (Input.GetKey(GetKeyCode(selectNo))) {
            OnClick();
        }
    }

    KeyCode GetKeyCode(int no){
        if (no == 1) return KeyCode.Alpha1;
        else if(no == 2) return KeyCode.Alpha2;
        else if (no == 3) return KeyCode.Alpha3;
        else if (no == 4) return KeyCode.Alpha4;
        else if (no == 5) return KeyCode.Alpha5;
        return KeyCode.Alpha1;
    }
}
