using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScenario : MonoBehaviour
{
    public Dictionary<string, string> dictSwitch { set; get; }

    public void DataScenarioInit() {
        dictSwitch = new Dictionary<string, string>();
    }
}
