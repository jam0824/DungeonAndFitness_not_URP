using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonAnimation : MonoBehaviour
{
    public List<Sprite> listHeaderButtonSprite;
    Dictionary<string, GameObject> dictGameObjectHeaderButton;

    public void Init() {
        dictGameObjectHeaderButton = new Dictionary<string, GameObject>();
        dictGameObjectHeaderButton["Backpack"] = GameObject.Find("BackpackImage");
        dictGameObjectHeaderButton["Collection"] = GameObject.Find("CollectionImage");
        dictGameObjectHeaderButton["Status"] = GameObject.Find("StatusImage");
        dictGameObjectHeaderButton["System"] = GameObject.Find("SystemImage");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeHeaderButtonsActive(string buttonName) {

        ChangeHeaderButtonsDisable();
        dictGameObjectHeaderButton[buttonName].GetComponent<Image>().sprite = listHeaderButtonSprite[1];
        Transform child = dictGameObjectHeaderButton[buttonName].transform.Find("Text");
        child.gameObject.GetComponent<Text>().color = GetActiveFontColor();

    }

    void ChangeHeaderButtonsDisable() {
        foreach (string key in dictGameObjectHeaderButton.Keys) {
            Debug.Log(key);
            dictGameObjectHeaderButton[key].GetComponent<Image>().sprite = listHeaderButtonSprite[0];
            Transform child = dictGameObjectHeaderButton[key].transform.Find("Text");
            child.gameObject.GetComponent<Text>().color = GetDisableFontColor();
        }
    }

    public Color GetActiveFontColor() {
        return new Color(
            180.0f / 255.0f,
            216.0f / 255.0f,
            253.0f / 255.0f
            );
    }

    public Color GetDisableFontColor() {
        return new Color(
            0.0f,
            162.0f / 255.0f,
            1.0f
            );
    }

    
}
