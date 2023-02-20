using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBag : MonoBehaviour
{
    public string itemNo { set; get; }

    string KANTEI_ITEM_NO = "1";
    public string DECIDED_ITEM_NO = "";
    OVRGrabbable ovrGrabbable;
    DungeonSystem dungeonSystem;
    LabelSystem labelSystem;
    Dictionary<string, string> itemData;
    GameObject face;
    PlayerView playerView;
    public GameObject ItemBagDescriptionObject;
    public TextMeshPro itemDescription;


    public string labelKey;
    string label;


    private void Awake() {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //最初から決まっているアイテムの場合
        if (DECIDED_ITEM_NO != "") 
            ItemBagInit(DECIDED_ITEM_NO);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ovrGrabbable.isGrabbed == false) {
            if (itemDescription.enabled == true) itemDescription.enabled = false;
            return;
        }
        
        Grabing();
        RotateDescription(face);
        playerView.SetEnableItemBox();
    }

    void RotateDescription(GameObject face) {
        if (itemDescription.enabled == false) return;

        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        ItemBagDescriptionObject.transform.rotation = r;
    }

    void Grabing() {
        //はじめて掴まれた時
        if (itemNo == null) ItemBagInit("-1");

        if(itemDescription.enabled == false) {
            itemDescription.enabled = true;
            itemDescription.text = MakeDescription(itemData, label);
            CheckGrabber();
        }

        
    }

    /// <summary>
    /// 初期化。
    /// 引数が文字列-1の場合はdungeonSystemからランダムでitemをset
    /// </summary>
    /// <param name="no"></param>
    public void ItemBagInit(string no) {
        if(no == "-1") {
            //noが-1だったときは掴まれた時に何のアイテムか決定する
            itemNo = dungeonSystem.GetItemNo();
        }
        else {
            //-1以外だったらアイテムno指定で作成する
            itemNo = no;
        }

        labelSystem = SingletonGeneral.instance.labelSystem;
        playerView = GameObject.Find("Player").GetComponent<PlayerView>();
        label = labelSystem.GetLabel(labelKey);
        itemData = SingletonGeneral.instance.itemDb.GetItemData(itemNo);

        face = GameObject.Find("HitArea");
    }

    void CheckGrabber() {
        OVRGrabber grabber = ovrGrabbable.grabbedBy;
        SingletonGeneral.instance.VivrationController(grabber.gameObject.name, 0.2f, 0.2f, 0.1f);
        //DebugWindow.instance.DFDebug("hand:" + grabber.gameObject.name);
    }

    string MakeDescription(Dictionary<string, string> itemData, string label) {
        string returnString = "";
        if (SingletonGeneral.instance.itemDb.HasItem(KANTEI_ITEM_NO)) {
            string itemNameDescription = MakeItemNameRank(itemData);
            returnString = itemNameDescription + "<br><br>" + label;
        }
        else {
            returnString = label;
        }
        return returnString;
    }

    string MakeItemNameRank(Dictionary<string, string> itemData) {
        string returnData = itemData["Name"] + "\n";
        returnData += "Rank : " + itemData["Rank"] + "\n";
        returnData += itemData["Description"];
        return returnData;
    }

    public void DestroyItem() {
        Destroy(gameObject);
    }
}
