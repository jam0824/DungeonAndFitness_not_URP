using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    float addForce = 1000.0f;
    float BLOW_OFF_IMPACT = 2000.0f;
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

    Dictionary<string, float> itemDropProbability = new Dictionary<string, float>() {
        {"HighProbability", 0.5f },
        {"MiddleProbability", 0.1f },
        {"RareProbability", 0.01f }
    };
    float HIT_STOP = 0.1f;
    bool isBlowOff = false;

    public bool isGameObjectLoaded = false;
    DungeonSystem dungeonSystem;

    private void Awake() {
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

    // Start is called before the first frame update
    void Start()
    {
        
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
                makeHitEffect(contact, damage, impact);
                SetDamageAnimation(handsScript, contact, hp, impact);
                DebugWindow.instance.DFDebug("敵は" + damage + "のダメージ！");
            }
        }
        //吹っ飛んでいる最中なら
        if (isBlowOff) {
            if (collision.gameObject.tag == "Wall") 
                makeHitSE("BlowOffAndHitWall");
            if (collision.gameObject.tag == "Ground") 
                DeleteEnemyMain();
        }
    }

    private void SetDamageAnimation(HandsScript handsScript, ContactPoint contact, int hp, float impact) {
        //HPがゼロになったら
        if (hp == 0) {
            enemyMove.StopAttack();
            //Freeze rotationを解除する
            rigidbody.constraints = RigidbodyConstraints.None;
            //アイテムドロップ
            DropItem(generalSystem.itemDb);

            if (impact > BLOW_OFF_IMPACT) {
                //コントローラーを振動させる
                handsScript.VivrationArmor(0.5f, 1f, 1f);
                isBlowOff = true;
                enemyAnimation.SetDamageAnim();
                //ヒットストップを試してみる。手触りが変わるらしい
                StartCoroutine(ExecBlowOff(contact, impact, addForce, HIT_STOP));
            }
            else {
                //コントローラーを振動させる
                handsScript.VivrationArmor(0.5f, 0.5f, 0.2f);
                enemyAnimation.setDieAnim();
                makeHitSE("NormalEnemyDie");
                StartCoroutine(DeleteEnemy(1.5f));
            }
        }
        else {
            //コントローラーを振動させる
            handsScript.VivrationArmor(0.5f, 0.5f, 0.2f);
            enemyAnimation.SetDamageAnim();
            makeHitSE("NormalHitToEnemy");
        }
    }
    //BlowOff実行
    IEnumerator ExecBlowOff(
        ContactPoint contact,
        float impact, 
        float addForce, 
        float waitTime) 
    {
        yield return new WaitForSeconds(waitTime);
        SetBlowOff(contact, addForce, impact);
        makeHitSE("BlowOff");
        StartCoroutine(DeleteEnemy(3.0f));
    }

    //敵が死んだときの爆発エフェクトとdestroy
    IEnumerator DeleteEnemy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        DeleteEnemyMain();
    }
    void DeleteEnemyMain() {
        Instantiate(
            enemyConfig.GetPrefabEnemyDieEffect(),
            gameObject.transform.position,
            gameObject.transform.rotation);
        Destroy(gameObject);
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

    public void makeHitEffect(ContactPoint contact, int damage, float impact) {
        //向きは顔の向きを取る
        Quaternion r = Face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;

        //ダメージエフェクト
        GameObject hit = Instantiate(PunchHitPrefab, contact.point, r);
        //ダメージ数字
        GameObject damageText = dungeonSystem.GetDamageTextFromPool();
        float scaleTimes = 1 + (impact * 0.0001f);
        damageText.GetComponent<TMP_AlphaAndDestroy>().SetDamage(
            damage, 
            contact.point, 
            r, 
            scaleTimes);
        
    }

    public void makeHitSE(string SeName) {
        //ダメージ音
        generalSystem.PlayOneShot(audioSource, SeName);
    }

    //アイテムをドロップする
    void DropItem(ItemDB itemDb) {
        string itemNo = SelectDropItem();
        if (itemNo == "") return;
        Vector3 pos = gameObject.transform.position;
        pos.y += 1.0f;
        itemDb.MakeItemBag(
            itemNo, 
            pos, 
            gameObject.transform.rotation);
    }

    //アイテムドロップ率。高い、低い、レアの3パターン
    string SelectDropItem() {
        string itemNo = "";
        float r = Random.RandomRange(0.0f, 1.0f);
        DebugWindow.instance.DFDebug("itemDropRate:" + r);
        if (r <= itemDropProbability["HighProbability"])
            itemNo = enemyConfig.GetDropItems()[0];
        if(r <= itemDropProbability["MiddleProbability"])
            itemNo = enemyConfig.GetDropItems()[1];
        if (r <= itemDropProbability["RareProbability"])
            itemNo = enemyConfig.GetDropItems()[2];
        return itemNo;
    }

    
}
