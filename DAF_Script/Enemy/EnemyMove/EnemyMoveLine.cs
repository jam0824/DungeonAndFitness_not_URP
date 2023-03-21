using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveLine : EnemyMoveParent, IEnemyMove
{



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
                Walk(config);
                break;
            case "Notice":
                WorkForward(config.GetWalkSpeed());
                WhenNotice(dist, player, config, enemyAnimation);
                break;
            case "Battle":
                WhenBattle(dist, player, config, enemyAnimation);
                
                break;
        }
        STATE_TYPE nowState = DetermineStatusByDistance(dist, config);
        if (nowState == STATE_TYPE.WALK) enemyAnimation.SetRunAnim(true);
    }
    public override void WhenWalk(
        float dist,
        EnemyConfig enemyConfig) {
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
        EnemyAnimation enemyAnimation) 
    {
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
        enemyAnimation.SetRunAnim(false);
        enemyAttack.StartAttack(enemyView);
    }


    public override void WhenBattle(
        float dist,
        GameObject player,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) 
    {
        LookAt(player);
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.WALK) {
            enemyAttack.StopAttack();
            ChangeWalkState(enemyConfig);
        }
    }


    void Walk(EnemyConfig enemyConfig) {
        WorkForward(enemyConfig.GetWalkSpeed());
    }

    void ChangeDirection() {
        Vector3 worldAngle = transform.eulerAngles;
        worldAngle.y *= -1;
        transform.eulerAngles = worldAngle;
    }

    /// <summary>
    /// 前に歩く
    /// </summary>
    /// <param name="walkSpeed"></param>
    void WorkForward(float walkSpeed) {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
        if (enemyView.enemyAnimation.GetBoolRunAnim() == false) {
            enemyView.enemyAnimation.SetRunAnim(true);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Wall") {
            ChangeDirection();
        }
    }
}
