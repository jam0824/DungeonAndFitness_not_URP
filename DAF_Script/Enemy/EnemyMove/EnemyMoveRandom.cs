using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveRandom : EnemyMoveParent, IEnemyMove
{
    int walkCount = 0;
    int MAX_WALK_COUNT = 600;



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
                RandomWalk(config);
                break;
            case "Notice":
                WorkForward(config.GetWalkSpeed());
                WhenNotice(dist, player, config, enemyAnimation);
                break;
            case "Battle":
                WhenBattle(dist, player, config, enemyAnimation);
                
                break;
        }
        if((state == "Notice") || (state == "Battle")) {
            if (dist >= config.GetBattleEndDistance())
                enemyAnimation.SetWalkAnim(true);
        }
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

    /// <summary>
    /// 前に歩く
    /// </summary>
    /// <param name="walkSpeed"></param>
    void WorkForward(float walkSpeed) {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
        //enemyView.rigidbody.AddForce(transform.forward * walkSpeed, ForceMode.Force);
        if (enemyView.enemyAnimation.GetBoolWalkAnim() == false) {
            enemyView.enemyAnimation.SetWalkAnim(true);
        }
    }

}
