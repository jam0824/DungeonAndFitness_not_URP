using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveStop : EnemyMoveParent, IEnemyMove
{
    /// <summary>
    /// ActionModeがStopの時
    /// </summary>
    /// <param name="state"></param>
    /// <param name="dist"></param>
    public override void ActionMode(string state, float dist) {
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

}
