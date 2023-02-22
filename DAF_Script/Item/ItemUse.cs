using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// これを呼び出すとアイテムを使う
    /// </summary>
    /// <param name="itemData"></param>
    public void Use(Dictionary<string, string> itemData) {
        switch (itemData["Effect"]) {
            case "Satiation":
                UseSatiationItem(itemData);
                break;
        }
    }

    /// <summary>
    /// 満腹度の回復
    /// </summary>
    /// <param name="itemData"></param>
    void UseSatiationItem(Dictionary<string, string> itemData) {
        string key = "RecoverSatiation";
        float recover = float.Parse(itemData["EffectSize"]);
        PlayerView.instance.config.RecoverSatiation(recover);
        SingletonGeneral.instance.labelInformationText.SetInformationLabel(key);
    }
}
