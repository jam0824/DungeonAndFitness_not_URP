using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    EnemyView enemyView;

    string state = "Walk";
    int walkCount = 0;
    int MAX_WALK_COUNT = 600;

    float DELETE_Y = -30.0f;

    private void Awake() {
        enemyView = GetComponent<EnemyView>();
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        //ロードが完了していなかったら戻る
        if(enemyView.isGameObjectLoaded == false) {
            return;
        }
        //ある高さより低くなったら落ちているので削除する
        if (gameObject.transform.position.y < DELETE_Y) Destroy(gameObject);

        state = enemyView.enemyConfig.GetEnemyState();
        float dist = FQCommon.Common.GetDistance(transform.position, enemyView.Player.transform.position);

        switch (state) {
            case "Walk":
                WhenWalk(dist, enemyView.enemyConfig);
                break;
            case "Notice":
                WhenNotice(dist, enemyView.Player, enemyView.enemyConfig, enemyView.enemyAnimation);
                break;
            case "Battle":
                WhenBattle(dist, enemyView.enemyConfig, enemyView.enemyAnimation);
                break;

        }
        
    }

    void WhenNotice(
        float dist, 
        GameObject player,
        EnemyConfig enemyConfig, 
        EnemyAnimation enemyAnimation) {

        LookAt(player);
        WorkForward(enemyConfig.GetWalkSpeed());
        
        if(dist <= enemyView.enemyConfig.GetBattleDistance()) {
            DebugWindow.instance.DFDebug("StateCange:Battle");
            enemyConfig.SetEnemyState("Battle");
            enemyAnimation.SetWalkAnim(false);
            StartAttack();
        }
        //一定距離離れたらwalkに戻る
        if (dist >= enemyConfig.GetBattleEndDistance()) {
            ChangeWalkState(enemyConfig, enemyAnimation);
        }
    }

    void WhenWalk(
        float dist, 
        EnemyConfig enemyConfig) {

        RandomWalk(enemyConfig);
        if (dist <= enemyConfig.GetNoticeDistance()) {
            DebugWindow.instance.DFDebug("StateCange:Notice");
            //MakeNoticeEffect();
            SingletonGeneral.instance.PlayOneShot(enemyView.audioSource, "EnemyNoticeSE");
            enemyConfig.SetEnemyState("Notice");
        }
    }

    void WhenBattle(
        float dist,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) {

        if (dist >= enemyConfig.GetBattleEndDistance()) {
            ChangeWalkState(enemyConfig, enemyAnimation);
            StopAttack();
        }
    }

    void ChangeWalkState(EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) {
        DebugWindow.instance.DFDebug("StateCange:Walk");
        enemyConfig.SetEnemyState("Walk");
        enemyAnimation.SetWalkAnim(true);
    }

    void RandomWalk(EnemyConfig enemyConfig) {
        if (walkCount == 0) {
            ChangeDirection();
        }
        else {
            WorkForward(enemyConfig.GetWalkSpeed());
        }
        walkCount++;

        if (walkCount >= MAX_WALK_COUNT) {
            walkCount = 0;
        }
    }

    void ChangeDirection() {
        int rnd = Random.Range(0, 359);
        Vector3 worldAngle = transform.eulerAngles;
        worldAngle.y = rnd;
        transform.eulerAngles = worldAngle;
    }

    //前に歩く
    void WorkForward(float walkSpeed) {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
        //enemyView.rigidbody.AddForce(transform.forward * walkSpeed, ForceMode.Force);
        if (enemyView.enemyAnimation.GetBoolWalkAnim() == false) {
            enemyView.enemyAnimation.SetWalkAnim(true);
        }
    }

    //オブジェクトの方を向く
    void LookAt(GameObject target) {
        //あるオブジェクトから見た別のオブジェクトの方向を求める
        var direction = target.transform.position - transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
    }

    //攻撃開始
    public void StartAttack() {
        StartCoroutine("MakeBullet");
    }

    //攻撃終了
    public void StopAttack() {
        StopCoroutine("MakeBullet");
    }

    IEnumerator MakeBullet() {
        while (true) {
            EnemyConfig config = GetComponent<EnemyConfig>();
            GameObject ball = config.GetPrefabAttack();
            float waitTime = config.GetATKInterval();
            yield return new WaitForSeconds(waitTime);

            int atk = config.GetATK();
            float spd = config.GetSPD();
            Vector3 pos = transform.position;
            Vector3 addPos = -transform.forward;
            pos = pos + addPos;
            pos.y += 3.5f;
            GameObject shot = Instantiate(ball, pos, Quaternion.identity);
            shot.transform.parent = SingletonGeneral.instance.dungeonRoot.transform;
            SingletonGeneral.instance.PlayOneShot(enemyView.audioSource, "NormalShotAppear");

            EnemyBullet enemyBullet = shot.GetComponent<EnemyBullet>();
            enemyBullet.player = enemyView.Face;
            enemyBullet.SetSPD(spd);
            enemyBullet.SetATK(atk);
            enemyBullet.SetEnemyGameObject(gameObject);
            enemyBullet.enemyConfig = enemyView.enemyConfig;
        } 
    }

}
