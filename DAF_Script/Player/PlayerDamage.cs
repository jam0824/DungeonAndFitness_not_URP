using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    //HIT_AREA�ɂ��̃X�N���v�g������

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

    //Player���U�����󂯂��ۂɌĂ΂��
    void DamageController(Collider other) {
        int damage = Damage(other, SingletonGeneral.instance.player);
        PlayerDamageExec(damage);
    }

    //�_���[�W�̕\����HUD�̍X�V
    public void PlayerDamageExec(int damage) {
        SingletonGeneral.instance.PlayOneShot(audioSource, "NormalHitToPlayer");
        MakePlayerDamageText(damage);
        DebugWindow.instance.DFDebug("Player��" + damage + "�̃_���[�W�I");

        if (damage == 0) return;
        int nowHp = config.CalcHp(damage);
        hud.RedrawHp();
    }

    //Player�̃_���[�W�v�Z
    private int Damage(Collider other, GameObject player) {
        int playerDef = player.GetComponent<PlayerConfig>().GetDEF();
        int enemyAtk = other.GetComponent<EnemyBullet>().GetATK();
        other.GetComponent<EnemyBullet>().HitBullet();
        int damage = FQCommon.Common.GetDamage(enemyAtk, playerDef);
        if (damage < 0) damage = 0;
        return damage;
    }

    //�_���[�W�e�L�X�g�̕\��
    private void MakePlayerDamageText(int damage) {
        Vector3 pos = transform.position;
        Vector3 addPos = transform.forward;
        addPos.y = -0.1f;
        pos += addPos;

        //������HitArea�̌��������
        Quaternion r = transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;

        //�_���[�W����
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
