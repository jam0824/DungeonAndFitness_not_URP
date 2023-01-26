using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSystem : MonoBehaviour
{
    public GameObject ItemCanvas;
    public GameObject DamageTextCanvas;
    public GameObject PlayerDamageTextCanvas;
    public GameObject NoticeTextCanvas;
    GameObject player;
    GameObject face;
    GameObject rightPlayerPunch;
    GameObject leftPlayerPunch;
    public GameObject ItemWindow { set; get; }

    public List<AudioClip> listBattleSE;

    Dictionary<string, int> dictSeName;

    ItemDB itemDb;

    public AudioSource audioSource { set; get; }


    void Awake() {
        //OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
        GeneralInit();
        SetDictSeName();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //全体的な初期化
    void GeneralInit() {
        //ItemDBのロード
        itemDb = GetComponent<ItemDB>();
        itemDb.ItemDbInit();

        //ItemWindowの事前作成
        LoadItemWindow();

    }

    public GameObject GetPlayerPrefab() {
        return player;
    }
    public void SetPlayerPrefab(GameObject p) {
        this.player = p;
    }
    public GameObject GetFacePrefab() {
        return face;
    }
    public void SetFacePrefab(GameObject f) {
        this.face = f;
    }
    public GameObject GetRightArmorPrefab() {
        return rightPlayerPunch;
    }
    public void SetRightArmorPrefab(GameObject rightArmorPrefab) {
        this.rightPlayerPunch = rightArmorPrefab;
    }
    public GameObject GetLeftArmorPrefab() {
        return leftPlayerPunch;
    }
    public void SetLeftArmorPrefab(GameObject leftArmorPrefab) {
        this.leftPlayerPunch = leftArmorPrefab;
    }

    public GameObject GetItemCanvas() {
        return ItemCanvas;
    }
    public GameObject GetPrefabDamageTextCanvas() {
        return DamageTextCanvas;
    }
    public GameObject GetPrefabPlayerDamageTextCanvas() {
        return PlayerDamageTextCanvas;
    }
    public GameObject GetPrefabNoticeTextCanvas() {
        return NoticeTextCanvas;
    }


    public void PlayOneShot(AudioSource audioSource, string SeName) {
        AudioClip audioClip = GetSE(SeName);
        audioSource.PlayOneShot(audioClip);
    }
    public void PlayOneShotNoAudioSource(string SeName) {
        AudioClip audioClip = GetSE(SeName);
        audioSource.PlayOneShot(audioClip);
    }

    AudioClip GetSE(string SeName) {
        int no = dictSeName[SeName];
        return listBattleSE[no];
    }

    Dictionary<string, int> SetDictSeName() {
        dictSeName = new Dictionary<string, int>();
        dictSeName.Add("NormalShot", 0);
        dictSeName.Add("NormalHitToPlayer", 1);
        dictSeName.Add("NormalHitToEnemy", 2);
        dictSeName.Add("PunchEnableSE", 3);
        dictSeName.Add("EnemyNoticeSE", 4);
        dictSeName.Add("NormalEnemyDie", 5);
        dictSeName.Add("NormalShotAppear", 6);
        dictSeName.Add("ItemBigSelect", 7);
        dictSeName.Add("ItemSmallSelect", 8);
        dictSeName.Add("ItemMenuClose", 9);
        dictSeName.Add("MessageNormal", 10);
        dictSeName.Add("NormalFoot", 11);

        return dictSeName;
    }

    //ターゲットから特定の場所にインスタンスを作る
    public GameObject MakeInstanceFromTarget(
        GameObject targetGameObject,
        GameObject prefab, 
        Vector3 addPos
     ) {

        Vector3 pos = targetGameObject.transform.position;
        pos += addPos;
        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        GameObject returnPrefab = Instantiate(prefab,pos,r);
        return returnPrefab;
    }

    //ターゲットとfaceの中間点にインスタンスを作る
    public GameObject MakeInstanceBetweenTargetAndFace(
        GameObject targetGameObject,
        GameObject prefab,
        Vector3 addPos
     ) {
        Vector3 makePos = GetPosBetweenTargetAndFace(targetGameObject, addPos);
        Quaternion r = GetQuaternionFace();
        GameObject returnPrefab = Instantiate(prefab, makePos, r);
        return returnPrefab;
    }

    public Vector3 GetPosBetweenTargetAndFace(GameObject targetGameObject, Vector3 addPos) {
        Vector3 facePos = face.transform.position;
        Vector3 targetPos = targetGameObject.transform.position;

        return new Vector3(
            ((targetPos.x + facePos.x) / 2) + addPos.x,
            facePos.y + addPos.y,
            ((targetPos.z + facePos.z) / 2) + addPos.z
            );
    }

    public Quaternion GetQuaternionFace() {
        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        return r;
    }

    public void LookAt(GameObject target, GameObject me) {
        //あるオブジェクトから見た別のオブジェクトの方向を求める
        var direction = target.transform.position - me.transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        me.transform.rotation = Quaternion.Lerp(me.transform.rotation, lookRotation, 0.1f);
    }

    void LoadItemWindow() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        ItemWindow = Instantiate(ItemCanvas, pos, transform.rotation);
        ItemWindow.SetActive(false);
    }
}
