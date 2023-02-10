using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelAttach : MonoBehaviour
{
    public string LABEL_KEY;
    LabelSystem labelSystem;
    TextMeshPro labelText;

    // Start is called before the first frame update
    void Start()
    {
        labelSystem = SingletonGeneral.instance.labelSystem;
        labelText = GetComponent<TextMeshPro>();
        SetLabel(LABEL_KEY);
    }

    void SetLabel(string key) {
        labelText.text = labelSystem.GetLabel(key);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
