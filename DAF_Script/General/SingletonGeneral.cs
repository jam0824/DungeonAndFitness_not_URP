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
    public string dugeonRootName;
    public GameObject ItemCanvas;
    public GameObject DamageTextCanvas;
    public GameObject PlayerDamageTextCanvas;
    public GameObject ItemBoxPrefab;
    

    public List<AudioClip> listBattleSE;

    Dictionary<string, int> dictSeName;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SingletonGeneralInit();
        }
        else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SingletonGeneralInit() {
        LanguageMode = "japanese";
        dungeonRoot = GameObject.Find(dugeonRootName);
        saveLoadSystem = GetComponent<SaveLoadSystem>();

        itemDb = LoadItemDb();
        itemWindow = LoadItemWindow();

        LoadPlayerObject();

        labelSystem = LoadLabelSystem();

        itemBox = LoadItemBox();

        scenarioSystem = LoadScenarioSystem();

        SetDictSeName();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //�v���C���[�̊O������K�v�Ƃ����I�u�W�F�N�g�����[�h
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
    //�A�C�e���{�b�N�X������Ĕ�A�N�e�B�u�ɂ��Ă���
    ItemBox LoadItemBox() {
        Vector3 pos = new Vector3(-10f, -10f, -10f);
        GameObject itemBoxObject = Instantiate(ItemBoxPrefab, pos, transform.rotation);
        ItemBox ib = itemBoxObject.GetComponent<ItemBox>();
        ib.ItemBoxInit();
        itemBoxObject.SetActive(false);
        ib.UnableItemBox();
        return ib;
    }
    //���j���[������Ĕ�A�N�e�B�u�ɂ��Ă���
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
        //ItemDB�̏�����
        iD.ItemDbInit();
        return iD;
    }

    LabelSystem LoadLabelSystem() {
        LabelSystem ls = GetComponent<LabelSystem>();
        //LabelSystem�̏�����
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

        return dictSeName;
    }

    //�R���g���[���[��U��������
    public void VivrationController(string objName, float frequency, float amplitude, float waitTime) {
        StartCoroutine(Vivration(objName.ToLower(), frequency, amplitude, waitTime));
    }
    //�R���g���[���[�𓮂����Ď~�߂邽�߂̃R�[���`��
    IEnumerator Vivration(string objName, float frequency, float amplitude, float waitTime) {
        OVRInput.Controller controller = (objName.Contains("left")) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(waitTime);
        OVRInput.SetControllerVibration(0, 0, controller);
    }



    public void LookAt(GameObject target, GameObject me) {
        //����I�u�W�F�N�g���猩���ʂ̃I�u�W�F�N�g�̕��������߂�
        var direction = target.transform.position - me.transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        me.transform.rotation = Quaternion.Lerp(me.transform.rotation, lookRotation, 0.1f);
    }


    //��ƃ^�[�Q�b�g��gameObject�̒��_pos��Ԃ�
    public Vector3 GetPosBetweenTargetAndFace(GameObject targetGameObject, Vector3 addPos) {
        Vector3 facePos = face.transform.position;
        Vector3 targetPos = targetGameObject.transform.position;

        return new Vector3(
            ((targetPos.x + facePos.x) / 2) + addPos.x,
            facePos.y + addPos.y,
            ((targetPos.z + facePos.z) / 2) + addPos.z
            );
    }

    //�^�[�Q�b�g�������̏ꏊ�ɃC���X�^���X�����
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
    //�^�[�Q�b�g��gameObject�̑O����addPos�������ꂽ�Ƃ����pos���擾����
    public Vector3 GetPosFromTarget(GameObject targetGameObject, Vector3 addPos) {
        Vector3 pos = targetGameObject.transform.position;
        Vector3 forward = targetGameObject.transform.forward;
        forward.x *= addPos.x;
        forward.y *= addPos.y;
        forward.z *= addPos.z;
        pos += forward;
        return pos;
    }
    //��̉�]���擾����
    public Quaternion GetQuaternionFace() {
        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        return r;
    }

}
