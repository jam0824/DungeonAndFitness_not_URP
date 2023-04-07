using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSingle : MonoBehaviour, IEnemyAttack
{
    IEnumerator routine;
    const string firePositionPath = "EnemyAttachment/FirePosition";

    //çUåÇäJén
    public void StartAttack(EnemyView enemyView) {
        routine = MakeBullet(enemyView);
        StartCoroutine(routine);
    }

    //çUåÇèIóπ
    public void StopAttack() {
        if (routine == null) return;
        StopCoroutine(routine);
        routine = null;
    }

    public IEnumerator MakeBullet(EnemyView enemyView) {
        while (true) {
            EnemyConfig config = GetComponent<EnemyConfig>();
            float waitTime = config.GetATKInterval();
            yield return new WaitForSeconds(waitTime);

            GameObject shot = MakeShotObject(config);
            SingletonGeneral.instance.PlayOneShot(
                enemyView.audioSource, 
                "NormalShotAppear");

            EnemyBullet enemyBullet = SetEnemyBullet(shot, config, enemyView);
        }
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
}