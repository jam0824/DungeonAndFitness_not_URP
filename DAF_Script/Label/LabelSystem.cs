using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelSystem : MonoBehaviour
{
    public string languageMode { set; get; }
    List<Dictionary<string, string>> labelDB;
    string LABEL_DB_PATH = "LabelDB/LabelDB";



    // Start is called before the first frame update
    void Start()
    {
        
    }

    //èâä˙âª
    public void LabelSystemInit(string language) {
        languageMode = language;
        labelDB = LoadLabelDB(FQCommon.Common.LoadCsvFile(LABEL_DB_PATH));
    }

    //LabelDBÇÉçÅ[ÉhÇµÇƒdictionaryå^ÇÃlistÇ…ÇµÇƒï‘Ç∑
    List<Dictionary<string, string>> LoadLabelDB(List<string[]> csvDatas) {
        List<Dictionary<string, string>> labelDB = new List<Dictionary<string, string>>();
        foreach (string[] data in csvDatas) {
            if (data[0] == "key") continue;
            Dictionary<string, string> itemData = new Dictionary<string, string>();
            for (int i = 0; i < data.Length; i++) {
                itemData[csvDatas[0][i]] = data[i];
            }
            labelDB.Add(itemData);
        }
        return labelDB;
    }

    //keyÇ©ÇÁLanguageModeÇÃåæåÍÇÃÉâÉxÉãÇï‘Ç∑
    public string GetLabel(string key) {
        string returnData = "";
        foreach (Dictionary<string, string> data in labelDB) {
            if (data["key"] == key) {
                returnData = data[languageMode];
                break;
            }
        }
        return returnData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
