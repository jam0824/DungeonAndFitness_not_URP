using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    
    PlayerConfig config;
    public GeneralSystem generalSystem { get; set; }
    public AudioSource audioSource { get; set; }

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

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        config = GetComponent<PlayerConfig>();
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
        audioSource = CameraC.GetComponent<AudioSource>();
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //FQCommon.Common.AppendStringFile("PlayerItemSave.txt", "101");
    }

    // Update is called once per frame
    void Update()
    {
        SetEnablePunch();
        setEnableItemCanvas();
        UnEnableItemBox();
    }

    public void SetEnablePunch() {
        EnablePunch(
            OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger),
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger),
            leftPlayerPunch
        );
        EnablePunch(
            OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger),
            OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger),
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
            if (generalSystem.itemBox.ActiveSelf()) return;
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
            if ((generalSystem.itemBox.ActiveSelf())&&
                (generalSystem.itemBox.activeCount > 32)) {
                DestroyItemBox();
            }
        }
    }

    //拳表示
    void EnablePunch(float flex, float index, GameObject punch) {
        if ((flex > 0.7)&&(index > 0.7)) {
            SetPunch(punch);
        }
        else {
            punch.SetActive(false);
        }
    }

    //拳表示メイン
    void SetPunch(GameObject punch) {
        if (punch.activeSelf == false) {
            punch.SetActive(true);
            //拳出現の時にtriggerにしてないと、
            //出現ダメージを与えることができるためtriggerにする
            punch.GetComponent<HandsScript>().SetIsTrigger(true);
            GameObject shot = Instantiate(
                config.GetPunchEnablePrefab(),
                punch.transform.position,
                Quaternion.identity);
            SingletonGeneral.instance.PlayOneShot(
                punch.GetComponent<AudioSource>(),
                "PunchEnableSE");
        }
    }

    
    //メニュー表示本体
    void EnableItemCanvas(GameObject face) {
        if (generalSystem.itemWindow.ActiveSelf()) {
            generalSystem.itemWindow.OnClickClose();
            return;
        }
        Vector3 pos = face.transform.position;
        Vector3 addPos = face.transform.forward;
        addPos.y = 0f;
        pos += addPos;

        Quaternion r = SingletonGeneral.instance.GetQuaternionFace();

        generalSystem.itemWindow.EnableItemWindow(pos, r);
    }

    //ItemBox表示本体
    void EnableItemBox(GameObject face) {
        Vector3 addPos = new Vector3(1.2f,0.6f,1.2f);
        generalSystem.itemBox.EnableItemBox(
            SingletonGeneral.instance.GetPosFromTarget(face, addPos),
            SingletonGeneral.instance.GetQuaternionFace()
            ) ;
    }
    //ItemBox破棄
    void DestroyItemBox() {
        generalSystem.itemBox.UnableItemBox();
        lockEnableItemBox = true;
        StartCoroutine(UnlockEnableItemBox(1.0f));
    }

    //アイテムボックスが消えると同時に生成があったのでそうならないように処理
    IEnumerator UnlockEnableItemBox(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        lockEnableItemBox = false;
    }

    
}
