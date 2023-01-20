using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public int MAX_HP;
    public int MAX_MP;
    public int ATK;
    public int DEF;
    public int MAX_PAGE_NO;
    public GameObject PUNCH_HIT_PREFAB;
    public GameObject PUNCH_ENABLE_PREFAB;
    private int nowHp;
    private int nowMp;

    private void Start() {
        nowHp = MAX_HP;
        nowMp = MAX_MP;
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


    public void SetATK(int atk) {
        ATK = atk;
    }
    public void SetDEF(int def) {
        DEF = def;
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

}
