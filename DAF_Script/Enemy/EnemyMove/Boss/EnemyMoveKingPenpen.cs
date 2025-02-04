using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveKingPenpen : EnemyMoveParent, IEnemyMove
{
    int walkCount = 0;
    int MAX_WALK_COUNT = 600;
    bool canWalk = true;
    bool isLook = false;
    const float CAN_WALK_INTERVAL_AFTER_SHOT = 6f;
    const string ANIMATION_ATTACK = "Attack 01";
    [SerializeField] private GameObject spawnEnemy;

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
                break;
            case "Notice":
                WhenNotice(dist, player, config, enemyAnimation);
                break;
            case "Battle":
                WhenBattle(dist, player, config, enemyAnimation);
                if (isLook) LookAt(player);
                break;
        }
    }
    public override void WhenWalk(
        float dist,
        EnemyConfig enemyConfig) 
    {
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        if (nowState == STATE_TYPE.NOTICE) ChangeNoticeState(enemyConfig);
    }

    public override void ChangeNoticeState(EnemyConfig enemyConfig) {
        enemyConfig.SetEnemyState("Notice");
        DebugWindow.instance.DFDebug("StateCange:Notice");
    }

    public override void WhenNotice(
        float dist, 
        GameObject player,
        EnemyConfig enemyConfig, 
        EnemyAnimation enemyAnimation) 
    {
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
        StartCoroutine(Attack(config.ATK_INTERVAL));
    }


    public override void WhenBattle(
        float dist,
        GameObject player,
        EnemyConfig enemyConfig,
        EnemyAnimation enemyAnimation) 
    {
        STATE_TYPE nowState = DetermineStatusByDistance(dist, enemyConfig);
        RandomWalk(enemyConfig);
    }


    void RandomWalk(EnemyConfig enemyConfig) {
        if (!canWalk) return;

        if (walkCount == 0) {
            ChangeDirection();
        }
        else {
            WorkForward(enemyConfig.GetWalkSpeed());
        }

        walkCount++;
        if (walkCount >= MAX_WALK_COUNT) walkCount = 0;
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

    IEnumerator Attack(float waitTime) {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            canWalk = false;

            float randomValue = Random.Range(0f, 1f);

            if (randomValue < 0.5f) {
                AttackShot(enemyView);
            }
            else {
                Spawn(spawnEnemy, enemyAnimation);
            }
        }
    }

    void AttackShot(EnemyView view) {
        isLook = true;
        enemyView.enemyAnimation.SetWalkAnim(false);
        enemyAttack.StartAttack(view);
        StartCoroutine(ChangeCanWalk(CAN_WALK_INTERVAL_AFTER_SHOT));
    }

    void Spawn(GameObject enemy, EnemyAnimation enemyAnimation) {
        
        GameObject instancedEnemy = Instantiate(
            enemy,
            GetRandomPositionWithinRadius(gameObject.transform.position, 5f), 
            GetRandomYRotation());
        enemyAnimation.setAttackAnim(ANIMATION_ATTACK);
    }

    /// <summary>
    /// 指定したpositionからradiusの範囲でランダムな位置を返す。
    /// ただしyは+1mで固定
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    Vector3 GetRandomPositionWithinRadius(Vector3 origin, float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0; // y方向の成分を0にリセット
        Vector3 randomPosition = origin + randomDirection;
        randomPosition.y += 1f; // y座標を+1mに固定
        return randomPosition;
    }

    /// <summary>
    /// yがランダムなQuaternionを返す
    /// </summary>
    /// <returns></returns>
    Quaternion GetRandomYRotation() {
        float randomYAngle = Random.Range(0f, 360f); // 0から360度の範囲でランダムなy軸の角度を生成
        Quaternion randomYRotation = Quaternion.Euler(0, randomYAngle, 0); // y軸の角度を使用してQuaternionを生成
        return randomYRotation;
    }

    IEnumerator ChangeCanWalk(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        canWalk = true;
        isLook = false;
    }

}
