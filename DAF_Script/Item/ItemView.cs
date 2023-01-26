using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    ItemLoad itemLoad;
    ItemDB itemDb;
    //アイテム表示のそれぞれの項目
    List<GameObject> itemListItem;
    ItemButtonAnimation itemButtonAnimation;
    GameObject itemList;
    GeneralSystem generalSystem;
    PlayerConfig playerConfig;
    Text itemListPagingText;
    
    public Text itemDescriptionText { set; get; }
    string nowListName = "Backpack";
    int pageNo = 0;
    int collectionPageNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject generalSystemObject = GameObject.Find("GeneralSystem");
        generalSystem = generalSystemObject.GetComponent<GeneralSystem>();
        itemDb = generalSystem.GetComponent<ItemDB>();

        playerConfig = GameObject.Find("Player").GetComponent<PlayerConfig>();
        itemListPagingText = GameObject.Find("ItemListPagingText").GetComponent<Text>();
        itemDescriptionText = GameObject.Find("ItemDescriptionText").GetComponent<Text>();

        itemListItem = GetItemListeItemText(this);
        itemLoad = GetComponent<ItemLoad>();
        itemButtonAnimation = GetComponent<ItemButtonAnimation>();
        itemButtonAnimation.Init();
        itemList = GameObject.Find("ItemList");

        OnClickBackpack();
    }

    //アイテムリスト欄の各アイテム名表示用objectを取得
    List<GameObject> GetItemListeItemText(ItemView itemView) {
        List<GameObject> returnList = new List<GameObject>();
        for (int i = 0; i < 10; i++) {
            string name = "ItemListItemText" + i;
            GameObject itemList = GameObject.Find(name);
            itemList.GetComponent<ItemList>().Init(itemView);
            returnList.Add(itemList);
        }
        return returnList;
    }

    //アイテムウィンドウを閉じる
    public void OnClickClose() {
        generalSystem.PlayOneShotNoAudioSource("ItemMenuClose");
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void OnClickItemListNext() {
        generalSystem.PlayOneShotNoAudioSource("ItemSmallSelect");
        if (nowListName == "Backpack") {
            if (pageNo < playerConfig.GetMaxPageNo() - 1) {
                pageNo++;
            }
            else {
                pageNo = 0;
            }
            itemLoad.LoadItem(pageNo, itemDb, this, itemListItem);
        }
        else if (nowListName == "Collection") {
            if (collectionPageNo < 9) {
                collectionPageNo++;
            }
            else {
                collectionPageNo = 0;
            }
            itemLoad.LoadCollection(collectionPageNo, itemDb, this, itemListItem);
        }
    }

    public void OnClickItemListPrev() {
        generalSystem.PlayOneShotNoAudioSource("ItemSmallSelect");
        if (nowListName == "Backpack") {
            if (pageNo > 0) {
                pageNo--;
            }
            else {
                pageNo = playerConfig.GetMaxPageNo() - 1;
            }
            itemLoad.LoadItem(pageNo, itemDb, this, itemListItem);
        }
        else if (nowListName == "Collection") {
            if (collectionPageNo > 0) {
                collectionPageNo--;
            }
            else {
                collectionPageNo = 9;
            }
            itemLoad.LoadCollection(collectionPageNo, itemDb, this, itemListItem);
        }
        
    }

    public void OnClickBackpack() {
        nowListName = "Backpack";
        itemList.SetActive(true);
        ChangeHeaderButtonActive(nowListName);
        itemLoad.LoadItem(pageNo, itemDb, this, itemListItem);
    }

    public void OnClickCollection() {
        nowListName = "Collection";
        itemList.SetActive(true);
        ChangeHeaderButtonActive(nowListName);
        itemLoad.LoadCollection(collectionPageNo, itemDb, this, itemListItem);
    }

    public void OnClickStatus() {
        nowListName = "Status";
        itemList.SetActive(false);
        ChangeHeaderButtonActive(nowListName);
    }

    public void OnClickSystem() {
        nowListName = "System";
        itemList.SetActive(false);
        ChangeHeaderButtonActive(nowListName);
    }

    void ChangeHeaderButtonActive(string nowListName) {
        generalSystem.PlayOneShotNoAudioSource("ItemBigSelect");
        itemButtonAnimation.ChangeHeaderButtonsActive(nowListName);
    }



    public int GetPageNo() {
        return pageNo;
    }

    //上部の1/2みたいなページ表示
    public void ChangePagingText() {
        if (nowListName == "Backpack") {
            itemListPagingText.text = (pageNo + 1) + "/" + playerConfig.GetMaxPageNo();
        }
        else if(nowListName == "Collection") {
            itemListPagingText.text = (collectionPageNo + 1) + "/10";
        }
    }

    public void PlayOneShot(string seName) {
        generalSystem.PlayOneShotNoAudioSource(seName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
