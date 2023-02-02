using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMP_AlphaAndDestroy : MonoBehaviour
{
    
    float ALPHA_NUM = 0.01f;
    float ADD_Y = 0.5f;
    float DEFAULT_SCALE = 0.1f;
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
            UnActive();
        }
        textMeshPro.color = c;
        rb.AddForce(0f, ADD_Y, 0f);
    }

    void UnActive() {
        Vector3 scale = new Vector3(DEFAULT_SCALE, DEFAULT_SCALE, DEFAULT_SCALE);
        gameObject.transform.localScale = scale;
        gameObject.SetActive(false);
    }

    public void SetDamage(int damage, Vector3 pos, Quaternion r, float scaleTimes) {
        textMeshPro.text = damage.ToString();
        gameObject.transform.position = pos;
        gameObject.transform.rotation = r;
        Vector3 scale = gameObject.transform.localScale;
        scale *= scaleTimes;
        gameObject.transform.localScale = scale;
        rb.velocity = Vector3.zero;
        Color c = textMeshPro.color;
        c.a = 1.0f;
        textMeshPro.color = c;
        gameObject.SetActive(true);

    }
}
