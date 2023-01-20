using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLoad : MonoBehaviour
{
    ItemDB itemDB;
    List<GameObject> ItemListItem;

    // Start is called before the first frame update
    void Start()
    {


    }

    void Init() {
        ItemListItem = new List<GameObject>();
        itemDB = GameObject.Find("GeneralSystem").GetComponent<ItemDB>();
        ItemView itemView = GameObject.Find("ItemList").GetComponent<ItemView>();
        GetItemListeItemText(itemView);

    }

    //�R���N�V�����f�[�^�����[�h
    public void LoadCollection(int collectionPageNo) {
        Init();
        List<string> playerCollectionList = FQCommon.Common.LoadTextFile("ItemDB/PlayerCollectionSave");
        List<bool> listHasCollection = GetListHasCollection(playerCollectionList);
        MakeCollectionList(ItemListItem, listHasCollection, collectionPageNo);
        GetComponent<ItemView>().ChangePagingText();
    }

    //�R���N�V�����͈��S��false�Ŗ��߂���A�����Ă�����̂���true�ɂ���
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

    //�\������Ă���10���̃R���N�V�������X�g�����
    void MakeCollectionList(List<GameObject> ItemListItem, List<bool> listHasCollection, int pageNo) {
        int addIndex = pageNo * 10;
        for (int i = 0; i < 10; i++) {
            int index = i + addIndex;
            if (listHasCollection[index] == false) {
                ItemListItem[i].GetComponent<Text>().text = "No." + AddZeroString(index + 1) + " : ????";
                continue;
            }
            Dictionary<string, string> itemData = itemDB.GetItemData((index + 1).ToString());
            ItemListItem[i].GetComponent<Text>().text = "No." + AddZeroString(index + 1) + " : " + itemData["Name"];
            ItemListItem[i].GetComponent<ItemList>().SetItemData(itemData);
        }
    }

    //�[�����߂���������̐�����Ԃ�
    public string AddZeroString(int no) {
        return no.ToString("000");
    }
    
    //�A�C�e�������[�h����
    public void LoadItem(int pageNo) {
        Init();
        List<string> playerItemList = FQCommon.Common.LoadTextFile("ItemDB/PlayerItemSave");
        MakeItemList(ItemListItem, playerItemList, pageNo);
        GetComponent<ItemView>().ChangePagingText();
    }

    

    //�A�C�e�����X�g���̊e�A�C�e�����\���pobject���擾
    void GetItemListeItemText(ItemView itemView) {
        for (int i = 0; i < 10; i++) {
            string name = "ItemListItemText" + i;
            GameObject itemList = GameObject.Find(name);
            itemList.GetComponent<ItemList>().Init(itemView);
            ItemListItem.Add(itemList);
        }
    }

    //�\������Ă���10���̃A�C�e�����X�g�����
    void MakeItemList(List<GameObject> ItemListItem, List<string> playerItemList, int pageNo) {
        int addIndex = pageNo * 10;
        for (int i = 0; i < 10; i++) {
            int index = i + addIndex;
            if (playerItemList.Count <= index) {
                ItemListItem[i].GetComponent<Text>().text = "";
                continue;
            }
            string stringItemNo = playerItemList[index];
            Dictionary<string, string> itemData = itemDB.GetItemData(stringItemNo);
            ItemListItem[i].GetComponent<Text>().text = itemData["Name"];
            ItemListItem[i].GetComponent<ItemList>().SetItemData(itemData);
        }

    }
}
