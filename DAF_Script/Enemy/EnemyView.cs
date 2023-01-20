using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    float addForce = 10000.0f;
    public GameObject Player { get; set; }
    public GameObject Face { get; set; }
    public EnemyConfig enemyConfig { get; set; }
    public EnemyDamage enemyDamage { get; set; }
    public EnemyMove enemyMove { get; set; }
    public EnemyAnimation enemyAnimation { get; set; }
    public GameObject PunchHitPrefab { get; set; }
    public GeneralSystem generalSystem { get; set; }
    public AudioSource audioSource { get; set; }
    public bool isGameObjectLoaded = false;


    // Start is called before the first frame update
    void Start()
    {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        Player = GameObject.Find("Player");
        Face = GameObject.Find("HitArea");

        PunchHitPrefab = Player.GetComponent<PlayerConfig>().GetPunchHitPrefab();

        enemyConfig = GetComponent<EnemyConfig>();
        enemyDamage = GetComponent<EnemyDamage>();
        enemyMove = GetComponent<EnemyMove>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        audioSource = GetComponent<AudioSource>();
        isGameObjectLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        int damage = 0;
        if (other.gameObject.tag == "PlayerAttack") {
            HandsScript handsScript = other.GetComponent<HandsScript>();
            if (handsScript.GetIsHit()) return;
            damage = enemyDamage.Damage(other, Player, enemyConfig, Face);
            if (damage > 0) {
                handsScript.SetIsHit(true);
                int hp = enemyConfig.calcHp(damage);
                makeHitEffect(other, damage);
                makeHitSE("NormalHitToEnemy");
                DebugWindow.instance.DFDebug("�X���C����" + damage + "�̃_���[�W�I");
                SetDamageAnimation(hp, other);
            }
        }
    }

    private void SetDamageAnimation(int hp, Collider other) {
        //HP���[���ɂȂ�����
        if (hp == 0) {
            enemyMove.StopAttack();
            enemyDamage.SetBlowOff(other, addForce);
            enemyAnimation.setDieAnim();
            generalSystem.PlayOneShot(audioSource, "NormalEnemyDie");
            /*
             * TODO ������΂������Ƃǂ������A�j���[�V�������邩
            enemyAnimation.DieMove(
                enemyConfig.GetHight(),
                enemyConfig.GetDeleteTime()
            );
            */
        }
        else {
            enemyAnimation.SetDamageAnim();
        }
    }

    public void makeHitEffect(Collider other, int damage) {
        Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
        //�����͊�̌��������
        Quaternion r = Face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;

        //�_���[�W�G�t�F�N�g
        GameObject hit = Instantiate(PunchHitPrefab, hitPos, r);

        //�_���[�W����
        GameObject damageText = Instantiate(
            generalSystem.GetPrefabDamageTextCanvas(),
            hitPos,
            r
        );
        damageText.GetComponent<AlphaAndDestroyObject>().SetDamage(damage);
        
    }

    public void makeHitSE(string SeName) {
        //�_���[�W��
        generalSystem.PlayOneShot(audioSource, SeName);
    }

    
}
