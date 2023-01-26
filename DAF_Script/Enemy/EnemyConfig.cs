using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
    public int MAX_HP;
    public int MAX_MP;
    public int ATK;
    public int DEF;
    public float SPD;
    public float ATK_INTERVAL;
    public string ENEMY_STATE;
    public float HIGHT;
    public float DELETE_TIME;
    public float WALK_SPEED;
    public float NOTICE_DISTANCE;
    public float BATTLE_DISTANCE;
    public float BATTLE_END_DISTANCE;
    public GameObject PREFAB_ATTACK;
    public GameObject PREFAB_DIE_EFFECT;

    private int nowHp;
    private int nowMp;

    

    private void Start() {
        nowHp = MAX_HP;
        nowMp = MAX_MP;
    }

    public int calcHp(int damage) {
        nowHp = nowHp - damage;
        if (nowHp < 0) {
            nowHp = 0;
        }
        return nowHp;
    }

    public int GetHP() {
        return nowHp;
    }
    public void SetHP(int setHp) {
        nowHp = setHp;
    }
    public int GetMaxHP() {
        return MAX_HP;
    }
    public int GetMP() {
        return nowMp;
    }
    public void SetMP(int setMp) {
        nowMp = setMp;
    }
    public int GetMaxMP() {
        return MAX_MP;
    }

    public int GetATK() {
        return ATK;
    }
    public int GetDEF() {
        return DEF;
    }
    public float GetSPD() {
        return SPD;
    }
    public float GetATKInterval() {
        return ATK_INTERVAL;
    }

    public float GetHight() {
        return HIGHT;
    }
    public float GetDeleteTime() {
        return DELETE_TIME;
    }

    public float GetWalkSpeed() {
        return WALK_SPEED;
    }
    public float GetNoticeDistance() {
        return NOTICE_DISTANCE;
    }
    public float GetBattleDistance() {
        return BATTLE_DISTANCE;
    }
    public float GetBattleEndDistance() {
        return BATTLE_END_DISTANCE;
    }
    public GameObject GetPrefabAttack() {
        return PREFAB_ATTACK;
    }
    public GameObject GetPrefabEnemyDieEffect() {
        return PREFAB_DIE_EFFECT;
    }

    public string GetEnemyState() {
        return ENEMY_STATE;
    }


    public void SetATK(int atk) {
        ATK = atk;
    }
    public void SetDEF(int def) {
        DEF = def;
    }
    public void SetSPD(float spd) {
        SPD = spd;
    }
    public void SetATKInterval(int atkInterval) {
        ATK_INTERVAL = atkInterval;
    }
    public void SetEnemyState(string state) {
        ENEMY_STATE = state;
    }
    

    

}
