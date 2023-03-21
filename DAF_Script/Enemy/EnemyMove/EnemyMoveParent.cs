using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveParent : MonoBehaviour
{
    [HideInInspector] public EnemyView enemyView;
    [HideInInspector] public EnemyConfig config;
    [HideInInspector] public EnemyAnimation enemyAnimation;
    [HideInInspector] public IEnemyAttack enemyAttack;

    public enum STATE_TYPE
    {
        WALK,
        NOTICE,
        BATTLE
    };

    public string state = "Walk";

    float DELETE_Y = -30.0f;

    public void EnemyMoveInit(EnemyView ev) {
        enemyView = ev;
        enemyAttack = ev.enemyAttack;
        config = ev.enemyConfig;
        enemyAnimation = ev.enemyAnimation;
    }
    // Update is called once per frame
    void Update() {
        //ロードが完了していなかったら戻る
        if (enemyView == null) return;

        //ある高さより低くなったら落ちているので削除する
        if (gameObject.transform.position.y < DELETE_Y) Destroy(gameObject);

        state = config.GetEnemyState();
        float dist = FQCommon.Common.GetDistance(transform.position, enemyView.Player.transform.position);

        ActionMode(state, dist);
    }

    /// <summary>
    /// ActionModeがStopの時
    /// </summary>
    /// <param name="state"></param>
    /// <param name="dist"></param>
    public virtual void ActionMode(string state, float dist) {
        GameObject player = enemyView.Player;
        switch (state) {
            case "Walk":
                WhenWalk(dist, config);
                break;
            case "Notice":
                WhenNotice(dist, player, config, enemyAnimation);
                break;
            case "Battle":
                WhenBattle(dist, player, config, enemyAnimation);
                break;
        }

        STATE_TYPE nowState = DetermineStatusByDistance(dist, config);
        if (nowState == STATE_TYPE.WALK) enemyAnimation.SetWalkAnim(true);
    }

    public virtual void WhenWalk(
        float dist,
        EnemyConfig enemyConfig) {
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.NOTICE) {
            //MakeNoticeEffect();
            ChangeNoticeState(enemyConfig);
        }
    }
    public void ChangeNoticeState(EnemyConfig enemyConfig) {
        SingletonGeneral.instance.PlayOneShot(enemyView.audioSource, "EnemyNoticeSE");
        enemyConfig.SetEnemyState("Notice");
        DebugWindow.instance.DFDebug("StateCange:Notice");
    }

    public virtual void WhenNotice(
        float dist,
        GameObject player,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) {

        LookAt(player);
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.BATTLE) {
            ChangeBattleState(enemyConfig);
        }
        else if (nowState == STATE_TYPE.WALK) {
            ChangeWalkState(enemyConfig);
        }
    }
    public void ChangeBattleState(EnemyConfig enemyConfig) {
        enemyConfig.SetEnemyState("Battle");
        DebugWindow.instance.DFDebug("StateCange:Battle");
        enemyAnimation.SetWalkAnim(false);
        enemyAttack.StartAttack(enemyView);
    }

    public virtual void WhenBattle(
        float dist,
        GameObject player,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) {
        LookAt(player);
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.WALK) {
            enemyAttack.StopAttack();
            ChangeWalkState(enemyConfig);
        }
    }

    public void ChangeWalkState(EnemyConfig enemyConfig) {
        enemyConfig.SetEnemyState("Walk");
        DebugWindow.instance.DFDebug("StateCange:Walk");
    }

    public STATE_TYPE DetermineStatusByDistance(float dist, EnemyConfig enemyConfig) {
        string state = enemyConfig.GetEnemyState();
        if ((state == "Notice") || (state == "Battle")) {
            if (dist >= enemyConfig.GetBattleEndDistance())
                return STATE_TYPE.WALK;
        }
        if (dist <= enemyConfig.GetBattleDistance()) {
            return STATE_TYPE.BATTLE;
        }
        else if (dist <= enemyConfig.GetNoticeDistance()) {
            return STATE_TYPE.NOTICE;
        }
        switch (state) {
            case "Notice":
                return STATE_TYPE.NOTICE;
            case "Battle":
                return STATE_TYPE.BATTLE;
            default:
                return STATE_TYPE.WALK;
        }
        return STATE_TYPE.WALK;
    }

    /// <summary>
    /// 外部から呼び出しでNoticeにする
    /// </summary>
    public void ExternalNotice() {
        ChangeNoticeState(config);
    }

    /// <summary>
    /// オブジェクトの方を向く
    /// </summary>
    /// <param name="target"></param>
    public void LookAt(GameObject target) {
        //あるオブジェクトから見た別のオブジェクトの方向を求める
        var direction = target.transform.position - transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
    }
}
