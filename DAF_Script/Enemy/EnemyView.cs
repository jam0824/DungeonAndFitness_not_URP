using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    const float MAX_DAMAGE_TEXT_SCALE = 1.2f;//MINと合計なので1.7fまで
    const float MIN_DAMAGE_TEXT_SCALE = 0.5f;
    const float OFFSET_DAMAGE_TEXT_POS = 0.3f;
    const float ADD_FORCE = 1000.0f;
    const float BLOW_OFF_IMPACT = 1500.0f;
    public GameObject Player { get; set; }
    public GameObject Face { get; set; }
    public EnemyConfig enemyConfig { get; set; }
    public EnemyDamage enemyDamage { get; set; }
    public IEnemyMove enemyMove { get; set; }
    public IEnemyAttack enemyAttack { get; set; }
    public EnemyAnimation enemyAnimation { get; set; }
    public GameObject PunchHitPrefab { get; set; }
    public AudioSource audioSource { get; set; }
    public Rigidbody rigidbody { get; set; }

    Dictionary<string, float> itemDropProbability = new Dictionary<string, float>() {
        {"HighProbability", 0.5f },
        {"MiddleProbability", 0.1f },
        {"RareProbability", 0.01f }
    };
    bool isBlowOff = false;

    [HideInInspector] public bool isGameObjectLoaded = false;
    DungeonSystem dungeonSystem;

    private void Awake() {
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();

        Player = GameObject.Find("Player");
        Face = GameObject.Find("HitArea");


        PunchHitPrefab = Player.GetComponent<PlayerConfig>().GetPunchHitPrefab();

        enemyConfig = GetComponent<EnemyConfig>();
        enemyDamage = GetComponent<EnemyDamage>();
        enemyMove = GetComponent<IEnemyMove>();
        enemyAttack = GetComponent<IEnemyAttack>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        enemyMove.EnemyMoveInit(this);
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
            //HPがゼロだったら判定させない
            if (enemyConfig.GetHP() <= 0) return;
            HandsScript handsScript = collision.gameObject.GetComponent<HandsScript>();
            //ダメージロック中は計算しない
            if (handsScript.GetDamageLock()) return;
            //ArmorのIsTriggerをtrueにして透過するようにする
            handsScript.SetIsTrigger(true);
            //ダメージ計算
            DamageCalculation(collision, handsScript, Player, enemyConfig);
        }
        //吹っ飛んでいる最中かチェック
        CheckBlowOff(collision, isBlowOff);
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="handsScript"></param>
    /// <param name="player"></param>
    /// <param name="config"></param>
    void DamageCalculation(
        Collision collision, 
        HandsScript handsScript, 
        GameObject player, 
        EnemyConfig config) 
    {
        float impact = enemyDamage.GetImpact(collision);
        int damage = enemyDamage.Damage(collision, impact, player, config);
        DebugWindow.instance.DFDebug("impact:" + impact);
        if (damage > 0) {
            ContactPoint contact = collision.contacts[0];
            int hp = enemyConfig.calcHp(damage);
            MakeHitEffectAndText(contact, damage, impact);
            SetDamageAnimation(handsScript, contact, hp, impact);
            //ひとつの腕で複数ダメージがあるときがあるのでロック
            handsScript.SetDamageLock();
            //近づいて攻撃した場合ではないときに気づかせる
            ChangeEnemyStatus();
            DebugWindow.instance.DFDebug("敵は" + damage + "のダメージ！");
        }
    }

    void CheckBlowOff(Collision collision, bool isBlowOff) {
        if (!isBlowOff) return;
        if (collision.gameObject.tag == "Wall") makeHitSE("BlowOffAndHitWall");
        if (collision.gameObject.tag == "Ground") DeleteEnemyMain();
    }

    /// <summary>
    /// 近づく前にNoticeにする場合に呼ぶ
    /// </summary>
    private void ChangeEnemyStatus() {
        string state = enemyConfig.GetEnemyState();
        if (state == "Walk") enemyMove.ExternalNotice();
    }

    private void SetDamageAnimation(
        HandsScript handsScript, 
        ContactPoint contact, 
        int hp, 
        float impact) 
    {
        //HPがゼロになったら
        if (hp == 0) {
            enemyAttack.StopAttack();
            //Freeze rotationを解除する
            rigidbody.constraints = RigidbodyConstraints.None;
            //アイテムドロップ
            DropItem(SingletonGeneral.instance.itemDb);

            if (impact > BLOW_OFF_IMPACT) {
                //コントローラーを振動させる
                handsScript.VivrationArmor(0.5f, 1f, 1f);
                isBlowOff = true;
                enemyAnimation.SetDamageAnim();
                ExecBlowOff(contact, impact, ADD_FORCE);
            }
            else {
                //コントローラーを振動させる
                handsScript.VivrationArmor(0.5f, 0.5f, 0.1f);
                enemyAnimation.setDieAnim();
                SelectHitSe(impact);
                makeHitSE("NormalEnemyDie");
                StartCoroutine(DeleteEnemy(1.5f));
            }
        }
        else {
            //コントローラーを振動させる
            handsScript.VivrationArmor(0.5f, 0.5f, 0.1f);
            enemyAnimation.SetDamageAnim();
            SelectHitSe(impact);
        }
    }
    //BlowOff実行
    void ExecBlowOff(
        ContactPoint contact,
        float impact, 
        float addForce) 
    {
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
        Vector3 pos = contact.point;
        Vector3 direction = contact.normal;
        DebugWindow.instance.DFDebug("direction : " + direction);
        direction.y += 0.3f;
        direction.x *= addForce * (1 + impact * 0.0001f);
        direction.y *= addForce * (1 + impact * 0.0001f);
        direction.z *= addForce * (1 + impact * 0.0001f);
        GetComponent<Rigidbody>().AddForceAtPosition(direction, pos, ForceMode.Impulse);
    }

    public void MakeHitEffectAndText(ContactPoint contact, int damage, float impact) {
        //向きは顔の向きを取る
        Quaternion r = Face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;

        //ダメージエフェクト
        GameObject hit = Instantiate(PunchHitPrefab, contact.point, r);
        //ダメージ数字
        GameObject damageText = dungeonSystem.GetDamageTextFromPool();
        float scaleTimes = MIN_DAMAGE_TEXT_SCALE + Mathf.Min(impact * 0.0001f, MAX_DAMAGE_TEXT_SCALE);
        
        // ダメージテキストの位置を調整するオフセットを設定
        Vector3 adjustedPosition = FQCommon.Common.GetPositionCloserToPoint(
            OFFSET_DAMAGE_TEXT_POS, 
            contact.point, 
            Face.transform.position);
        //ダメージ表示
        damageText.GetComponent<TMP_AlphaAndDestroy>().SetDamage(
            damage,
            adjustedPosition,
            r,
            scaleTimes);
    }

    /// <summary>
    /// impactで殴る音変更
    /// </summary>
    /// <param name="impact"></param>
    void SelectHitSe(float impact) {
        string seKey = "";
        if (impact < 1000f) {
            seKey = "SmallHitToEnemy";
        }
        else if (impact < 1500f) {
            seKey = "NormalHitToEnemy";
        }
        else {
            seKey = "HardHitToEnemy";
        }
        makeHitSE(seKey);
    }

    public void makeHitSE(string SeName) {
        //ダメージ音
        SingletonGeneral.instance.PlayOneShot(audioSource, SeName);
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
        float r = Random.Range(0.0f, 1.0f);
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

public interface IEnemyAttack
{
    public void StartAttack(EnemyView enemyView);
    public void StopAttack();
    IEnumerator MakeBullet(EnemyView enemyView);
}

public interface IEnemyMove
{
    public void EnemyMoveInit(EnemyView ev);
    void WhenNotice(
        float dist, 
        GameObject player, 
        EnemyConfig enemyConfig, 
        EnemyAnimation enemyAnimation);
    void WhenWalk(
        float dist,
        EnemyConfig enemyConfig);
    void WhenBattle(
        float dist,
        GameObject player,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation);
    void ExternalNotice();
    public void ChangeBattleState(EnemyConfig enemyConfig);
    void ChangeNoticeState(EnemyConfig enemyConfig);
    void ChangeWalkState(EnemyConfig enemyConfig);
}
