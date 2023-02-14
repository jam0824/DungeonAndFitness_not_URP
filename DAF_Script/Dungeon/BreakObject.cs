using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    public int hitCount = 1;
    public string SWITCH_KEY = "";
    public string SWITCH_VALUE = "";
    public GameObject PEACE_PREFAB;
    public GameObject BASE_PREFAB;
    public AudioClip BREAK_SE;
    public GameObject ITEM_PREFAB;
    public float ITEM_PROBABILITY = 0;
    public float base_y = 0.2f;
    public float item_base_y = 0.5f;
    public int peaseNum = 5;
    public float peaseRandomRange = 0.3f;

    bool hasBroken = false;

    float MIN_BREAK_IMPACT = 1.0f;

    string HIT_SE = "HitObjectSe";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        CheckCollider(collision);
    }


    void CheckCollider(Collision collision) {
        if ((collision.gameObject.tag == "PlayerAttack") && (!hasBroken)) {
            float impact = GetImpact(collision);

            hitCount--;
            ContactPoint contact = collision.contacts[0];
            MakeHitEffect(contact);
            MakeHitSE(HIT_SE);
            if (hitCount <= 0) Break(impact, contact);

        }
    }

    void MakeHitEffect(ContactPoint contact) {
        Quaternion r = SingletonGeneral.instance.GetQuaternionFace();
        //ダメージエフェクト
        GameObject hit = Instantiate(
            PlayerView.instance.config.PUNCH_HIT_PREFAB,
            contact.point,
            r);
    }
    void MakeHitSE(string SeName) {
        //ダメージ音
        SingletonGeneral.instance.PlayOneShotNoAudioSource(SeName);
    }

    void Break(float impact, ContactPoint contact) {
        hasBroken = true;
        
        GameObject dungeonRoot = GetDungeonRootObject();

        MakeItem(ITEM_PREFAB, ITEM_PROBABILITY, item_base_y);
        //BasePrefabが設定されているときはbase objectを作る
        if(BASE_PREFAB != null) {
            GameObject baseObject = MakeBaseObject(
                BASE_PREFAB,
                gameObject.transform.position,
                gameObject.transform.rotation,
                dungeonRoot);
            //元のオブジェクトは消すので、残るオブジェクトで音を鳴らす
            baseObject.GetComponent<AudioSource>().PlayOneShot(BREAK_SE);
        }
        else{
            //残るオブジェクトがない場合はシングルトンの方でSEを鳴らす
            SingletonGeneral.instance.PlayOneShotNoAudioWithClip(BREAK_SE);
        }
        
        MakePeaceObjects(
            PEACE_PREFAB,
            contact,
            gameObject.transform.rotation,
            impact,
            peaseNum,
            peaseRandomRange,
            dungeonRoot
            );


        SetSwitch();
        Destroy(gameObject);
    }

    void SetSwitch() {
        if (SWITCH_KEY == "") return;
        SingletonGeneral.instance.scenarioSystem.SetSwitch(SWITCH_KEY, SWITCH_VALUE);
    }

    void MakePeaceObjects(
        GameObject peacePrefab,
        ContactPoint contact,
        Quaternion r,
        float impact,
        int peaseNum,
        float range,
        GameObject dungeonRoot) 
    {
        DebugWindow.instance.DFDebug("vase_impact:" + impact);
        for (int i = 0; i < peaseNum; i++) {
            Vector3 pos = GetRandomPosition(contact.point, range);
            Vector3 velocity = contact.normal;
            velocity *= impact;
            GameObject peace = Instantiate(peacePrefab, pos, r);
            peace.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            peace.transform.parent = dungeonRoot.transform;
        }
    }

    Vector3 GetRandomPosition(Vector3 pos, float range) {
        pos.x = pos.x - (range * 0.5f) + Random.Range(0.0f, range);
        pos.y = pos.y - (range * 0.5f) + Random.Range(0.0f, range);
        pos.z = pos.z - (range * 0.5f) + Random.Range(0.0f, range);
        return pos;
    }

    GameObject MakeBaseObject(
        GameObject basePrefab, 
        Vector3 pos, 
        Quaternion r, 
        GameObject dungeonRoot) 
    {
        pos.y += base_y;
        GameObject baseObject = Instantiate(basePrefab, pos, r);
        baseObject.transform.parent = dungeonRoot.transform;
        return baseObject;
    }


    float GetImpact(Collision collision) {
        float impact = collision.impulse.magnitude / Time.fixedDeltaTime;
        impact /= 1000f;
        if (impact > 5f) impact = 5f;
        return impact;
    }

    //アイテムをprobalilityで設定した確率でaddYに生成する
    void MakeItem(GameObject item, float probability, float addY) {
        if (probability == 0) return;
        float r = Random.Range(0.0f, 1.0f);
        if (r > probability) return;
        Vector3 pos = transform.position;
        pos.y += addY;
        GameObject itemObject = Instantiate(item, pos, transform.rotation);
        Vector3 velocity = new Vector3(0f, 1f, 0f);
        itemObject.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
    }

    GameObject GetDungeonRootObject() {
        return SingletonGeneral.instance.dungeonRoot;
    }
}
