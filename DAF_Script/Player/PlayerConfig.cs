using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public int MAX_HP;
    public int MAX_MP;
    public int ATK;
    public int DEF;
    public float DEC_SATIATION;
    public int MAX_PAGE_NO;
    public GameObject PUNCH_HIT_PREFAB;
    public GameObject PUNCH_ENABLE_PREFAB;
    private int nowHp;
    private int nowMp;
    private float nowSatiation = 100.0f;

    private void Start() {
        
    }

    public void PlayerConfigInit() {
        nowHp = MAX_HP;
        nowMp = MAX_MP;
        ResetSatiation();
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
    public int GetMaxPageNo() {
        return MAX_PAGE_NO;
    }

    public float GetSatiation() {
        return nowSatiation;
    }

    public float GetDecreaseSatiation() {
        return DEC_SATIATION;
    }

    public void SetMaxHp(int maxHp) {
        MAX_HP = maxHp;
    }

    public void SetMaxMp(int maxMp) {
        MAX_MP = maxMp;
    }

    public void SetHp(int hp) {
        nowHp = hp;
    }
    public void SetMp(int mp) {
        nowMp = mp;
    }

    public void SetAtk(int atk) {
        ATK = atk;
    }
    public void SetDef(int def) {
        DEF = def;
    }
    public void SetSatiation(float satiation) {
        nowSatiation = satiation;
    }
    public void ResetSatiation() {
        nowSatiation = 100f;
        return;
    }

    public void SetMaxPageNo(int pageNo) {
        MAX_PAGE_NO = pageNo;
    }

    public void SetPunchHitPrefab(GameObject prefab) {
        PUNCH_HIT_PREFAB = prefab;
    }

    

    public GameObject GetPunchHitPrefab() {
        return PUNCH_HIT_PREFAB;
    }

    public GameObject GetPunchEnablePrefab() {
        return PUNCH_ENABLE_PREFAB;
    }

    public int CalcHp(int damage) {
        nowHp = nowHp - damage;
        if(nowHp < 0) {
            nowHp = 0;
        }
        return nowHp;
    }

    public float CalcSatiation(float value) {
        nowSatiation += value;
        if (nowSatiation < 0) nowSatiation = 0;
        if (nowSatiation > 100f) nowSatiation = 100f;
        return nowSatiation;
    }

    /// <summary>
    /// –ž• “x‚ð‰ñ•œ‚·‚é
    /// </summary>
    /// <param name="recoverPercent"></param>
    public void RecoverSatiation(float recoverPercent) {
        float recover = 100f * recoverPercent;
        nowSatiation += recover;
        if (nowSatiation > 100f) nowSatiation = 100f;
    }
}
