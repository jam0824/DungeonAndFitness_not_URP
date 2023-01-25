using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSystem : MonoBehaviour
{
    public GameObject[] Enemies;
    public int ENEMY_MAX;
    //fixupdate = 0.02秒　1分で3000
    public int SPAWN_WAIT;
    public GameObject DamageTextPrefab;
    public GameObject PlayerDamageTextPrefab;
    public int DAMAGE_TEXT_NUM;
    public int PLAYER_DAMAGE_TEXT_NUM;

    List<GameObject> PoolDamageText;
    List<GameObject> PoolPlayerDamageText;
    GameObject[] Floors;
    List<GameObject> ListEnemy = new List<GameObject>();

    
    int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //poolにオブジェクトをセット
        PoolDamageText = LoadPrefabs(DamageTextPrefab, DAMAGE_TEXT_NUM);
        PoolPlayerDamageText = LoadPrefabs(PlayerDamageTextPrefab, PLAYER_DAMAGE_TEXT_NUM);
        Floors = GameObject.FindGameObjectsWithTag("Ground");
        DebugWindow.instance.DFDebug("Floor数" + Floors.Length);

        //実際に使うときにFPSが1度だけ下がるので、一旦ここで再生をやっておく
        PlayDamageObject(PoolDamageText);
        PlayDamageObject(PoolPlayerDamageText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        spawnCount += 1;
        if (spawnCount >= SPAWN_WAIT) {
            spawnCount = 0;
            Spawn();
        }
    }

    void Spawn() {
        if (canSpawn()) {
            int floorNo = Random.Range(0, Floors.Length);
            int enemyNo = Random.Range(0, Enemies.Length);
            Vector3 pos = Floors[floorNo].transform.position;
            pos.y = 1.0f;
            GameObject enemy = Instantiate(Enemies[enemyNo], pos, transform.rotation);
            ListEnemy.Add(enemy);
        }
    }

    bool canSpawn() {
        List<GameObject> listObject = DeleteNullList(ListEnemy);
        return (listObject.Count < ENEMY_MAX) ? true : false;

    }

    List<GameObject> DeleteNullList(List<GameObject> listObject) {
        List<GameObject> listNew = new List<GameObject>();
        foreach (GameObject obj in listObject) {
            if (obj != null) listNew.Add(obj);
        }
        return listNew;
    }

    //poolのDamageTextを全部起動
    public void PlayDamageObject(List<GameObject> Pool) {
        //見えないところで
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        Quaternion r = transform.rotation;
        foreach (GameObject obj in Pool) {
            obj.GetComponent<TMP_AlphaAndDestroy>().SetDamage(0, pos, r);
        }
    }

    public GameObject GetDamageTextFromPool() {
        return GetObjectFromPool(PoolDamageText);
    }
    public GameObject GetPlayerDamageTextFromPool() {
        return GetObjectFromPool(PoolPlayerDamageText);
    }

    GameObject GetObjectFromPool(List<GameObject> Pool) {
        foreach (GameObject obj in Pool) {
            if (obj.activeSelf == false) return obj;
        }
        return null;
    }

    List<GameObject> LoadPrefabs(GameObject obj, int max) {
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < max; i++) {
            pool.Add(Instantiate(obj));
        }
        return pool;
    }
}
