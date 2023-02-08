using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsScript : MonoBehaviour
{
    bool isHit = false;
    BoxCollider boxCollider;
    Rigidbody rb;
    string objName;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        objName = GetObjectName();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsTrigger(bool isTrigger) {
        boxCollider.isTrigger = isTrigger;
    }

    public bool GetIsHit() {
        return isHit;
    }
    public void SetIsHit(bool hit) {
        isHit = hit;
    }

    public float GetVelocity() {
        return rb.velocity.magnitude;
    }

    string GetObjectName() {
        return gameObject.name.ToLower();
    }

    

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "HandsDitectArea") {
            //敵に攻撃が当たったらisTriggerをtrueにして透過するようにしているので
            //HandsDitectAreaでtriggerしたらfalseでrigidbodyが働くようにする。
            SetIsTrigger(false);
        }
    }

    public void VivrationArmor(float frequency, float amplitude, float waitTime) {
        SingletonGeneral.instance.VivrationController(objName, frequency, amplitude, waitTime);
    }


}
