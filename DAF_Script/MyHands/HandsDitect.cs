using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsDitect : MonoBehaviour
{
    GeneralSystem generalSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "PlayerAttack") {
            other.gameObject.GetComponent<HandsScript>().SetIsHit(false);
        }
    }
}
