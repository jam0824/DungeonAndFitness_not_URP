using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelView : MonoBehaviour
{
    public string LABEL_KEY;

    LabelSystem labelSystem;
    TextMeshPro labelTextObject;
    GameObject face;
    GameObject messageTextObject;
    string labelText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        LabelViewInit();
    }

    void LabelViewInit() {
        labelSystem = SingletonGeneral.instance.labelSystem;
        labelTextObject = transform.Find("LabelText").GetComponent<TextMeshPro>();
        face = GameObject.Find("HitArea");
    }

    void SetLabel(string key) {
        labelText = labelSystem.GetLabel(key);
        labelTextObject.text = labelText;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Event") {
            if (labelText == null) 
                SetLabel(LABEL_KEY);
            if (messageTextObject == null) 
                messageTextObject = SingletonGeneral.instance.scenarioSystem.GetMessageTextObject();
            //メッセージ表示中だったらラベルは出さない
            if (messageTextObject.activeSelf) {
                if(labelTextObject.enabled) 
                    labelTextObject.enabled = false;
                return;
            }

            if (labelTextObject.enabled == false) 
                labelTextObject.enabled = true;
            transform.LookAt(face.transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Event") {
            labelTextObject.enabled = false;
        }
    }
}
