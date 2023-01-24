using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMP_AlphaAndDestroy : MonoBehaviour
{
    
    float ALPHA_NUM = 0.01f;
    float ADD_Y = 0.5f;
    TextMeshPro textMeshPro;
    Rigidbody rb;
    // Start is called before the first frame update

    private void Awake() {
        textMeshPro = GetComponent<TextMeshPro>();
        rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == false) return;

        Color c = textMeshPro.color;
        c.a -= ALPHA_NUM;
        if (c.a <= 0) {
            gameObject.SetActive(false);
        }
        textMeshPro.color = c;
        rb.AddForce(0f, ADD_Y, 0f);
    }

    public void SetDamage(int damage, Vector3 pos, Quaternion r) {
        textMeshPro.text = damage.ToString();
        gameObject.transform.position = pos;
        gameObject.transform.rotation = r;
        rb.velocity = Vector3.zero;
        Color c = textMeshPro.color;
        c.a = 1.0f;
        textMeshPro.color = c;
        gameObject.SetActive(true);

    }
}
