using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsScript : MonoBehaviour
{
    GeneralSystem generalSystem;
    bool isHit = false;
    BoxCollider boxCollider;
    Rigidbody rb;
    string objName;

    private void Awake() {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
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
            //“G‚ÉUŒ‚‚ª“–‚½‚Á‚½‚çisTrigger‚ğtrue‚É‚µ‚Ä“§‰ß‚·‚é‚æ‚¤‚É‚µ‚Ä‚¢‚é‚Ì‚Å
            //HandsDitectArea‚Åtrigger‚µ‚½‚çfalse‚Årigidbody‚ª“­‚­‚æ‚¤‚É‚·‚éB
            SetIsTrigger(false);
        }
    }

    public void VivrationArmor(float frequency, float amplitude, float waitTime) {
        generalSystem.VivrationController(objName, frequency, amplitude, waitTime);
    }


}
