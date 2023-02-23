using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    DataPlayer dataPlayer;

    public int MAX_HP;
    public int MAX_MP;
    public int ATK;
    public int DEF;
    public float DEC_SATIATION;
    public int MAX_PAGE_NO;
    public GameObject PUNCH_HIT_PREFAB;
    public GameObject PUNCH_ENABLE_PREFAB;

    private void Awake() {
        
    }

    public void PlayerConfigInit() {
        dataPlayer = DataSystem.instance.dataPlayer;
        if (dataPlayer.nowHp != 0) return;

        dataPlayer.nowHp = MAX_HP;
        dataPlayer.nowMp = MAX_MP;
        dataPlayer.MAX_HP = MAX_HP;
        dataPlayer.MAX_MP = MAX_MP;
        dataPlayer.ATK = ATK;
        dataPlayer.DEF = DEF;
        dataPlayer.DEC_SATIATION = DEC_SATIATION;
        dataPlayer.MAX_PAGE_NO = MAX_PAGE_NO;
        ResetSatiation();
    }

    public int GetHP() {
        return dataPlayer.nowHp;
    }
    public void SetHP(int setHp) {
        dataPlayer.nowHp = setHp;
    }
    public int GetMaxHP() {
        return dataPlayer.MAX_HP;
    }
    public int GetMP() {
        return dataPlayer.nowMp;
    }
    public void SetMP(int setMp) {
        dataPlayer.nowMp = setMp;
    }
    public int GetMaxMP() {
        return dataPlayer.MAX_MP;
    }

    public int GetATK() {
        return dataPlayer.ATK;
    }
    public int GetDEF() {
        return dataPlayer.DEF;
    }
    public int GetMaxPageNo() {
        return dataPlayer.MAX_PAGE_NO;
    }

    public float GetSatiation() {
        return dataPlayer.nowSatiation;
    }

    public float GetDecreaseSatiation() {
        return dataPlayer.DEC_SATIATION;
    }

    public void SetMaxHp(int maxHp) {
        dataPlayer.MAX_HP = maxHp;
    }

    public void SetMaxMp(int maxMp) {
        dataPlayer.MAX_MP = maxMp;
    }

    public void SetHp(int hp) {
        dataPlayer.nowHp = hp;
    }
    public void SetMp(int mp) {
        dataPlayer.nowMp = mp;
    }

    public void SetAtk(int atk) {
        dataPlayer.ATK = atk;
    }
    public void SetDef(int def) {
        dataPlayer.DEF = def;
    }
    public void SetSatiation(float satiation) {
        dataPlayer.nowSatiation = satiation;
    }
    public void ResetSatiation() {
        dataPlayer.nowSatiation = 100f;
        return;
    }

    public void SetMaxPageNo(int pageNo) {
        dataPlayer.MAX_PAGE_NO = pageNo;
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
        dataPlayer.nowHp = dataPlayer.nowHp - damage;
        if(dataPlayer.nowHp < 0) {
            dataPlayer.nowHp = 0;
        }
        return dataPlayer.nowHp;
    }

    public float CalcSatiation(float value) {
        dataPlayer.nowSatiation += value;
        if (dataPlayer.nowSatiation < 0) dataPlayer.nowSatiation = 0;
        if (dataPlayer.nowSatiation > 100f) dataPlayer.nowSatiation = 100f;
        return dataPlayer.nowSatiation;
    }

    /// <summary>
    /// –ž• “x‚ð‰ñ•œ‚·‚é
    /// </summary>
    /// <param name="recoverPercent"></param>
    public void RecoverSatiation(float recoverPercent) {
        float recover = 100f * recoverPercent;
        dataPlayer.nowSatiation += recover;
        if (dataPlayer.nowSatiation > 100f) dataPlayer.nowSatiation = 100f;
    }
}
