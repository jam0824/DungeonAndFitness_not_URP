using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public static PlayerView instance;

    public PlayerConfig config { set; get; }
    public HUD hud { set; get; }
    public PlayerStatusChange playerStatusChange { set; get; }
    public PlayerDamage playerDamage { set; get; }
    public AudioSource audioSource { get; set; }
    public GameObject face { get; set; }

    public bool canControll = true;

    [SerializeField]
    public GameObject CameraC;
    public GameObject rightPlayerPunch;
    public GameObject leftPlayerPunch;
    public GameObject HitArea;
    public GameObject ItemBoxObject;

    public DungeonSystem dungeonSystem { set; get; }
    GameObject itemBox;

    //ItemBoxを消してすぐEnableになる状態があるので回避のため。
    bool lockEnableItemBox = false;

    PlayerRocketPanch leftPlayerRocketPunch;
    PlayerRocketPanch rightPlayerRocketPunch;

    private void Awake() {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            playerViewInit();
        }
        else {
            //Destroy(gameObject);
        }
    }

    void playerViewInit() {
        face = HitArea;
        config = GetComponent<PlayerConfig>();
        playerStatusChange = GetComponent<PlayerStatusChange>();
        playerDamage = face.GetComponent<PlayerDamage>();

        hud = transform.Find("HUD").GetComponent<HUD>();
        GameObject dungeonSystemObject = GameObject.Find("DungeonSystem");
        if(dungeonSystemObject != null) 
            dungeonSystem = dungeonSystemObject.GetComponent<DungeonSystem>();
        audioSource = CameraC.GetComponent<AudioSource>();

        config.PlayerConfigInit();
        hud.HudInit(dungeonSystem);
        playerStatusChange.PlayerStatusChangeInit();

        leftPlayerRocketPunch = leftPlayerPunch.GetComponent<PlayerRocketPanch>();
        rightPlayerRocketPunch = rightPlayerPunch.GetComponent<PlayerRocketPanch>();
        
    }

    /// <summary>
    /// 初期位置がセットされているときはそのAnchorに移動
    /// </summary>
    public void MoveToAnchor() {
        if (DataSystem.instance.dataScenario.sceneAnchorName == null) return;
        SetPlayerPosition(DataSystem.instance.dataScenario.sceneAnchorName);
        DataSystem.instance.dataScenario.sceneAnchorName = null;
    }
    /// <summary>
    /// プレイヤーの初期位置の変更
    /// </summary>
    /// <param name="anchorName"></param>
    void SetPlayerPosition(string anchorName) {
        GameObject anchor = GameObject.Find(anchorName);
        Vector3 pos = anchor.transform.position;
        DebugWindow.instance.DFDebug("プレイヤー初期位置AnchorName : " + anchorName);
        transform.position = pos;
        transform.rotation = anchor.transform.rotation;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        MoveToAnchor();
        //FQCommon.Common.AppendStringFile("PlayerItemSave.txt", "101");
    }

    // Update is called once per frame
    void Update()
    {
        SetEnablePunch();
        setEnableItemCanvas();
        UnEnableItemBox();
    }

    void DebugRocketPunch() {
        if (Input.GetKeyDown(KeyCode.W)|| 
            (OVRInput.GetDown(OVRInput.RawButton.B))) {
            DebugWindow.instance.DFDebug("Fire");
            PlayerRocketPanch pr = GameObject.Find("RightArmor").GetComponent<PlayerRocketPanch>();
            pr.Fire();
        }
    }

    public void SetEnablePunch() {
        EnablePunch(
            OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger),
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger),
            leftPlayerRocketPunch,
            leftPlayerPunch
        );
        EnablePunch(
            OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger),
            OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger),
            rightPlayerRocketPunch,
            rightPlayerPunch
        );
    }

    //メニュー表示
    void setEnableItemCanvas() {
        if ((OVRInput.GetDown(OVRInput.RawButton.Y))
            || (Input.GetKeyDown(KeyCode.Q))) {
            EnableItemCanvas(HitArea);

        }
    }

    //ItemBoxを表示する。ItemBagなどから呼ばれる。
    public void SetEnableItemBox() {
        if ((Input.GetKeyDown(KeyCode.A))
            || (OVRInput.GetDown(OVRInput.RawButton.A))) {
            //アイテムボックスがアクティブだったら何も起こさない（消すのはupdate側で行う）
            if (SingletonGeneral.instance.itemBox.ActiveSelf()) return;
            //アイテムボックスが消えないことがあったのでロックをつける
            if (lockEnableItemBox) return;
            EnableItemBox(HitArea);
        }
    }
    //ItemBoxを消す。ItemBagを持っていないときにも必要なためplayerViewのupdateから呼ぶ
    public void UnEnableItemBox() {
        if ((Input.GetKeyDown(KeyCode.A))
        || (OVRInput.GetDown(OVRInput.RawButton.A))) {
            //Aボタンがupdateでもひっかかってitemboxを付けたと同時に消えるため
            //カウントを設置してある程度以上で発火するようにした
            if ((SingletonGeneral.instance.itemBox.ActiveSelf())&&
                (SingletonGeneral.instance.itemBox.activeCount > 32)) {
                DestroyItemBox();
            }
        }
    }

    //拳表示
    void EnablePunch(float flex, float index, PlayerRocketPanch rocketPunch, GameObject punch) {
        if ((flex > 0.7)&&(index > 0.7)) {
            if(!punch.activeSelf) SetPunch(punch);
            //人差し指が戻されたらロケットパンチのロックを解除する
            if (rocketPunch.isLock) rocketPunch.isLock = false;
            return;
        }
        //人差し指だけが離されていたらロケットパンチ
        else if((flex > 0.7f) && (punch.activeSelf)) {
            rocketPunch.Fire();
            rocketPunch.isLock = true;
            return;
        }
        else {
            if (punch.activeSelf) {
                if (!rocketPunch.isFire) UnenablePunch(punch, rocketPunch);
            }
        }
    }

    //拳表示メイン
    void SetPunch(GameObject punch) {
        if (punch.activeSelf == false) {
            punch.SetActive(true);
            PlayerRocketPanch pr = punch.GetComponent<PlayerRocketPanch>();
            pr.ResetRocketPunch();
            HandsScript handsScript = punch.GetComponent<HandsScript>();
            handsScript.ShowHandsArmor();
            GameObject shot = Instantiate(
                config.GetPunchEnablePrefab(),
                punch.transform.position,
                Quaternion.identity);
            SingletonGeneral.instance.PlayOneShot(
                punch.GetComponent<AudioSource>(),
                "PunchEnableSE");
        }
    }

    void UnenablePunch(GameObject punch, PlayerRocketPanch rocketPunch) {
        rocketPunch.isLock = false;
        punch.SetActive(false);
    }

    
    //メニュー表示本体
    void EnableItemCanvas(GameObject face) {
        if (SingletonGeneral.instance.itemWindow.ActiveSelf()) {
            SingletonGeneral.instance.itemWindow.OnClickClose();
            return;
        }
        Vector3 pos = face.transform.position;
        Vector3 addPos = face.transform.forward;
        addPos.y = 0f;
        pos += addPos;

        Quaternion r = SingletonGeneral.instance.GetQuaternionFace();

        SingletonGeneral.instance.itemWindow.EnableItemWindow(pos, r);
    }

    //ItemBox表示本体
    void EnableItemBox(GameObject face) {
        Vector3 addPos = new Vector3(1.2f,0.6f,1.2f);
        SingletonGeneral.instance.itemBox.EnableItemBox(
            SingletonGeneral.instance.GetPosFromTarget(face, addPos),
            SingletonGeneral.instance.GetQuaternionFace()
            ) ;
    }
    //ItemBox破棄
    void DestroyItemBox() {
        SingletonGeneral.instance.itemBox.UnableItemBox();
        lockEnableItemBox = true;
        StartCoroutine(UnlockEnableItemBox(1.0f));
    }

    //アイテムボックスが消えると同時に生成があったのでそうならないように処理
    IEnumerator UnlockEnableItemBox(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        lockEnableItemBox = false;
    }
    
}
