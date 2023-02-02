using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if ((collision.gameObject.tag == "PlayerAttack")&&(!hasBroken)) {
            float impact = GetImpact(collision);
            //インパクトが小さかったら壊れない
            if (impact < MIN_BREAK_IMPACT) return;

            hasBroken = true;
            ContactPoint contact = collision.contacts[0];
            MakeItem(ITEM_PREFAB, ITEM_PROBABILITY, item_base_y) ;
            GameObject baseObject = MakeBaseObject(
                BASE_PREFAB, 
                gameObject.transform.position, 
                gameObject.transform.rotation);
            
            MakePeaceObjects(
                PEACE_PREFAB,
                contact,
                gameObject.transform.rotation,
                impact,
                peaseNum,
                peaseRandomRange
                );
            
            //元のオブジェクトは消すので、残るオブジェクトで音を鳴らす
            baseObject.GetComponent<AudioSource>().PlayOneShot(BREAK_SE);
            Destroy(gameObject);
        }
    }

    void MakePeaceObjects(
        GameObject peacePrefab, 
        ContactPoint contact,
        Quaternion r,
        float impact, 
        int peaseNum,
        float range) 
    {
        DebugWindow.instance.DFDebug("vase_impact:" + impact);
        for (int i = 0; i < peaseNum; i++) {
            Vector3 pos = GetRandomPosition(contact.point, range);
            Vector3 velocity = contact.normal;
            velocity *= impact;
            GameObject peace = Instantiate(peacePrefab, pos, r);
            peace.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
        }
    }

    Vector3 GetRandomPosition(Vector3 pos, float range) {
        pos.x = pos.x - (range * 0.5f) + Random.Range(0.0f, range);
        pos.y = pos.y - (range * 0.5f) + Random.Range(0.0f, range);
        pos.z = pos.z - (range * 0.5f) + Random.Range(0.0f, range);
        return pos;
    }

    GameObject MakeBaseObject(GameObject basePrefab, Vector3 pos, Quaternion r) {
        pos.y += base_y;
        return Instantiate(basePrefab, pos, r);
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
}
