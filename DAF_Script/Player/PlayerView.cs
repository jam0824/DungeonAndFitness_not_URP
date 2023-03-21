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

    //ItemBox�������Ă���Enable�ɂȂ��Ԃ�����̂ŉ���̂��߁B
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
    /// �����ʒu���Z�b�g����Ă���Ƃ��͂���Anchor�Ɉړ�
    /// </summary>
    public void MoveToAnchor() {
        if (DataSystem.instance.dataScenario.sceneAnchorName == null) return;
        SetPlayerPosition(DataSystem.instance.dataScenario.sceneAnchorName);
        DataSystem.instance.dataScenario.sceneAnchorName = null;
    }
    /// <summary>
    /// �v���C���[�̏����ʒu�̕ύX
    /// </summary>
    /// <param name="anchorName"></param>
    void SetPlayerPosition(string anchorName) {
        GameObject anchor = GameObject.Find(anchorName);
        Vector3 pos = anchor.transform.position;
        DebugWindow.instance.DFDebug("�v���C���[�����ʒuAnchorName : " + anchorName);
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

    //���j���[�\��
    void setEnableItemCanvas() {
        if ((OVRInput.GetDown(OVRInput.RawButton.Y))
            || (Input.GetKeyDown(KeyCode.Q))) {
            EnableItemCanvas(HitArea);

        }
    }

    //ItemBox��\������BItemBag�Ȃǂ���Ă΂��B
    public void SetEnableItemBox() {
        if ((Input.GetKeyDown(KeyCode.A))
            || (OVRInput.GetDown(OVRInput.RawButton.A))) {
            //�A�C�e���{�b�N�X���A�N�e�B�u�������牽���N�����Ȃ��i�����̂�update���ōs���j
            if (SingletonGeneral.instance.itemBox.ActiveSelf()) return;
            //�A�C�e���{�b�N�X�������Ȃ����Ƃ��������̂Ń��b�N������
            if (lockEnableItemBox) return;
            EnableItemBox(HitArea);
        }
    }
    //ItemBox�������BItemBag�������Ă��Ȃ��Ƃ��ɂ��K�v�Ȃ���playerView��update����Ă�
    public void UnEnableItemBox() {
        if ((Input.GetKeyDown(KeyCode.A))
        || (OVRInput.GetDown(OVRInput.RawButton.A))) {
            //A�{�^����update�ł��Ђ���������itembox��t�����Ɠ����ɏ����邽��
            //�J�E���g��ݒu���Ă�����x�ȏ�Ŕ��΂���悤�ɂ���
            if ((SingletonGeneral.instance.itemBox.ActiveSelf())&&
                (SingletonGeneral.instance.itemBox.activeCount > 32)) {
                DestroyItemBox();
            }
        }
    }

    //���\��
    void EnablePunch(float flex, float index, PlayerRocketPanch rocketPunch, GameObject punch) {
        if ((flex > 0.7)&&(index > 0.7)) {
            if(!punch.activeSelf) SetPunch(punch);
            //�l�����w���߂��ꂽ�烍�P�b�g�p���`�̃��b�N����������
            if (rocketPunch.isLock) rocketPunch.isLock = false;
            return;
        }
        //�l�����w������������Ă����烍�P�b�g�p���`
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

    //���\�����C��
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

    
    //���j���[�\���{��
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

    //ItemBox�\���{��
    void EnableItemBox(GameObject face) {
        Vector3 addPos = new Vector3(1.2f,0.6f,1.2f);
        SingletonGeneral.instance.itemBox.EnableItemBox(
            SingletonGeneral.instance.GetPosFromTarget(face, addPos),
            SingletonGeneral.instance.GetQuaternionFace()
            ) ;
    }
    //ItemBox�j��
    void DestroyItemBox() {
        SingletonGeneral.instance.itemBox.UnableItemBox();
        lockEnableItemBox = true;
        StartCoroutine(UnlockEnableItemBox(1.0f));
    }

    //�A�C�e���{�b�N�X��������Ɠ����ɐ������������̂ł����Ȃ�Ȃ��悤�ɏ���
    IEnumerator UnlockEnableItemBox(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        lockEnableItemBox = false;
    }
    
}
