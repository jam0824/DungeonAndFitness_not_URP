using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public GameObject hudRig;
    public TextMeshPro hpNumber;
    public TextMeshPro mpNumber;
    public TextMeshPro satiationNumber;
    public GameObject hpBar;
    public GameObject mpBar;
    public GameObject satiationBar;
    GameObject face;
    PlayerConfig config;

    // Start is called before the first frame update
    void Start()
    {
        HudInit();
    }

    public void HudInit() {
        face = GameObject.Find("HitArea");
        config = PlayerView.instance.config;
        RedrawHp();
        RedrawMp();
        RedrawSatiation();
    }

    // Update is called once per frame
    void Update()
    {
        RotationHud(face);
        HudLookAt(hudRig, face);
    }

    void RotationHud(GameObject face) {
        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, r, 0.01f);
    }

    void HudLookAt(GameObject hudRig, GameObject face) {
        hudRig.transform.LookAt(face.transform);
    }

    public void RedrawHp() {
        int nowHp = config.GetHP();
        int maxHp = config.GetMaxHP();
        string txt = nowHp.ToString() + "/" + maxHp.ToString();
        hpNumber.text = txt;
        hpBar = RedrawBar(hpBar, nowHp, maxHp);
    }

    public void RedrawMp() {
        string txt = config.GetMP().ToString() + "/" + config.GetMaxMP().ToString();
        mpNumber.text = txt;
        mpBar = RedrawBar(mpBar, config.GetMP(), config.GetMaxMP());
    }

    public void RedrawSatiation() {
        string txt = config.GetSatiation().ToString() + "%";
        satiationNumber.text = txt;
        satiationBar = RedrawBar(satiationBar, config.GetSatiation(), 100);
    }

    GameObject RedrawBar(GameObject bar, int nowValue, int maxValue) {
        float per = (float)nowValue / (float)maxValue;
        Vector3 size = new Vector3(per, 1f, 1f);
        bar.transform.localScale = size;
        float x = (1 - per) / 4f;
        Vector3 pos = bar.transform.localPosition;
        pos.x += x;
        bar.transform.localPosition = pos;
        return bar;
    }

}
