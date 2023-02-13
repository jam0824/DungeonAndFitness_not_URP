using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsScript : MonoBehaviour
{
    float damageLockTime = 0.3f;
    float SHOW_DAMAGE_LOCK_TIME = 1f;
    bool isHit = false;
    BoxCollider boxCollider;
    Rigidbody rb;
    string objName;

    bool isDamageLock = false;

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

    /// <summary>
    /// 手のアーマーを表示するときに呼ばれる
    /// </summary>
    public void ShowHandsArmor() {
        //手を出したときはtriggerにして敵にぶつからないようにする
        SetIsTrigger(true);
        //手を出したときにhandsDitectと重なってると
        //Collisionになるのでその対応
        isDamageLock = true;
        StartCoroutine(UnlockDamageLock(SHOW_DAMAGE_LOCK_TIME));
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

    public bool GetDamageLock() {
        return isDamageLock;
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

    public void SetDamageLock() {
        isDamageLock = true;
        StartCoroutine(UnlockDamageLock(damageLockTime));
    }

    IEnumerator UnlockDamageLock(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        isDamageLock = false;
    }

}
