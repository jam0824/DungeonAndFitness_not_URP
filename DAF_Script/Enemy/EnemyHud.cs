using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHud : MonoBehaviour
{
    GameObject face;
    EnemyConfig config;
    GameObject hpBar;
    SpriteRenderer hpBarSpriteRenderer;
    TextMeshPro hpNumber;

    string KANTEI_ITEM_NO;

    int oldHp;

    // Start is called before the first frame update
    void Start()
    {
        EnemyHudInit();
    }

    void EnemyHudInit() {
        KANTEI_ITEM_NO = SingletonGeneral.instance.KANTEI_ITEM_NO;
        if (!SingletonGeneral.instance.itemDb.HasItem(KANTEI_ITEM_NO)) {
            gameObject.SetActive(false);
            return;
        }
        face = GameObject.Find("HitArea");
        GameObject parentObject = transform.parent.gameObject;
        GameObject grandParentObject = parentObject.transform.parent.gameObject;
        config = grandParentObject.GetComponent<EnemyConfig>();
        hpBar = transform.Find("HpBar").gameObject;
        hpBarSpriteRenderer = hpBar.GetComponent<SpriteRenderer>();
        hpNumber = transform.Find("HpNumber").GetComponent<TextMeshPro>();
        oldHp = RedrawHpNumber(config);
    }

    // Update is called once per frame
    void Update()
    {
        ShowHud();
    }

    void ShowHud() {
        if (face == null) EnemyHudInit();
        transform.LookAt(face.transform);
        Redraw();
    }

    void Redraw() {
        //HPÇ™ïœÇÌÇ¡ÇƒÇ¢Ç»Ç©Ç¡ÇΩÇÁèàóùÇµÇ»Ç¢
        if (oldHp == config.GetHP()) return;
        oldHp = RedrawHpNumber(config);
        RedrawBar(hpBar, config.GetHP(), config.GetMaxHP());
    }

    int RedrawHpNumber(EnemyConfig config) {
        string hp = config.GetHP().ToString() + "/" + config.GetMaxHP().ToString();
        hpNumber.text = hp;
        return config.GetHP();
    }

    GameObject RedrawBar(GameObject bar, int nowValue, int maxValue) {
        float per = (float)nowValue / (float)maxValue;
        Vector3 size = new Vector3(per, 1f, 1f);
        bar.transform.localScale = size;

        float x = (1.0f - per) / 2.0f;
        Vector3 pos = bar.transform.localPosition;
        pos.x = -x;
        bar.transform.localPosition = pos;
        return bar;
    }
}
