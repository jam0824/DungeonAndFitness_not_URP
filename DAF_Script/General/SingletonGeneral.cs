using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGeneral : MonoBehaviour
{
    public static SingletonGeneral instance;
    public string LanguageMode { set; get; }
    public GameObject player { set; get; }
    public GameObject face { set; get; }
    public PlayerConfig playerConfig{set;get;}
    public PlayerView playerView { set; get; }
    public LabelInformationText labelInformationText { set; get; }
    public AudioSource audioSource { set; get; }
    public ItemBox itemBox { set; get; }

    public ItemView itemWindow { set; get; }
    public LabelSystem labelSystem { set; get; }
    public ItemDB itemDb { set; get; }
    public ScenarioSystem scenarioSystem { set; get; }
    public SaveLoadSystem saveLoadSystem { set; get; }

    public GameObject dungeonRoot { set; get; }


    public bool DEBUG_MODE;
    public string STATUS_SAVE_PATH;
    public string NORMAL_ITEM_SAVE_PATH;
    public string COLLECTION_ITEM_SAVE_PATH;
    public string SWITCH_SAVE_PATH;
    public string KANTEI_ITEM_NO;
    public string dugeonRootName;
    public GameObject ItemCanvas;
    public GameObject DamageTextCanvas;
    public GameObject PlayerDamageTextCanvas;
    public GameObject ItemBoxPrefab;
    

    public List<AudioClip> listBattleSE;
    public List<GameObject> listEffectPrefab;

    Dictionary<string, int> dictSeName;
    Dictionary<string, int> dictEffectName;
    Vector3 dungeonRootPos;

    private void Awake() {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            SingletonGeneralInit();
        }
        else {
            //Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //デバッグ
        //saveLoadSystem.Load();
    }

    void SingletonGeneralInit() {
        LanguageMode = "japanese";
        dungeonRoot = GameObject.Find(dugeonRootName);
        dungeonRootPos = dungeonRoot.transform.position;
        saveLoadSystem = GetComponent<SaveLoadSystem>();

        itemDb = LoadItemDb();
        itemWindow = LoadItemWindow();

        LoadPlayerObject();

        labelSystem = LoadLabelSystem();

        itemBox = LoadItemBox();

        scenarioSystem = LoadScenarioSystem();

        SetDictSeName();
        SetDictEffectName();

        //ダンジョンがずれたときに補正する
        StartCoroutine(FixDungeonRootPosCorutine(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ダンジョンがズレたときに補正する
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator FixDungeonRootPosCorutine(float waitTime) {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            FixDungeonRootPos();
        }
    }
    void FixDungeonRootPos() {
        if (dungeonRoot.transform.position == dungeonRootPos) return;

        
        Vector3 pos = dungeonRoot.transform.position;
        Vector3 playerPos = player.transform.position;
        float x = (pos.x > dungeonRootPos.x) ? -0.01f : 0.01f;
        float z = (pos.z > dungeonRootPos.z) ? -0.01f : 0.01f;
        if ((Mathf.Abs(pos.x - dungeonRootPos.x)) > 0.01f) {
            pos.x += x;
            playerPos.x += x;
        }
        else {
            pos.x = dungeonRootPos.x;
        }

        if ((Mathf.Abs(pos.z - dungeonRootPos.z)) > 0.01f) {
            pos.z += z;
            playerPos.z += z;
        }

        else {
            pos.z = dungeonRootPos.z;
        }
        dungeonRoot.transform.position = pos;
        player.transform.position = playerPos;
        //DebugWindow.instance.DFDebug("Dungeon:" + dungeonRoot.transform.position);
        
    }

    public string GetStatusSavePath() {
        return STATUS_SAVE_PATH;
    }

    public string GetNormalItemSavePath() {
        return NORMAL_ITEM_SAVE_PATH;
    }
    public string GetCollectionItemSavePath() {
        return COLLECTION_ITEM_SAVE_PATH;
    }

    public string GetSwitchSavePath() {
        return SWITCH_SAVE_PATH;
    }

    //プレイヤーの外部から必要とされるオブジェクトをロード
    void LoadPlayerObject() {
        player = GameObject.Find("Player");
        face = GameObject.Find("HitArea");
        playerConfig = player.GetComponent<PlayerConfig>();
        playerView = player.GetComponent<PlayerView>();
        audioSource = GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>();
        labelInformationText =
            GameObject.Find("InformationText").
            GetComponent<LabelInformationText>();
    }
    //アイテムボックスを作って非アクティブにしておく
    ItemBox LoadItemBox() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject itemBoxObject = Instantiate(ItemBoxPrefab, pos, transform.rotation);
        ItemBox ib = itemBoxObject.GetComponent<ItemBox>();
        ib.ItemBoxInit();
        itemBoxObject.SetActive(false);
        ib.UnableItemBox();
        return ib;
    }
    //メニューを作って非アクティブにしておく
    ItemView LoadItemWindow() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject ItemWindowObject = Instantiate(ItemCanvas, pos, transform.rotation);
        ItemView iW = ItemWindowObject.GetComponent<ItemView>();
        iW.ItemViewInit();
        ItemWindowObject.SetActive(false);
        return iW;
    }

    ItemDB LoadItemDb() {
        ItemDB iD = GetComponent<ItemDB>();
        //ItemDBの初期化
        iD.ItemDbInit();
        return iD;
    }

    LabelSystem LoadLabelSystem() {
        LabelSystem ls = GetComponent<LabelSystem>();
        //LabelSystemの初期化
        ls.LabelSystemInit(LanguageMode);
        return ls;
    }

    ScenarioSystem LoadScenarioSystem() {
        ScenarioSystem sS = GetComponent<ScenarioSystem>();
        sS.ScenarioSystemInit();
        return sS;
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
        dictSeName.Add("SaveSe", 15);
        dictSeName.Add("HitObjectSe", 16);
        dictSeName.Add("SmallHitToEnemy", 17);
        dictSeName.Add("HardHitToEnemy", 18);
        dictSeName.Add("RocketPunchCombine", 19);
        dictSeName.Add("RocketPunchFire", 20);
        dictSeName.Add("Error", 21);
        dictSeName.Add("RecoverSmall", 22);
        dictSeName.Add("OpenTheBigDoor", 23);
        dictSeName.Add("OpenTheSmallDoor", 24);

        return dictSeName;
    }


    public GameObject GetEffect(string effectName) {
        int no = dictEffectName[effectName];
        return listEffectPrefab[no];
    }

    Dictionary<string, int> SetDictEffectName() {
        dictEffectName = new Dictionary<string, int>();
        dictEffectName.Add("RecoverSmall", 0);

        return dictEffectName;
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



    public void LookAt(GameObject target, GameObject me) {
        //あるオブジェクトから見た別のオブジェクトの方向を求める
        var direction = target.transform.position - me.transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        me.transform.rotation = Quaternion.Lerp(me.transform.rotation, lookRotation, 0.1f);
    }

    //あるオブジェクトから見た別のオブジェクトの方向を求める
    public Quaternion GetQuaternionLookAt(GameObject target, GameObject me) {
        var direction = target.transform.position - me.transform.position;
        direction.y = 0;
        //メッセージだと逆になってしまうので反転させる
        return Quaternion.LookRotation(-direction, Vector3.up);
    }

    /// <summary>
    /// 顔とターゲットのgameObjectの中点posを返す
    /// </summary>
    /// <param name="targetGameObject"></param>
    /// <param name="addPos"></param>
    /// <returns></returns>
    public Vector3 GetPosBetweenTargetAndFace(GameObject targetGameObject, Vector3 addPos) {
        Vector3 facePos = face.transform.position;
        Vector3 targetPos = targetGameObject.transform.position;

        return new Vector3(
            ((targetPos.x + facePos.x) / 2) + addPos.x,
            facePos.y + addPos.y,
            ((targetPos.z + facePos.z) / 2) + addPos.z
            );
    }

    //ターゲットから特定の場所にインスタンスを作る
    public GameObject MakeInstanceFromTarget(
        GameObject targetGameObject,
        GameObject prefab,
        Vector3 addPos
     ) {
        Vector3 pos = GetPosFromTarget(targetGameObject, addPos);
        Quaternion r = GetQuaternionFace();

        GameObject returnPrefab = Instantiate(prefab, pos, r);
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
    //顔の回転を取得する
    public Quaternion GetQuaternionFace() {
        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        return r;
    }

    /// <summary>
    /// targetObjectのローカルの角度にxyzで与えた角度を足したQuaternionを返す
    /// </summary>
    /// <param name="targetObject"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Quaternion GetLocalQuaternionToAddRotation(GameObject targetObject, float x, float y, float z) {
        Vector3 newRotation = targetObject.transform.localRotation.eulerAngles;
        newRotation.x += x;
        newRotation.y += y;
        newRotation.z += z;
        return Quaternion.Euler(newRotation);
    }

}
