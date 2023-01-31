using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    public GameObject PEACE_PREFAB;
    public GameObject BASE_PREFAB;
    public AudioClip BREAK_SE;
    public float base_y = 0.2f;
    public int peaseNum = 5;
    public float peaseRandomRange = 0.3f;
    bool hasBroken = false;

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
            hasBroken = true;
            float impact = GetImpact(collision);
            ContactPoint contact = collision.contacts[0];
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
        float impact = collision.impulse.magnitude;
        impact /= 1000f;
        if (impact < 1) impact = 1f;
        if (impact > 3f) impact = 3f;
        return impact;
    }
}
