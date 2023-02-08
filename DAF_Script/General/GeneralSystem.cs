using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSystem : MonoBehaviour
{
    public bool DEBUG_MODE;
    public string NORMAL_ITEM_SAVE_PATH;
    public string COLLECTION_ITEM_SAVE_PATH;
    public GameObject ItemCanvas;
    public ItemView itemWindow { set; get; }
    public GameObject ItemBoxPrefab;
    public ItemBox itemBox { set; get; }
    public LabelSystem labelSystem { set; get; }
    public LabelInformationText labelInformationText { set; get; }

    public GameObject DamageTextCanvas;
    public GameObject PlayerDamageTextCanvas;
    public GameObject NoticeTextCanvas;
    GameObject player;
    GameObject face;
    GameObject rightPlayerPunch;
    GameObject leftPlayerPunch;
    public PlayerConfig playerConfig { set; get; }
    public PlayerView playerView { set; get; }
    public ItemDB itemDb { set; get; }

    public List<AudioClip> listBattleSE;

    Dictionary<string, int> dictSeName;

    



    public AudioSource audioSource { set; get; }
    public string LanguageMode { set; get; }


    void Awake() {
        //OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
        LanguageMode = "japanese";
        GeneralInit();
        SetDictSeName();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //全体的な初期化
    void GeneralInit() {
        //Playerのコンポーネントなど取得
        LoadPlayerObject();

        //Labelのロード
        labelSystem = GetComponent<LabelSystem>();
        labelSystem.LabelSystemInit(LanguageMode);

        //ItemDBのロード
        itemDb = GetComponent<ItemDB>();
        itemDb.ItemDbInit(this);

        //ItemWindowの事前作成
        LoadItemWindow();

        //ItemBoxの事前作成
        LoadItemBox();

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
    public void PlayOneShotNoAudioWithClip(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }

    public string GetNormalItemSavePath() {
        return NORMAL_ITEM_SAVE_PATH;
    }
    public string GetCollectionItemSavePath() {
        return COLLECTION_ITEM_SAVE_PATH;
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
        dictSeName.Add("NormalItemGet", 12);
        dictSeName.Add("BlowOff", 13);
        dictSeName.Add("BlowOffAndHitWall", 14);

        return dictSeName;
    }

    //ターゲットから特定の場所にインスタンスを作る
    public GameObject MakeInstanceFromTarget(
        GameObject targetGameObject,
        GameObject prefab, 
        Vector3 addPos
     ) {
        Vector3 pos = GetPosFromTarget(targetGameObject, addPos);
        Quaternion r = GetQuaternionFace();

        GameObject returnPrefab = Instantiate(prefab,pos,r);
        return returnPrefab;
    }

    //ターゲットのgameObjectの前からaddPosだけ離れたところのposを取得する
    public Vector3 GetPosFromTarget(GameObject targetGameObject, Vector3 addPos) {
        Vector3 pos = targetGameObject.transform.position;
        Vector3 forward = targetGameObject.transform.forward;
        forward.x *= addPos.x;
        forward.y *= addPos.y;
        forward.z *= addPos.z;
        pos += forward;
        return pos;
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

    //顔とターゲットのgameObjectの中点posを返す
    public Vector3 GetPosBetweenTargetAndFace(GameObject targetGameObject, Vector3 addPos) {
        Vector3 facePos = face.transform.position;
        Vector3 targetPos = targetGameObject.transform.position;

        return new Vector3(
            ((targetPos.x + facePos.x) / 2) + addPos.x,
            facePos.y + addPos.y,
            ((targetPos.z + facePos.z) / 2) + addPos.z
            );
    }

    //顔の回転を取得する
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

    //メニューを作って非アクティブにしておく
    void LoadItemWindow() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject ItemWindowObject = Instantiate(ItemCanvas, pos, transform.rotation);
        itemWindow = ItemWindowObject.GetComponent<ItemView>();
        itemWindow.ItemViewInit();
        ItemWindowObject.SetActive(false);
    }

    

    //アイテムボックスを作って非アクティブにしておく
    void LoadItemBox() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject itemBoxObject = Instantiate(ItemBoxPrefab, pos, transform.rotation);
        itemBox = itemBoxObject.GetComponent<ItemBox>();
        itemBox.ItemBoxInit();
        itemBoxObject.SetActive(false);
        itemBox.UnableItemBox();
    }

    void LoadPlayerObject() {
        player = GameObject.Find("Player");
        face = GameObject.Find("HitArea");
        playerConfig = player.GetComponent<PlayerConfig>();
        playerView = player.GetComponent<PlayerView>();
        labelInformationText = 
            GameObject.Find("InformationText").
            GetComponent<LabelInformationText>();
    }

    //コントローラーを振動させる
    public void VivrationController(string objName, float frequency, float amplitude, float waitTime) {
        StartCoroutine(Vivration(objName.ToLower(), frequency, amplitude, waitTime));
    }
    //コントローラーを動かして止めるためのコールチン
    IEnumerator Vivration(string objName, float frequency, float amplitude, float waitTime) {
        OVRInput.Controller controller = (objName.Contains("left")) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(waitTime);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
