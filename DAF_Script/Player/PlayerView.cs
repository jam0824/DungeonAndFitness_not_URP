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

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        config = GetComponent<PlayerConfig>();
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        dungeonSystem = GameObject.Find("DungeonSystem").GetComponent<DungeonSystem>();
        audioSource = CameraC.GetComponent<AudioSource>();
        SetPlayerGameObjects();
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //FQCommon.Common.AppendStringFile("PlayerItemSave.txt", "101");
    }

    //GeneralSystem�ɃI�u�W�F�N�g��o�^����
    private void SetPlayerGameObjects() {
        generalSystem.SetPlayerPrefab(gameObject);
        generalSystem.SetFacePrefab(HitArea);
        generalSystem.SetLeftArmorPrefab(leftPlayerPunch);
        generalSystem.SetRightArmorPrefab(rightPlayerPunch);
        generalSystem.audioSource = audioSource;
    }

    // Update is called once per frame
    void Update()
    {
        //SetEnablePunch();
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
            if (generalSystem.itemBox.ActiveSelf()) return;
            EnableItemBox(HitArea);
        }
    }
    //ItemBox�������BItemBag�������Ă��Ȃ��Ƃ��ɂ��K�v�Ȃ���playerView��update����Ă�
    public void UnEnableItemBox() {
        if ((Input.GetKeyDown(KeyCode.A))
        || (OVRInput.GetDown(OVRInput.RawButton.A))) {
            //A�{�^����update�ł��Ђ���������itembox��t�����Ɠ����ɏ����邽��
            //�J�E���g��ݒu���Ă�����x�ȏ�Ŕ��΂���悤�ɂ���
            if ((generalSystem.itemBox.ActiveSelf())&&(generalSystem.itemBox.activeCount > 72)) {
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
            generalSystem.PlayOneShot(
                punch.GetComponent<AudioSource>(),
                "PunchEnableSE");
        }
    }

    
    //���j���[�\���{��
    void EnableItemCanvas(GameObject face) {
        if (generalSystem.itemWindow.ActiveSelf()) {
            generalSystem.itemWindow.OnClickClose();
            return;
        }
        Vector3 pos = face.transform.position;
        Vector3 addPos = face.transform.forward;
        addPos.y = 0f;
        pos += addPos;

        Quaternion r = generalSystem.GetQuaternionFace();

        generalSystem.itemWindow.EnableItemWindow(pos, r);
    }

    //ItemBox�\���{��
    void EnableItemBox(GameObject face) {
        Vector3 addPos = new Vector3(1.2f,0.6f,1.2f);
        generalSystem.itemBox.EnableItemBox(
            generalSystem.GetPosFromTarget(face, addPos),
            generalSystem.GetQuaternionFace()
            ) ;
    }
    //ItemBox�j��
    void DestroyItemBox() {
        generalSystem.itemBox.UnableItemBox();
    }

    
}
