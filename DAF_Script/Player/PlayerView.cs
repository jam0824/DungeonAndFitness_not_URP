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

    public DungeonSystem dungeonSystem { set; get; }

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
       
    }

    //GeneralSystemにオブジェクトを登録する
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
        SetEnablePunch();
        setEnableItemCanvas();
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
            || (Input.GetKeyDown(KeyCode.A))) {
            EnableItemCanvas(HitArea);
        }
    }

    //拳表示
    void EnablePunch(float flex, float index, GameObject punch) {
        if ((flex > 0.7)&&(index > 0.7)) {
            if(punch.activeSelf == false) {
                punch.SetActive(true);
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
        else {
            punch.SetActive(false);
        }
    }

    
    //メニュー表示本体
    void EnableItemCanvas(GameObject face) {
        if (generalSystem.ItemWindow.activeSelf) {
            generalSystem.ItemWindow.GetComponent<ItemView>().OnClickClose();
            return;
        }
        Vector3 pos = face.transform.position;
        Vector3 addPos = face.transform.forward;
        addPos.y = 0f;
        pos += addPos;

        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        generalSystem.ItemWindow.SetActive(true);
        generalSystem.ItemWindow.transform.position = pos;
        generalSystem.ItemWindow.transform.rotation = r;
    }


    
}
