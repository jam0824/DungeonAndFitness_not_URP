using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLoad : MonoBehaviour
{
    ItemDB itemDB;
    List<ItemList> ItemListItem;

    // Start is called before the first frame update
    void Start()
    {


    }


    //アイテムをロードする
    public void LoadItem(
        int pageNo, 
        ItemDB itemDb, 
        ItemView itemView, 
        List<ItemList> itemListItem) 
    {
        Init(itemDb, itemView);
        ItemListItem = itemListItem;
        MakeItemList(ItemListItem, itemDb.GetPlayerItemList(), pageNo);
        itemView.ChangePagingText();
    }

    //コレクションデータをロード
    public void LoadCollection(
        int collectionPageNo, 
        ItemDB itemDb, 
        ItemView itemView, 
        List<ItemList> itemListItem) 
    {
        Init(itemDb, itemView);
        ItemListItem = itemListItem;
        List<bool> listHasCollection = GetListHasCollection(itemDb.GetPlayerCollectionList());
        MakeCollectionList(ItemListItem, listHasCollection, collectionPageNo);
        itemView.ChangePagingText();
    }

    void Init(ItemDB itemDb, ItemView iv) {
        ItemListItem = new List<ItemList>();
        itemDB = itemDb;
        ItemView itemView = iv;

    }

    //コレクションは一回全部falseで埋めた後、持っているものだけtrueにする
    List<bool> GetListHasCollection(List<string> playerCollectionList) {
        List<bool> listHasCollection = new List<bool>();
        for (int i = 0; i < 100; i++) {
            listHasCollection.Add(false);
        }
        foreach (string itemNo in playerCollectionList) {
            int no = int.Parse(itemNo) - 1;
            listHasCollection[no] = true;
        }
        return listHasCollection;
    }

    //表示されている10件のコレクションリストを作る
    void MakeCollectionList(List<ItemList> ItemListItem, List<bool> listHasCollection, int pageNo) {
        int addIndex = pageNo * 10;
        for (int i = 0; i < 10; i++) {
            int index = i + addIndex;
            if (listHasCollection[index] == false) {
                ItemListItem[i].SetItemName("No." + AddZeroString(index + 1) + " : ????");
                continue;
            }
            Dictionary<string, string> itemData = itemDB.GetItemData((index + 1).ToString());
            ItemListItem[i].SetItemName("No." + AddZeroString(index + 1) + " : " + itemData["Name"]);
            ItemListItem[i].SetItemData(itemData);
            ItemListItem[i].itemIndex = index;
            ItemListItem[i].type = "Collection";
        }
    }

    //ゼロ埋めした文字列の数字を返す
    public string AddZeroString(int no) {
        return no.ToString("000");
    }
    

    //表示されている10件のアイテムリストを作る
    void MakeItemList(List<ItemList> ItemListItem, List<string> playerItemList, int pageNo) {
        int addIndex = pageNo * 10;
        for (int i = 0; i < 10; i++) {
            int index = i + addIndex;
            //アイテム所持数を超えたら空白で埋める
            if (playerItemList.Count <= index) {
                ItemListItem[i].SetItemName("");
                continue;
            }
            string stringItemNo = playerItemList[index];
            Dictionary<string, string> itemData = itemDB.GetItemData(stringItemNo);
            ItemListItem[i].SetItemName(itemData["Name"]);
            ItemListItem[i].SetItemData(itemData);
            ItemListItem[i].itemIndex = index;
            ItemListItem[i].type = "Normal";
        }

    }
}
