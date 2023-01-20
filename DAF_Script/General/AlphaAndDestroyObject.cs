using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaAndDestroyObject : MonoBehaviour
{
    public float deleteTime;
    float alpha;
    CanvasGroup canvasGroup;
    Text damageText;
    string stringDamage = "0";


    // Start is called before the first frame update
    void Start()
    {
        alpha = 0.01f;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        float a = canvasGroup.alpha - alpha;
        if (a < 0) {
            Destroy(gameObject);
        }
        canvasGroup.alpha = a;
        gameObject.transform.position += new Vector3(0, alpha, 0);
    }

    public void SetDamage(int damage) {
        damageText = transform.Find("DamageText").GetComponent<Text>();
        damageText.text = damage.ToString();
    }
}
