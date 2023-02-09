using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelInformationText : MonoBehaviour
{
    LabelSystem labelSystem;
    TextMeshPro textMeshProComponent;
    int displayCount = 0;
    int displaySecond = 5;
    int maxDisplayCount;

    // Start is called before the first frame update
    void Start()
    {
        LabelInformationTextInit();
    }

    void LabelInformationTextInit() {
        labelSystem = SingletonGeneral.instance.labelSystem;
        textMeshProComponent = GetComponent<TextMeshPro>();
        textMeshProComponent.enabled = false;
        maxDisplayCount = 72 * displaySecond;
    }

    //������Ăяo����Information���\�������
    public void SetInformationLabel(string key) {
        textMeshProComponent.enabled = true;
        textMeshProComponent.text = labelSystem.GetLabel(key);
        displayCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�\������Ă������莞�Ԃŏ���
        if (textMeshProComponent.enabled) {
            displayCount++;
            if (displayCount > maxDisplayCount) 
                textMeshProComponent.enabled = false;
        }
        
    }
}
