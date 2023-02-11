using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusChange : MonoBehaviour
{
    PlayerConfig config;
    HUD hud;
    PlayerDamage playerDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayerStatusChangeInit() {
        config = PlayerView.instance.config;
        hud = PlayerView.instance.hud;
        playerDamage = PlayerView.instance.playerDamage;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSatiation() {
        StartCoroutine(CalcSatiation());
    }

    IEnumerator CalcSatiation() {
        while (true) {
            yield return new WaitForSeconds(1f);
            float nowSatiation = config.CalcSatiation(config.GetDecreaseSatiation() * -1f);
            hud.RedrawSatiation();
            SatiationDamage(nowSatiation);
        }
    }

    void SatiationDamage(float nowSatiation) {
        if (nowSatiation > 0) return;
        float per = config.GetDecreaseSatiation() / 100f;
        int damage = (int)Mathf.Floor(config.GetMaxHP() * per);
        if (damage <= 0) return;
        //‹ó• ‚Å‚ÍŽ€‚È‚È‚¢
        if (config.GetHP() <= damage) return;

        playerDamage.PlayerDamageExec(damage);
        hud.RedrawHp();
    }
}
