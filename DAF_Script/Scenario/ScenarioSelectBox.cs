using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSelectBox : MonoBehaviour
{
    GameObject character;
    string flag;

    public void SetValues(GameObject charGameObject, string flagName, string message) {
        character = charGameObject;
        this.flag = flagName;
        Transform box = transform.Find("SelectBoxText");
        box.gameObject.GetComponent<Text>().text = message;
    }

    public void OnClick() {
        ScenarioExec scenarioExec = character.GetComponent<ScenarioExec>();
        scenarioExec.ExecSelect(flag);
    }
}
