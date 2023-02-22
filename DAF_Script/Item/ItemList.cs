using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    //持っているアイテムリストでのアイテムの位置
    public int itemIndex { set; get; }
    public string type { set; get; }

    Dictionary<string, string> itemData;
    Text description;
    ItemView itemView;
    Text itemNameText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ItemListInit(ItemView targetItemView) {
        itemNameText = gameObject.GetComponent<Text>();
        itemView = targetItemView;
        description = itemView.itemDescriptionText;
    }

    public void SetItemData(Dictionary<string, string> data) {
        itemData = new Dictionary<string, string>();
        itemData = data;
    }

    public void SetItemName(string itemName) {
        itemNameText.text = itemName;
    }

    public void OnClick() {
        if (itemNameText.text == "") return;
        if (HasInString(itemNameText.text,"????")) return;

        OnClickInit();
        itemView.PlayOneShot("ItemSmallSelect");
    }

    void OnClickInit() {
        description.text = MakeDescription(itemData);
        //itemViewに選んだアイテムのデータを登録する
        itemView.selectedItemData = itemData;
        itemView.selectedItemIndex = itemIndex;
        if (type == "Normal") itemView.EnableItemUseButton();
    }

    bool HasInString(string targetString, string searchString) {
        return targetString.Contains(searchString);
    }

    string MakeDescription(Dictionary<string, string> itemData) {
        string returnData = "Rank : " + itemData["Rank"] + "\n";
        returnData += itemData["Name"] + "\n\n";
        returnData += itemData["Description"];
        return returnData;
    }
}
