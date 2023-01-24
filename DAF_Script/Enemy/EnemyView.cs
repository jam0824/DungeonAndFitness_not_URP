using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    float addForce = 1000.0f;
    public GameObject Player { get; set; }
    public GameObject Face { get; set; }
    public EnemyConfig enemyConfig { get; set; }
    public EnemyDamage enemyDamage { get; set; }
    public EnemyMove enemyMove { get; set; }
    public EnemyAnimation enemyAnimation { get; set; }
    public GameObject PunchHitPrefab { get; set; }
    public GeneralSystem generalSystem { get; set; }
    public AudioSource audioSource { get; set; }
    public Rigidbody rigidbody { get; set; }

    public bool isGameObjectLoaded = false;
    DungeonSystem dungeonSystem;


    // Start is called before the first frame update
    void Start()
    {
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();

        Player = GameObject.Find("Player");
        Face = GameObject.Find("HitArea");
        

        PunchHitPrefab = Player.GetComponent<PlayerConfig>().GetPunchHitPrefab();

        enemyConfig = GetComponent<EnemyConfig>();
        enemyDamage = GetComponent<EnemyDamage>();
        enemyMove = GetComponent<EnemyMove>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        isGameObjectLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "PlayerAttack") {
            HandsScript handsScript = collision.gameObject.GetComponent<HandsScript>();
            //ArmorのIsTriggerをtrueにして透過するようにする
            handsScript.SetIsTrigger(true);
            float impact = enemyDamage.GetImpact(collision);
            int damage = enemyDamage.Damage(collision, impact, Player, enemyConfig);
            DebugWindow.instance.DFDebug("impact:" + impact);
            if (damage > 0) {
                ContactPoint contact = collision.contacts[0];
                int hp = enemyConfig.calcHp(damage);
                makeHitEffect(contact, damage);
                SetDamageAnimation(contact, hp, impact);
                DebugWindow.instance.DFDebug("敵は" + damage + "のダメージ！");
            }
        }
    }

    private void SetDamageAnimation(ContactPoint contact, int hp, float impact) {
        //HPがゼロになったら
        if (hp == 0) {
            enemyMove.StopAttack();
            SetBlowOff(contact, addForce, impact);
            //Freeze rotationを解除する
            rigidbody.constraints = RigidbodyConstraints.None;
            enemyAnimation.setDieAnim();
            makeHitSE("NormalEnemyDie");
            /*
             * TODO 吹っ飛ばしたあとどういうアニメーションするか
            enemyAnimation.DieMove(
                enemyConfig.GetHight(),
                enemyConfig.GetDeleteTime()
            );
            */
        }
        else {
            enemyAnimation.SetDamageAnim();
            makeHitSE("NormalHitToEnemy");
        }
    }

    //最後の吹っ飛ばし
    public void SetBlowOff(ContactPoint contact, float addForce, float impact) {
        Vector3 direction = contact.normal;
        DebugWindow.instance.DFDebug("direction : " + direction);
        direction.y += 0.3f;
        direction.x *= addForce * (1 + impact * 0.0001f);
        direction.y *= addForce * (1 + impact * 0.0001f);
        direction.z *= addForce * (1 + impact * 0.0001f);
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }

    public void makeHitEffect(ContactPoint contact, int damage) {
        //向きは顔の向きを取る
        Quaternion r = Face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;

        //ダメージエフェクト
        GameObject hit = Instantiate(PunchHitPrefab, contact.point, r);
        //ダメージ数字
        GameObject damageText = dungeonSystem.GetDamageTextFromPool();
        damageText.GetComponent<TMP_AlphaAndDestroy>().SetDamage(damage, contact.point, r);
        
    }

    public void makeHitSE(string SeName) {
        //ダメージ音
        generalSystem.PlayOneShot(audioSource, SeName);
    }

    
}
