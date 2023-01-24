using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSystem : MonoBehaviour
{
    public GameObject DamageTextPrefab;
    public GameObject PlayerDamageTextPrefab;
    public int DAMAGE_TEXT_NUM;
    public int PLAYER_DAMAGE_TEXT_NUM;

    List<GameObject> PoolDamageText;
    List<GameObject> PoolPlayerDamageText;

    // Start is called before the first frame update
    void Start()
    {
        //poolにオブジェクトをセット
        PoolDamageText = LoadPrefabs(DamageTextPrefab, DAMAGE_TEXT_NUM);
        PoolPlayerDamageText = LoadPrefabs(PlayerDamageTextPrefab, PLAYER_DAMAGE_TEXT_NUM);

        //実際に使うときにFPSが1度だけ下がるので、一旦ここで再生をやっておく
        PlayDamageObject(PoolDamageText);
        PlayDamageObject(PoolPlayerDamageText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //poolのDamageTextを全部起動
    public void PlayDamageObject(List<GameObject> Pool) {
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
