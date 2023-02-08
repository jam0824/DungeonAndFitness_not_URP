using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSystem : MonoBehaviour
{
    public bool DEBUG_MODE;
    public string NORMAL_ITEM_SAVE_PATH;
    public string COLLECTION_ITEM_SAVE_PATH;
    public GameObject ItemCanvas;
    public ItemView itemWindow { set; get; }
    public GameObject ItemBoxPrefab;
    public ItemBox itemBox { set; get; }
    public LabelSystem labelSystem { set; get; }

    public GameObject DamageTextCanvas;
    public GameObject PlayerDamageTextCanvas;
    public GameObject NoticeTextCanvas;
    GameObject player;
    public PlayerConfig playerConfig { set; get; }
    public ItemDB itemDb { set; get; }



    public string LanguageMode { set; get; }


    void Awake() {
        //OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
        LanguageMode = "japanese";
        GeneralInit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //�S�̓I�ȏ�����
    void GeneralInit() {

        //Label�̃��[�h
        labelSystem = GetComponent<LabelSystem>();
        labelSystem.LabelSystemInit(LanguageMode);

        //ItemDB�̃��[�h
        itemDb = GetComponent<ItemDB>();
        itemDb.ItemDbInit(this);

        //ItemWindow�̎��O�쐬
        LoadItemWindow();

        //ItemBox�̎��O�쐬
        LoadItemBox();

    }


    public GameObject GetItemCanvas() {
        return ItemCanvas;
    }
    public GameObject GetPrefabDamageTextCanvas() {
        return DamageTextCanvas;
    }
    public GameObject GetPrefabPlayerDamageTextCanvas() {
        return PlayerDamageTextCanvas;
    }
    public GameObject GetPrefabNoticeTextCanvas() {
        return NoticeTextCanvas;
    }


    public string GetNormalItemSavePath() {
        return NORMAL_ITEM_SAVE_PATH;
    }
    public string GetCollectionItemSavePath() {
        return COLLECTION_ITEM_SAVE_PATH;
    }


    //���j���[������Ĕ�A�N�e�B�u�ɂ��Ă���
    void LoadItemWindow() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject ItemWindowObject = Instantiate(ItemCanvas, pos, transform.rotation);
        itemWindow = ItemWindowObject.GetComponent<ItemView>();
        itemWindow.ItemViewInit();
        ItemWindowObject.SetActive(false);
    }

    

    //�A�C�e���{�b�N�X������Ĕ�A�N�e�B�u�ɂ��Ă���
    void LoadItemBox() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject itemBoxObject = Instantiate(ItemBoxPrefab, pos, transform.rotation);
        itemBox = itemBoxObject.GetComponent<ItemBox>();
        itemBox.ItemBoxInit();
        itemBoxObject.SetActive(false);
        itemBox.UnableItemBox();
    }

}
