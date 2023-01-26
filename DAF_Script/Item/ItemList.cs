using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    Dictionary<string, string> itemData;
    Text description;
    ItemView itemView;
    Text itemNameText;

    // Start is called before the first frame update
    void Start()
    {
        itemNameText = gameObject.GetComponent<Text>();
    }

    public void Init(ItemView targetItemView) {
        itemView = targetItemView;
        description = itemView.itemDescriptionText;
    }

    public void SetItemData(Dictionary<string, string> data) {
        itemData = new Dictionary<string, string>();
        itemData = data;
    }

    public void OnClick() {
        if (itemNameText.text == "") return;
        if (HasInString(itemNameText.text,"????")) return;
        description.text = MakeDescription(itemData);
        itemView.PlayOneShot("ItemSmallSelect");
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
