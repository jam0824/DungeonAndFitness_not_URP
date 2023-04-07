using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackKingPenpen : MonoBehaviour, IEnemyAttack
{
    const string firePositionPath = "EnemyAttachment/FirePosition";

    //çUåÇäJén
    public void StartAttack(EnemyView enemyView) {
        EnemyConfig config = GetComponent<EnemyConfig>();
        GameObject shot = MakeShotObject(config);
        SingletonGeneral.instance.PlayOneShot(
            enemyView.audioSource,
            "NormalShotAppear");
        EnemyBullet enemyBullet = SetEnemyBullet(shot, config, enemyView);
    }

    

    GameObject MakeShotObject(EnemyConfig config) {
        GameObject ball = config.GetPrefabAttack();
        GameObject shot = Instantiate(ball, GetShotPos(), Quaternion.identity);
        shot.transform.parent = SingletonGeneral.instance.dungeonRoot.transform;
        return shot;
    }

    Vector3 GetShotPos() {
        GameObject firePosition = transform.Find(firePositionPath).gameObject;
        return firePosition.transform.position;
    }

    EnemyBullet SetEnemyBullet(
        GameObject shot, 
        EnemyConfig config, 
        EnemyView enemyView)
    {
        EnemyBullet enemyBullet = shot.GetComponent<EnemyBullet>();
        enemyBullet.player = enemyView.Face;
        enemyBullet.SetSPD(config.GetSPD());
        enemyBullet.SetATK(config.GetATK());
        enemyBullet.SetEnemyGameObject(gameObject);
        enemyBullet.SetWaitTime(config.GetBulletWaitTime());
        enemyBullet.SetLocalSize(config.GetBulletLocalSize());
        enemyBullet.enemyConfig = enemyView.enemyConfig;
        enemyBullet.firePosition = transform.Find(firePositionPath).gameObject;
        return enemyBullet;
    }

    //çUåÇèIóπ
    public void StopAttack() {

    }
    public IEnumerator MakeBullet(EnemyView enemyView) {
        yield return new WaitForSeconds(1f);
    }
}