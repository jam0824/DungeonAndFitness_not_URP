using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDB : MonoBehaviour
{
    List<Dictionary<string, string>> itemDB = new List<Dictionary<string, string>>();

    private void Awake() {
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemDbInit() {
        itemDB = MakeItemDB(
            FQCommon.Common.LoadCsvFile("ItemDB/ItemDB")
        );
    }

    public List<Dictionary<string, string>> MakeItemDB(List<string[]> csvDatas) {
        List<Dictionary<string, string>> itemDB = new List<Dictionary<string, string>>();
        foreach (string[] data in csvDatas) {
            if (data[0] == "ItemNo") continue;
            Dictionary<string, string> itemData = new Dictionary<string, string>();
            for (int i = 0; i < data.Length; i++) {
                itemData[csvDatas[0][i]] = data[i];
            }
            itemDB.Add(itemData);
        }
        return itemDB;
    }

    //文字列型のアイテムナンバーからアイテムのデータを返す
    public Dictionary<string, string> GetItemData(string stringItemNo) {
        Dictionary<string, string> returnData = new Dictionary<string, string>();
        foreach (Dictionary<string, string> data in itemDB) {
            if (data["ItemNo"] == stringItemNo) {
                returnData = data;
                break;
            }
        }
        return returnData;
    }
}
