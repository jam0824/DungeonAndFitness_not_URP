using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCollider : MonoBehaviour
{
    public GameObject cameraC;
    GameObject fieldObject;

    // Start is called before the first frame update
    void Start()
    {
        fieldObject = SingletonGeneral.instance.dungeonRoot;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = cameraC.transform.position;
        gameObject.transform.rotation = cameraC.transform.rotation;
    }

    //ƒJƒƒ‰‚ª•Ç‚É‚Ô‚Â‚©‚Á‚½‚Æ‚«‚Íƒ_ƒ“ƒWƒ‡ƒ“‚Ì•û‚ğˆÚ“®‚³‚¹‚Ä
    //’Ê‚è”²‚¯‚ç‚ê‚È‚¢‚æ‚¤‚É‚·‚é
    private void OnCollisionStay(Collision collision) {
        if(collision.gameObject.tag == "Wall") {
            ContactPoint contact = collision.contacts[0];
            Vector3 direction = contact.normal;
            Vector3 pos = fieldObject.transform.position;
            pos.x -= (direction.x * 0.1f);
            pos.z -= (direction.z * 0.1f);
            fieldObject.transform.position = pos;

        }
    }
}
