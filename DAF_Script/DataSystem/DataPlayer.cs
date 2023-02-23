using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{

    public int MAX_HP { set; get; }
    public int MAX_MP { set; get; }
    public int ATK { set; get; }
    public int DEF { set; get; }
    public float DEC_SATIATION { set; get; }
    public int MAX_PAGE_NO { set; get; }
    public int nowHp { set; get; }
    public int nowMp { set; get; }
    public float nowSatiation { set; get; }

    public void DataPlayerInit() {
        MAX_HP = 0;
        MAX_MP = 0;
        ATK = 0;
        DEF = 0;
        DEC_SATIATION = 0;
        MAX_PAGE_NO = 2;
        nowHp = 0;
        nowMp = 0;
        nowSatiation = 0;
    }

}
