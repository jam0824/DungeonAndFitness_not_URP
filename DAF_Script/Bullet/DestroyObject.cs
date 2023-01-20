using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Delete");
    }

    IEnumerator Delete() {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            Destroy(gameObject);
        }

    }
}
