using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSystem : MonoBehaviour
{
    public bool isDungeon = true;
    public GameObject[] Enemies;
    public int ENEMY_MAX;
    public int SPAWN_WAIT;
    public string[] DUNGEON_DROP_ITEMS_NO;
    public GameObject DamageTextPrefab;
    public GameObject PlayerDamageTextPrefab;
    public int DAMAGE_TEXT_NUM;
    public int PLAYER_DAMAGE_TEXT_NUM;

    List<GameObject> PoolDamageText;
    List<GameObject> PoolPlayerDamageText;
    GameObject[] Floors;
    List<GameObject> ListEnemy = new List<GameObject>();
    GameObject dungeonRoot;
    
    int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //poolにオブジェクトをセット
        PoolDamageText = LoadPrefabs(DamageTextPrefab, DAMAGE_TEXT_NUM);
        PoolPlayerDamageText = LoadPrefabs(PlayerDamageTextPrefab, PLAYER_DAMAGE_TEXT_NUM);
        Floors = GameObject.FindGameObjectsWithTag("Ground");
        DebugWindow.instance.DFDebug("Floor数" + Floors.Length);
        dungeonRoot = SingletonGeneral.instance.dungeonRoot;

        //ダンジョンじゃないときは処理しない
        if (!isDungeon) return;

        //実際に使うときにFPSが1度だけ下がるので、一旦ここで再生をやっておく
        PlayDamageObject(PoolDamageText);
        PlayDamageObject(PoolPlayerDamageText);

        //満腹度スタート
        PlayerView.instance.playerStatusChange.StartSatiation();
        StartCoroutine(IESpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IESpawn() {
        while (true) {
            if (!isDungeon) break;
            yield return new WaitForSeconds(SPAWN_WAIT);
            Spawn();
        }
    }

    //敵キャラをスポーンする
    void Spawn() {
        if (canSpawn()) {
            int floorNo = Random.Range(0, Floors.Length);
            int enemyNo = Random.Range(0, Enemies.Length);
            Vector3 pos = Floors[floorNo].transform.position;
            pos.y = 1.0f;
            GameObject enemy = Instantiate(Enemies[enemyNo], pos, transform.rotation);
            enemy.transform.eulerAngles = ChangeDirection(enemy);
            enemy.transform.parent = dungeonRoot.transform;
            ListEnemy.Add(enemy);
        }
    }
    Vector3 ChangeDirection(GameObject obj) {
        int rnd = Random.Range(0, 359);
        Vector3 worldAngle = obj.transform.eulerAngles;
        worldAngle.y = rnd;
        return worldAngle;
    }

    //敵キャラがMAXだったらfalseを返す。
    bool canSpawn() {
        List<GameObject> listObject = DeleteNullList(ListEnemy);
        return (listObject.Count < ENEMY_MAX) ? true : false;

    }

    //死んだ敵はnullになるので、それらを抜いたリストを返す
    List<GameObject> DeleteNullList(List<GameObject> listObject) {
        List<GameObject> listNew = new List<GameObject>();
        foreach (GameObject obj in listObject) {
            if (obj != null) listNew.Add(obj);
        }
        return listNew;
    }

    //そのダンジョンに設定されたアイテムNOをランダムで返す。
    public string GetItemNo() {
        int no = Random.Range(0, DUNGEON_DROP_ITEMS_NO.Length);
        return DUNGEON_DROP_ITEMS_NO[no];
    }

    //poolのDamageTextを全部起動
    public void PlayDamageObject(List<GameObject> Pool) {
        //見えないところで
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        Quaternion r = transform.rotation;
        foreach (GameObject obj in Pool) {
            obj.GetComponent<TMP_AlphaAndDestroy>().SetDamage(0, pos, r, 1f);
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

    /// <summary>
    /// HUDが有効かを返す
    /// </summary>
    /// <returns></returns>
    public bool GetIsDungeon() {
        return isDungeon;
    }

    List<GameObject> LoadPrefabs(GameObject obj, int max) {
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < max; i++) {
            pool.Add(Instantiate(obj));
        }
        return pool;
    }
}
