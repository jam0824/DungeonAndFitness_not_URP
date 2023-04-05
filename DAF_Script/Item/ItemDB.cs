using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemDB : MonoBehaviour
{
    int COLLECTION_MAX = 100;

    DataItem dataItem;

    public List<GameObject> itemPrefab;
    Dictionary<string, int> dictItemPrefabName;
    List<Dictionary<string, string>> itemDB = new List<Dictionary<string, string>>();

    private void Awake() {
        dataItem = DataSystem.instance.dataItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string> GetPlayerItemList() {
        return dataItem.playerItemList;
    }
    public List<string> GetPlayerCollectionList() {
        return dataItem.playerCollectionList;
    }

    public void ItemDbInit() {
        itemDB = MakeItemDB(
            FQCommon.Common.LoadCsvFile("ItemDB/ItemDB")
        );
        //LoadPlayerItems();
        dictItemPrefabName = SetDictItemPrefabName();
    }

    //プレイヤーのアイテムをファイルからロードする
    void LoadPlayerItems() {
        dataItem.playerItemList = FQCommon.Common.LoadSaveFile(
            SingletonGeneral.instance.GetNormalItemSavePath());
        dataItem.playerCollectionList = FQCommon.Common.LoadSaveFile(
            SingletonGeneral.instance.GetCollectionItemSavePath()) ;
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
    public bool canAddItem(string itemNo) {
        if (int.Parse(itemNo) < COLLECTION_MAX) return true;
        PlayerConfig playerConfig = GameObject.Find("Player").GetComponent<PlayerConfig>();
        int max = 10 * playerConfig.GetMaxPageNo();
        return (dataItem.playerItemList.Count < max) ? true : false;
    }

    //アイテムをアイテムリストに追加する
    public void AddItem(string itemNo) {
        if(int.Parse(itemNo) < COLLECTION_MAX) {
            dataItem.playerCollectionList.Add(itemNo);
        }
        else {
            dataItem.playerItemList.Add(itemNo);
        }
    }

    //アイテムバッグを指定アイテムNoで作成する
    public void MakeItemBag(string itemNo, Vector3 pos, Quaternion r) {
        Dictionary<string, string> itemData = GetItemData(itemNo);
        GameObject itemBagObject = Instantiate(
            GetItemPrefab(itemData["Prefab"]), 
            pos, 
            r);
        itemBagObject.transform.parent = SingletonGeneral.instance.dungeonRoot.transform;
        ItemBag itemBag = itemBagObject.GetComponent<ItemBag>();
        itemBag.ItemBagInit(itemNo);
    }

    //アイテムのkeyとItemPrefabの番号を紐づける
    Dictionary<string, int> SetDictItemPrefabName() {
        Dictionary<string, int> itemPrefabName = new Dictionary<string, int>();
        itemPrefabName.Add("bag", 0);
        itemPrefabName.Add("meat", 1);
        itemPrefabName.Add("greenpowder", 2);
        return itemPrefabName;
    }

    //Prefabの名前からアイテムのprefabを取得する
    GameObject GetItemPrefab(string prefabName) {
        int no = dictItemPrefabName[prefabName];
        return itemPrefab[no];
    }

    /// <summary>
    /// itemNoを持っていればtrueを返す
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    public bool HasItem(string itemNo) {
        if (dataItem.playerItemList.Contains(itemNo))
            return true;
        if (dataItem.playerCollectionList.Contains(itemNo))
            return true;
        return false;
    }

    /// <summary>
    /// アイテムリストのindexでアイテムを削除する
    /// </summary>
    /// <param name="index"></param>
    public void DeleteWithItemIndex(int index) {
        dataItem.playerItemList.RemoveAt(index);
    }
}
