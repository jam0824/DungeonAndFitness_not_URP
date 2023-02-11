using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    //HIT_AREAにこのスクリプトをつける

    PlayerView playerView;
    PlayerConfig config;
    HUD hud;
    AudioSource audioSource;
    // Start is called before the first frame update

    void Start()
    {
        playerView = PlayerView.instance;
        config = PlayerView.instance.config;
        hud = PlayerView.instance.hud;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "EnemyAttack") {
            DamageController(other);
        }

    }

    //Playerが攻撃を受けた際に呼ばれる
    void DamageController(Collider other) {
        int damage = Damage(other, SingletonGeneral.instance.player);
        PlayerDamageExec(damage);
    }

    //ダメージの表示とHUDの更新
    public void PlayerDamageExec(int damage) {
        SingletonGeneral.instance.PlayOneShot(audioSource, "NormalHitToPlayer");
        MakePlayerDamageText(damage);
        DebugWindow.instance.DFDebug("Playerは" + damage + "のダメージ！");

        if (damage == 0) return;
        int nowHp = config.CalcHp(damage);
        hud.RedrawHp();
    }

    //Playerのダメージ計算
    private int Damage(Collider other, GameObject player) {
        int playerDef = player.GetComponent<PlayerConfig>().GetDEF();
        int enemyAtk = other.GetComponent<EnemyBullet>().GetATK();
        other.GetComponent<EnemyBullet>().HitBullet();
        int damage = FQCommon.Common.GetDamage(enemyAtk, playerDef);
        if (damage < 0) damage = 0;
        return damage;
    }

    //ダメージテキストの表示
    private void MakePlayerDamageText(int damage) {
        Vector3 pos = transform.position;
        Vector3 addPos = transform.forward;
        addPos.y = -0.1f;
        pos += addPos;

        //向きはHitAreaの向きを取る
        Quaternion r = transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;

        //ダメージ数字
        /*
        GameObject damageText = Instantiate(
            generalSystem.GetPrefabPlayerDamageTextCanvas(),
            pos,
            r
        );
        */
        GameObject damageText = playerView.dungeonSystem.GetPlayerDamageTextFromPool();
        damageText.GetComponent<TMP_AlphaAndDestroy>().SetDamage(damage, pos, r, 1f);
    }
}
