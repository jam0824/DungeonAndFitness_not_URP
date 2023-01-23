using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsScript : MonoBehaviour
{
    bool isHit = false;
    BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
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

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "HandsDitectArea") {
            //敵に攻撃が当たったらisTriggerをtrueにして透過するようにしているので
            //HandsDitectAreaでtriggerしたらfalseでrigidbodyが働くようにする。
            SetIsTrigger(false);
        }
    }
}
