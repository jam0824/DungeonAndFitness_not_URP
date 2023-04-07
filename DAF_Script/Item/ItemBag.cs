using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBag : MonoBehaviour
{

    public string itemNo { set; get; }
    public DungeonSystem dungeonSystem { get; set; }
    public LabelSystem labelSystem { get; set; }
    public PlayerView playerView { get; set; }
    [SerializeField] private GameObject itemBagDescriptionObject;
    [SerializeField] private TextMeshPro itemDescription;
    public string labelKey;

    const string KANTEI_ITEM_NO = "1";
    const string RANDOM_ITEM = "-1";
    const float SECOND_OF_DESCRIPTION_FADE = 4.0f;
    const float DEC_ALPHA = 1.0f / (SECOND_OF_DESCRIPTION_FADE * 72f);

    public string DECIDED_ITEM_NO = "";
    
    OVRGrabbable ovrGrabbable;
    
    Dictionary<string, string> itemData;
    GameObject face;
    
    string label;
    bool isGrabbedMe = false;

    


    private void Awake() {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        dungeonSystem = dungeonSystem ?? GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
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
        //離されているとき
        if (ovrGrabbable.isGrabbed == false) {
            ResetState();
            return;
        }
        //何か掴んでいるとき
        Grabbing();
        itemDescription = DecreaseColor(itemDescription);
        itemBagDescriptionObject.transform.rotation = RotateDescription(
            itemBagDescriptionObject.transform, 
            face.transform);
        playerView.SetEnableItemBox();
    }

    void ResetState() {
        if (isGrabbedMe == true) {
            isGrabbedMe = false;
            itemDescription = ResetColor(itemDescription);
            itemDescription.enabled = false;
        }
    }

    TextMeshPro ResetColor(TextMeshPro textMeshPro) {
        Color c = textMeshPro.color;
        c.a = 1.0f;
        textMeshPro.color = c;
        return textMeshPro;
    }

    void Grabbing() {
        //はじめて掴まれた時
        if (itemNo == null) ItemBagInit(RANDOM_ITEM);

        if (isGrabbedMe == false) {
            isGrabbedMe = true;
            itemDescription.enabled = true;
            itemDescription.text = MakeDescription(itemData, label);
            CheckGrabber();
        }
    }

    Quaternion RotateDescription(Transform textTransform, Transform faceTransform) {
        if (itemDescription.enabled == false) return textTransform.rotation;

        Quaternion r = Quaternion.Euler(
            0f, 
            faceTransform.rotation.eulerAngles.y, 
            0f);
        return r;
    }



    /// <summary>
    /// 初期化。
    /// 引数が文字列-1の場合はdungeonSystemからランダムでitemをset
    /// </summary>
    /// <param name="no"></param>
    public void ItemBagInit(string no) {
        //noが-1だったときは掴まれた時に何のアイテムか決定する
        //-1以外だったらアイテムno指定で作成する
        itemNo = (no == RANDOM_ITEM) ? dungeonSystem.GetItemNo() : no;

        labelSystem = labelSystem ?? SingletonGeneral.instance.labelSystem;
        playerView = playerView ?? GameObject.Find("Player").GetComponent<PlayerView>();
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

    TextMeshPro DecreaseColor(TextMeshPro textMeshPro) {
        if (textMeshPro.color.a <= 0.0f) return textMeshPro;
        Color c = textMeshPro.color;
        c.a = Mathf.Max(c.a - DEC_ALPHA, 0f);
        textMeshPro.color = c;
        return textMeshPro;
    }
}
