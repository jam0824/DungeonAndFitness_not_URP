using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public float WAIT_TIME_DELETE;
    public float SIZE;
    public GameObject itemHitObject;
    public AudioClip START_SOUND;
    public AudioClip ITEM_GET_SOUND;
    public AudioClip END_SOUND;
    public int activeCount = 0;
    GeneralSystem generalSystem;

    public void ItemBoxInit() {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
    }

    //ItemBox起動時に呼ばれる
    public void EnableItemBox(Vector3 pos, Quaternion r) {
        gameObject.SetActive(true);
        activeCount = 0;
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        gameObject.transform.position = pos;
        gameObject.transform.rotation = r;
        PlayOneShot(START_SOUND);
    }

    //ItemBox終了時に呼ばれる
    public void UnableItemBox() {
        PlayOneShot(END_SOUND);
        Instantiate(
            itemHitObject,
            gameObject.transform.position,
            gameObject.transform.rotation);
        gameObject.SetActive(false);
    }

    //gameObjectのアクティブ、非アクティブを返す
    public bool ActiveSelf() {
        return gameObject.activeSelf;
    }
    void PlayOneShot(AudioClip clip) {
        //activeのときのみ音を鳴らす
        if (gameObject.activeSelf) 
            generalSystem.PlayOneShotNoAudioWithClip(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) {
            ScaleUp();
            activeCount++;
        }
    }

    void ScaleUp() {
        if (gameObject.transform.localScale.x < SIZE) {
            Vector3 scale = gameObject.transform.localScale;
            scale.x += 0.02f;
            scale.y += 0.02f;
            scale.z += 0.02f;
            gameObject.transform.localScale = scale;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Item") {
            AddItem(other);
        }
    }

    void AddItem(Collider other) {
        ItemBag itemBag = other.gameObject.GetComponent<ItemBag>();
        if (itemBag.itemNo == null) return;

        SaveItem(itemBag.itemNo);
        itemBag.DestroyItem();
        PlayOneShot(ITEM_GET_SOUND);
        
        StartCoroutine(CoroutineDestroyItemBox(WAIT_TIME_DELETE));
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

    IEnumerator CoroutineDestroyItemBox(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        UnableItemBox();
    }

    
}
