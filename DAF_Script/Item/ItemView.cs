using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    ItemLoad itemLoad;
    ItemDB itemDb;
    ItemUse itemUse;
    //�A�C�e���\���̂��ꂼ��̍���
    List<ItemList> itemListItem;
    ItemButtonAnimation itemButtonAnimation;
    GameObject itemList;
    PlayerConfig playerConfig;
    Text itemListPagingText;

    //���I�����Ă���A�C�e��
    public Dictionary<string, string> selectedItemData { set; get; }
    public int selectedItemIndex { set; get; }

    public Text itemDescriptionText { set; get; }
    string nowListName = "Backpack";
    int pageNo = 0;
    int collectionPageNo = 0;

    // Start is called before the first frame update
    public void ItemViewInit() {
        itemDb = SingletonGeneral.instance.itemDb;

        playerConfig = GameObject.Find("Player").GetComponent<PlayerConfig>();
        itemListPagingText = GameObject.Find("ItemListPagingText").GetComponent<Text>();
        itemDescriptionText = GameObject.Find("ItemDescriptionText").GetComponent<Text>();

        itemListItem = GetItemListeItemText(this);
        itemLoad = GetComponent<ItemLoad>();
        itemUse = GetComponent<ItemUse>();
        itemButtonAnimation = GetComponent<ItemButtonAnimation>();
        itemButtonAnimation.Init();
        itemList = GameObject.Find("ItemList");

        LoadBackpack();
    }

    //��ԍŏ��̃��[�h�p�iSE���������Ȃ������j
    public void LoadBackpack() {
        nowListName = "Backpack";
        itemList.SetActive(true);
        itemButtonAnimation.ChangeHeaderButtonsActive(nowListName);
        itemLoad.LoadItem(pageNo, itemDb, this, itemListItem);
    }

    void Start()
    {
        
    }

    //�A�C�e�����X�g���̊e�A�C�e�����\���pobject���擾
    List<ItemList> GetItemListeItemText(ItemView itemView) {
        List<ItemList> returnList = new List<ItemList>();
        for (int i = 0; i < 10; i++) {
            string name = "ItemListItemText" + i;
            ItemList itemList = GameObject.Find(name).GetComponent<ItemList>();
            itemList.ItemListInit(itemView);
            returnList.Add(itemList);
        }
        return returnList;
    }

    //�A�C�e���E�B���h�E���J��
    public void EnableItemWindow(Vector3 pos, Quaternion r) {
        gameObject.SetActive(true);
        EnableItemWindowInit();
        gameObject.transform.position = pos;
        gameObject.transform.rotation = r;
    }

    //�A�C�e���E�B���h�E���J�����Ƃ��̏�����
    void EnableItemWindowInit() {
        nowListName = "Backpack";
        pageNo = 0;
        selectedItemData = null;
        selectedItemIndex = 0;
        OnClickBackpack();
    }

    //�A�C�e���E�B���h�E�����
    public void OnClickClose() {
        SingletonGeneral.instance.PlayOneShotNoAudioSource("ItemMenuClose");
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public bool ActiveSelf() {
        return gameObject.activeSelf;
    }

    

    public void OnClickItemListNext() {
        SingletonGeneral.instance.PlayOneShotNoAudioSource("ItemSmallSelect");
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
        SingletonGeneral.instance.PlayOneShotNoAudioSource("ItemSmallSelect");
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

    /// <summary>
    /// Use���������Ƃ��ɌĂ΂��
    /// </summary>
    public void OnClickUseButton() {
        if (selectedItemData == null) return;

        itemUse.Use(selectedItemData);
        itemDb.DeleteWithItemIndex(selectedItemIndex);
        RedrawItemList();
    }

    /// <summary>
    /// ItemList���ĕ`�悷��
    /// </summary>
    void RedrawItemList() {
        itemLoad.LoadItem(pageNo, itemDb, this, itemListItem);
        selectedItemData = null;
        selectedItemIndex = 0;
        itemDescriptionText.text = "";
    }

    void ChangeHeaderButtonActive(string nowListName) {
        SingletonGeneral.instance.PlayOneShotNoAudioSource("ItemBigSelect");
        itemButtonAnimation.ChangeHeaderButtonsActive(nowListName);
    }



    public int GetPageNo() {
        return pageNo;
    }

    //�㕔��1/2�݂����ȃy�[�W�\��
    public void ChangePagingText() {
        if (nowListName == "Backpack") {
            itemListPagingText.text = (pageNo + 1) + "/" + playerConfig.GetMaxPageNo();
        }
        else if(nowListName == "Collection") {
            itemListPagingText.text = (collectionPageNo + 1) + "/10";
        }
    }

    public void PlayOneShot(string seName) {
        SingletonGeneral.instance.PlayOneShotNoAudioSource(seName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
