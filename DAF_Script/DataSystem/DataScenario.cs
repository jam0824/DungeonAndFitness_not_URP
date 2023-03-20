using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScenario : MonoBehaviour
{
    public Dictionary<string, string> dictSwitch { set; get; }
    public string sceneAnchorName { set; get; } //ƒV[ƒ“ˆÚ“®‚ÌˆÚ“®æAnchor–¼

    public void DataScenarioInit() {
        dictSwitch = new Dictionary<string, string>();
    }
}
