using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    //HIT_AREA�ɂ��̃X�N���v�g������

    GameObject player;
    GeneralSystem generalSystem;
    PlayerView playerView;
    AudioSource audioSource;
    // Start is called before the first frame update

    void Start()
    {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        playerView = GameObject.Find("Player").GetComponent<PlayerView>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "EnemyAttack") {
            int damage = Damage(other, generalSystem.GetPlayerPrefab());
            generalSystem.PlayOneShot(audioSource, "NormalHitToPlayer");
            MakePlayerDamageText(damage);
            DebugWindow.instance.DFDebug("Player��" + damage + "�̃_���[�W�I");
        }

    }

    private int Damage(Collider other, GameObject player) {
        int playerDef = player.GetComponent<PlayerConfig>().GetDEF();
        int enemyAtk = other.GetComponent<EnemyBullet>().GetATK();
        other.GetComponent<EnemyBullet>().HitBullet();
        return FQCommon.Common.GetDamage(enemyAtk, playerDef);
    }

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
