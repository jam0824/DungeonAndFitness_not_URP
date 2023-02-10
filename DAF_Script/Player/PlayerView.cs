using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public static PlayerView instance;

    public PlayerConfig config { set; get; }
    public HUD hud { set; get; }
    public AudioSource audioSource { get; set; }

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

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerViewInit();
        }
        else {
            Destroy(gameObject);
        }
    }

    void playerViewInit() {
        config = GetComponent<PlayerConfig>();
        hud = transform.Find("HUD").GetComponent<HUD>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
        audioSource = CameraC.GetComponent<AudioSource>();

        config.PlayerConfigInit();
        hud.HudInit();

        StartCoroutine(CalcSatiation());
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
    void EnablePunch(float flex, float index, GameObject punch) {
        if ((flex > 0.7)&&(index > 0.7)) {
            SetPunch(punch);
        }
        else {
            punch.SetActive(false);
        }
    }

    //���\�����C��
    void SetPunch(GameObject punch) {
        if (punch.activeSelf == false) {
            punch.SetActive(true);
            //���o���̎���trigger�ɂ��ĂȂ��ƁA
            //�o���_���[�W��^���邱�Ƃ��ł��邽��trigger�ɂ���
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

    //�����x�̌v�Z
    IEnumerator CalcSatiation() {
        while (true) {
            yield return new WaitForSeconds(1f);
            float nowSatiation = config.CalcSatiation(config.GetDecreaseSatiation() * -1f);
            hud.RedrawSatiation();
        }
    }
    
}
