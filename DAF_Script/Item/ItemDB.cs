using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDB : MonoBehaviour
{
    GeneralSystem generalSystem;
    List<Dictionary<string, string>> itemDB = new List<Dictionary<string, string>>();
    public List<string> playerItemList { set; get; }
    public List<string> playerCollectionList { set; get; }

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

    public void ItemDbInit(GeneralSystem gs) {
        itemDB = MakeItemDB(
            FQCommon.Common.LoadCsvFile("ItemDB/ItemDB")
        );
        generalSystem = gs;
        LoadPlayerItems();
    }

    //プレイヤーのアイテムをファイルからロードする
    void LoadPlayerItems() {
        playerItemList = FQCommon.Common.LoadSaveFile(
            generalSystem.GetNormalItemSavePath());
        playerCollectionList = FQCommon.Common.LoadSaveFile(
            generalSystem.GetCollectionItemSavePath()) ;
    }

    //ItemDBを作成する
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

    //アイテムデータから文字列を作成し返す。
    public string MakeDescription(Dictionary<string, string> itemData) {
        string returnData = "Rank : " + itemData["Rank"] + "\n";
        returnData += itemData["Name"] + "\n\n";
        returnData += itemData["Description"];
        return returnData;
    }

    //アイテムが追加可能か判定する
    public bool canAddItem() {
        int max = 10 * generalSystem.playerConfig.GetMaxPageNo();
        return (playerItemList.Count < max) ? true : false;
    }
}
