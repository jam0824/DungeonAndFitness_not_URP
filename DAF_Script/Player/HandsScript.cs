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
    /// ��̃A�[�}�[��\������Ƃ��ɌĂ΂��
    /// </summary>
    public void ShowHandsArmor() {
        //����o�����Ƃ���trigger�ɂ��ēG�ɂԂ���Ȃ��悤�ɂ���
        SetIsTrigger(true);
        //����o�����Ƃ���handsDitect�Əd�Ȃ��Ă��
        //Collision�ɂȂ�̂ł��̑Ή�
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
            //�G�ɍU��������������isTrigger��true�ɂ��ē��߂���悤�ɂ��Ă���̂�
            //HandsDitectArea��trigger������false��rigidbody�������悤�ɂ���B
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
