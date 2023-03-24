using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveStop_Notice_Move : EnemyMoveParent, IEnemyMove
{
    [HideInInspector] public enum MOVE_TYPE { Walk, Run };
    public MOVE_TYPE type;

    public bool isWhenNotNoticeWalk = true;
    public bool isWhenNoticeWalk = true;
    public bool isWhenBattleWlak = true;




    /// <summary>
    /// ActionModeがRandomの時
    /// </summary>
    /// <param name="state"></param>
    /// <param name="dist"></param>
    public override void ActionMode(string state, float dist) {
        GameObject player = enemyView.Player;
        switch (state) {
            case "Walk":
                WhenWalk(dist, config);
                if (isWhenNotNoticeWalk) Walk(config);
                break;
            case "Notice":
                WhenNotice(dist, player, config, enemyAnimation);
                if (isWhenNoticeWalk) Walk(config);
                break;
            case "Battle":
                WhenBattle(dist, player, config, enemyAnimation);
                if (isWhenBattleWlak) Walk(config);
                break;
        }
        STATE_TYPE nowState = DetermineStatusByDistance(dist, config);
        if (nowState == STATE_TYPE.WALK) {
            //通常時歩くONだけど歩くアニメになってないなら
            if ((isWhenNotNoticeWalk) && (!GetBoolMoveAnimation()))
                ChangeMoveAnimation(true);
        }
    }
    public override void WhenWalk(
        float dist,
        EnemyConfig enemyConfig) {
        //通常時歩くOFFだけど、歩くアニメになっているなら
        if ((!isWhenNotNoticeWalk) && (GetBoolMoveAnimation()))
            ChangeMoveAnimation(false);
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.NOTICE) {
            //MakeNoticeEffect();
            ChangeNoticeState(enemyConfig);
        }
    }

    public override void WhenNotice(
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
    public override void ChangeBattleState(EnemyConfig enemyConfig) {
        enemyConfig.SetEnemyState("Battle");
        DebugWindow.instance.DFDebug("StateCange:Battle");
        if (!isWhenBattleWlak) ChangeMoveAnimation(false);
        enemyAttack.StartAttack(enemyView);
    }


    public override void WhenBattle(
        float dist,
        GameObject player,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) {
        LookAt(player);
        //バトル時歩かない設定かつ歩いているアニメなら
        if ((!isWhenBattleWlak) && (GetBoolMoveAnimation()))
            ChangeMoveAnimation(false);

        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.WALK) {
            enemyAttack.StopAttack();
            ChangeWalkState(enemyConfig);
        }
    }

    public override void ChangeWalkState(EnemyConfig enemyConfig) {
        enemyConfig.SetEnemyState("Walk");
        DebugWindow.instance.DFDebug("StateCange:Walk");
        if (!isWhenNotNoticeWalk) ChangeMoveAnimation(false);
    }


    void Walk(EnemyConfig enemyConfig) {
        WorkForward(enemyConfig.GetWalkSpeed());
    }

    /// <summary>
    /// 前に歩く
    /// </summary>
    /// <param name="walkSpeed"></param>
    void WorkForward(float walkSpeed) {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
        if (!GetBoolMoveAnimation()) {
            ChangeMoveAnimation(true);
        }
    }

    void ChangeMoveAnimation(bool isMove) {
        switch (type) {
            case MOVE_TYPE.Run:
                enemyView.enemyAnimation.SetRunAnim(isMove);
                break;
            case MOVE_TYPE.Walk:
                enemyView.enemyAnimation.SetWalkAnim(isMove);
                break;
        }
    }

    bool GetBoolMoveAnimation() {
        switch (type) {
            case MOVE_TYPE.Run:
                return enemyView.enemyAnimation.GetBoolRunAnim();
            case MOVE_TYPE.Walk:
                return enemyView.enemyAnimation.GetBoolWalkAnim();
        }
        return false;
    }
}