using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScenario : MonoBehaviour
{
    public Dictionary<string, string> dictSwitch { set; get; }
    public string sceneAnchorName { set; get; } //シーン移動時の移動先Anchor名

    public void DataScenarioInit() {
        dictSwitch = new Dictionary<string, string>();
    }
}
