using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGeneral : MonoBehaviour
{
    public static SingletonGeneral instance;
    public GameObject player { set; get; }
    public GameObject face { set; get; }
    public PlayerConfig playerConfig{set;get;}
    public PlayerView playerView { set; get; }
    public LabelInformationText labelInformationText { set; get; }
    public AudioSource audioSource { set; get; }

    public List<AudioClip> listBattleSE;

    Dictionary<string, int> dictSeName;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetDictSeName();
        LoadPlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
