using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    ItemLoad itemLoad;
    ItemButtonAnimation itemButtonAnimation;
    GameObject itemList;
    GeneralSystem generalSystem;
    PlayerConfig playerConfig;
    Text itemListPagingText;
    AudioSource audioSource;
    string nowListName = "Backpack";
    int pageNo = 0;
    int collectionPageNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        itemLoad = GetComponent<ItemLoad>();
        itemButtonAnimation = GetComponent<ItemButtonAnimation>();
        itemButtonAnimation.Init();
        itemList = GameObject.Find("ItemList");
        playerConfig = GameObject.Find("Player").GetComponent<PlayerConfig>();
        itemListPagingText = GameObject.Find("ItemListPagingText").GetComponent<Text>();
        
        audioSource = GetComponent<AudioSource>();

        OnClickBackpack();
    }

    public void OnClickClose() {
        generalSystem.PlayOneShotNoAudioSource("ItemMenuClose");
        Destroy(gameObject);
    }

    public void OnClickItemListNext() {
        generalSystem.PlayOneShot(audioSource, "ItemSmallSelect");
        if (nowListName == "Backpack") {
            if (pageNo < playerConfig.GetMaxPageNo() - 1) {
                pageNo++;
            }
            else {
                pageNo = 0;
            }
            itemLoad.LoadItem(pageNo);
        }
        else if (nowListName == "Collection") {
            if (collectionPageNo < 9) {
                collectionPageNo++;
            }
            else {
                collectionPageNo = 0;
            }
            itemLoad.LoadCollection(collectionPageNo);
        }
    }

    public void OnClickItemListPrev() {
        generalSystem.PlayOneShot(audioSource, "ItemSmallSelect");
        if (nowListName == "Backpack") {
            if (pageNo > 0) {
                pageNo--;
            }
            else {
                pageNo = playerConfig.GetMaxPageNo() - 1;
            }
            itemLoad.LoadItem(pageNo);
        }
        else if (nowListName == "Collection") {
            if (collectionPageNo > 0) {
                collectionPageNo--;
            }
            else {
                collectionPageNo = 9;
            }
            itemLoad.LoadCollection(collectionPageNo);
        }
        
    }

    public void OnClickBackpack() {
        nowListName = "Backpack";
        itemList.SetActive(true);
        ChangeHeaderButtonActive(nowListName);
        itemLoad.LoadItem(pageNo);
    }

    public void OnClickCollection() {
        nowListName = "Collection";
        itemList.SetActive(true);
        ChangeHeaderButtonActive(nowListName);
        itemLoad.LoadCollection(collectionPageNo);
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
        generalSystem.PlayOneShot(audioSource, "ItemBigSelect");
        itemButtonAnimation.ChangeHeaderButtonsActive(nowListName);
    }



    public int GetPageNo() {
        return pageNo;
    }

    public void ChangePagingText() {
        if (nowListName == "Backpack") {
            itemListPagingText.text = (pageNo + 1) + "/" + playerConfig.GetMaxPageNo();
        }
        else if(nowListName == "Collection") {
            itemListPagingText.text = (collectionPageNo + 1) + "/10";
        }
    }

    public void PlayOneShot(string seName) {
        generalSystem.PlayOneShot(audioSource, seName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
