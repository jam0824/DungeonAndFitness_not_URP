using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public float WAIT_TIME_DELETE;
    GeneralSystem generalSystem;
    public GameObject itemHitObject;

    // Start is called before the first frame update
    void Start()
    {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        DebugWindow.instance.DFDebug("ÉgÉäÉKÅ[");
        if (other.gameObject.tag == "Item") {
            AddItem(other);
        }
    }

    void AddItem(Collider other) {
        ItemBag itemBag = other.gameObject.GetComponent<ItemBag>();
        if (itemBag.itemNo == null) return;

        SaveItem(itemBag.itemNo);
        itemBag.DestroyItem();
        StartCoroutine(DestroyItemBox(WAIT_TIME_DELETE));
    }

    void SaveItem(string itemNo) {
        string fileName = "";
        if (int.Parse(itemNo) >= 100) {
            fileName = generalSystem.NORMAL_ITEM_SAVE_PATH;
        }
        else {
            fileName = generalSystem.COLLECTION_ITEM_SAVE_PATH;
        }
        FQCommon.Common.AppendStringFile(fileName, itemNo);
    }

    IEnumerator DestroyItemBox(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Instantiate(itemHitObject, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }

    
}
