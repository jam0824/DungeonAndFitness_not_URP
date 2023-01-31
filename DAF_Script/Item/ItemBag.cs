using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBag : MonoBehaviour
{
    public string itemNo { set; get; }
    
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

    string itemNameDescription;

    private void Awake() {
        ovrGrabbable = GetComponent<OVRGrabbable>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        ItemBagDescriptionObject.transform.rotation = r;
    }

    void Grabing() {
        //‚Í‚¶‚ß‚Ä’Í‚Ü‚ê‚½Žž
        if (itemNo == null) ItemBagInit();

        if(itemDescription.enabled == false) {
            itemDescription.enabled = true;
        }

        
    }

    void ItemBagInit() {
        //’Í‚Ü‚ê‚½Žž‚É‰½‚ÌƒAƒCƒeƒ€‚©Œˆ’è‚·‚é
        itemNo = dungeonSystem.GetItemNo();
        labelSystem = GameObject.Find("GeneralSystem").GetComponent<LabelSystem>();
        playerView = GameObject.Find("Player").GetComponent<PlayerView>();
        label = labelSystem.GetLabel(labelKey);

        itemData = dungeonSystem.itemDb.GetItemData(itemNo);
        itemNameDescription = MakeItemNameRank(itemData);
        itemDescription.text = AddLabel(itemNameDescription);
        face = GameObject.Find("HitArea");
    }

    string AddLabel(string itemNameDescription) {
        return itemNameDescription + "<br><br>" + label;
    }

    string MakeItemNameRank(Dictionary<string, string> itemData) {
        string returnData = itemData["Name"] + "\n";
        returnData += "Rank : " + itemData["Rank"];
        return returnData;
    }

    public void DestroyItem() {
        Destroy(gameObject);
    }
}
