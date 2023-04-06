using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBag : MonoBehaviour
{
    public string itemNo { set; get; }

    const string KANTEI_ITEM_NO = "1";
    const float SECOND_OF_DESCRIPTION_FADE = 4.0f;
    const float DEC_ALPHA = 1.0f / (SECOND_OF_DESCRIPTION_FADE * 72f);

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

    bool isGrabbedMe = false;

    


    private void Awake() {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //�ŏ����猈�܂��Ă���A�C�e���̏ꍇ
        if (DECIDED_ITEM_NO != "") 
            ItemBagInit(DECIDED_ITEM_NO);
    }

    // Update is called once per frame
    void Update()
    {
        //������Ă���Ƃ�
        if (ovrGrabbable.isGrabbed == false) {
            ResetState();
            return;
        }
        //�����͂�ł���Ƃ�
        Grabing();
        itemDescription = DecreaseColor(itemDescription);
        RotateDescription(face);
        playerView.SetEnableItemBox();
    }

    void ResetState() {
        if (isGrabbedMe == true) {
            isGrabbedMe = false;
            itemDescription = ResetColor(itemDescription);
            itemDescription.enabled = false;
        }
        return;
    }

    TextMeshPro ResetColor(TextMeshPro textMeshPro) {
        Color c = textMeshPro.color;
        c.a = 1.0f;
        textMeshPro.color = c;
        return textMeshPro;
    }

    void Grabing() {
        //�͂��߂Ē͂܂ꂽ��
        if (itemNo == null) ItemBagInit("-1");

        if (isGrabbedMe == false) {
            isGrabbedMe = true;
            itemDescription.enabled = true;
            itemDescription.text = MakeDescription(itemData, label);
            CheckGrabber();
        }
    }

    void RotateDescription(GameObject face) {
        if (itemDescription.enabled == false) return;

        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        ItemBagDescriptionObject.transform.rotation = r;
    }

    

    /// <summary>
    /// �������B
    /// ������������-1�̏ꍇ��dungeonSystem���烉���_����item��set
    /// </summary>
    /// <param name="no"></param>
    public void ItemBagInit(string no) {
        if(no == "-1") {
            //no��-1�������Ƃ��͒͂܂ꂽ���ɉ��̃A�C�e�������肷��
            itemNo = dungeonSystem.GetItemNo();
        }
        else {
            //-1�ȊO��������A�C�e��no�w��ō쐬����
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

    TextMeshPro DecreaseColor(TextMeshPro textMeshPro) {
        if (textMeshPro.color.a <= 0.0f) return textMeshPro;
        Color c = textMeshPro.color;
        c.a -= DEC_ALPHA;
        if (c.a < 0f) c.a = 0f;
        textMeshPro.color = c;
        return textMeshPro;
    }
}
