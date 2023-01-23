using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    
    GameObject itemCanvas;
    PlayerConfig config;
    GeneralSystem generalSystem;
    bool isItemCanvas = false;

    [SerializeField]
    public GameObject rightPlayerPunch;
    public GameObject leftPlayerPunch;
    public GameObject HitArea;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        config = GetComponent<PlayerConfig>();
        generalSystem = GameObject.Find("GeneralSystem").GetComponent<GeneralSystem>();
        SetPlayerGameObjects();
    }

    //GeneralSystemにオブジェクトを登録する
    private void SetPlayerGameObjects() {
        generalSystem.SetPlayerPrefab(gameObject);
        generalSystem.SetFacePrefab(HitArea);
        generalSystem.SetLeftArmorPrefab(leftPlayerPunch);
        generalSystem.SetRightArmorPrefab(rightPlayerPunch);
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
        if (isItemCanvas) {
            isItemCanvas = false;
            itemCanvas.GetComponent<ItemView>().OnClickClose();
            return;
        }
        Vector3 pos = face.transform.position;
        Vector3 addPos = face.transform.forward;
        addPos.y = 1.5f;
        pos += addPos;

        Quaternion r = face.transform.rotation;
        r.x = 0.0f;
        r.z = 0.0f;
        itemCanvas = Instantiate(
            generalSystem.GetItemCanvas(),
            pos,
            r
        );
        isItemCanvas = true;
    }


    
}
