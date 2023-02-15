using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelFeelIcon : MonoBehaviour
{
    public string[] ICON_LABEL;
    public Sprite[] ICON_SPRITE;

    float DECREASE_SPEED = 0.01f;
    float alpha = 1.0f;
    SpriteRenderer spriteRenderer;
    bool isSpriteEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() {
        if (gameObject.activeSelf) {
            DecreaseAlpha();
            if(alpha <= 0) {
                alpha = 1.0f;
                gameObject.SetActive(false);
            }
        }
    }

    void DecreaseAlpha() {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        alpha -= DECREASE_SPEED;
        Color c = spriteRenderer.color;
        c.a = alpha;
        spriteRenderer.color = c;
    }

    void ActiveSprite(Sprite sp) {
        spriteRenderer.sprite = sp;
        gameObject.SetActive(true);
    }

    public void SetIcon(string iconKey, Quaternion r) {
        gameObject.transform.rotation = r;
        spriteRenderer = GetComponent<SpriteRenderer>();
        int no = GetIconNo(iconKey);
        Sprite sp = GetSprite(no);
        ActiveSprite(sp);
    }

    int GetIconNo(string iconKey) {
        int no = 0;
        for(int i=0; i< ICON_LABEL.Length; i++) {
            if(ICON_LABEL[i] == iconKey) {
                no = i;
                break;
            }
        }
        return no;
    }

    Sprite GetSprite(int spriteNo) {
        return ICON_SPRITE[spriteNo];
    }
}
