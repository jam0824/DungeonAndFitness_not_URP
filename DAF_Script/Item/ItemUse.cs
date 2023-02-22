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
        //インフォメーションに表示
        SingletonGeneral.instance.labelInformationText.SetInformationLabel(key);
        SingletonGeneral.instance.PlayOneShotNoAudioSource("RecoverSmall");
        MakeEffect("RecoverSmall");
    }

    /// <summary>
    /// Effectを作成
    /// </summary>
    /// <param name="effectName"></param>
    void MakeEffect(string effectName) {
        GameObject prefab = SingletonGeneral.instance.GetEffect(effectName);
        Vector3 addPos = new Vector3(1f, 0.5f, 1f);
        SingletonGeneral.instance.MakeInstanceFromTarget(
            SingletonGeneral.instance.face, 
            prefab, 
            addPos);
    }
}
