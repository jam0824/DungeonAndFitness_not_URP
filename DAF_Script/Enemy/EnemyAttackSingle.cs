using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSingle : MonoBehaviour, IEnemyAttack
{
    IEnumerator routine;

    //çUåÇäJén
    public void StartAttack(EnemyView enemyView) {
        routine = MakeBullet(enemyView);
        StartCoroutine(routine);
    }

    //çUåÇèIóπ
    public void StopAttack() {
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
        Vector3 pos = transform.position;
        Vector3 addPos = -transform.forward;
        pos = pos + addPos;
        pos.y += 3.5f;
        return pos;
    }

    EnemyBullet SetEnemyBullet(
        GameObject shot, 
        EnemyConfig config, 
        EnemyView enemyView)
    {
        EnemyBullet enemyBullet = shot.GetComponent<EnemyBullet>();
        int atk = config.GetATK();
        float spd = config.GetSPD();
        enemyBullet.player = enemyView.Face;
        enemyBullet.SetSPD(spd);
        enemyBullet.SetATK(atk);
        enemyBullet.SetEnemyGameObject(gameObject);
        enemyBullet.enemyConfig = enemyView.enemyConfig;
        return enemyBullet;
    }
}