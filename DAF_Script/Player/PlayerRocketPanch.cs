using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocketPanch : MonoBehaviour
{
    int LIST_MAX = 10;
    float IMPACT_GETA = 5.0f;
    float MIN_FIRE_SPEED = 0.1f;
    float MAX_FIRE_SPEED = 2.0f;
    float RESET_FIRE_TIME = 1.5f;
    string handName = "";
    public bool isFire { set; get; }
    public bool isLock { set; get; }
    GameObject anchor;

    List<Vector3> listPosition { set; get; }
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        listPosition = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
        handName = (gameObject.name.Contains("Left")) ? "left" : "right";
        anchor = (handName == "left") ? GameObject.Find("LeftControllerAnchor")
                                      : GameObject.Find("RightControllerAnchor");
        isFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// これを呼び出すとロケットパンチをする
    /// </summary>
    public void Fire() {
        if (isFire) return;
        if (isLock) return;

        float speed = GetSpeed(listPosition);
        //DebugWindow.instance.DFDebug("speed:" + speed);
        //if (speed < MIN_FIRE_SPEED) return;
        if (speed > MAX_FIRE_SPEED) speed = MAX_FIRE_SPEED;

        rb.isKinematic = false;
        rb.freezeRotation = true;
        gameObject.transform.parent = null;
        Vector3 direction = anchor.transform.forward;
        direction /= Time.fixedDeltaTime;
        rb.AddForce(direction * speed * IMPACT_GETA, ForceMode.Impulse);
        isFire = true;
        SingletonGeneral.instance.PlayOneShotNoAudioSource("RocketPunchFire");
        StartCoroutine(ReturnMyHand(RESET_FIRE_TIME));
    }

    /// <summary>
    /// 時間でもリセットをかける。引っかかったりするので。
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator ReturnMyHand(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        if (isFire) {
            ResetRocketPunchWithSe();
        }
    }

    /// <summary>
    /// 10フレームで進んだ距離を返す
    /// </summary>
    /// <param name="listPosition"></param>
    /// <returns></returns>
    float GetSpeed(List<Vector3> listPosition) {
        float dist = FQCommon.Common.GetDistance(
            listPosition[listPosition.Count - 1], 
            listPosition[0]);
        return dist / (Time.fixedDeltaTime * listPosition.Count);
    }

    private void FixedUpdate() {
        InsertPosition();
    }

    /// <summary>
    /// LIST_MAXだけposition情報を貯める。
    /// 0が最新
    /// </summary>
    void InsertPosition() {
        if (listPosition.Count == LIST_MAX) {
            listPosition.RemoveAt(LIST_MAX - 1);
        }
        Vector3 pos = this.transform.position;
        listPosition.Insert(0, pos);
    }

    /// <summary>
    /// 手とぶつかったらロケットパンチをリセット
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if((other.gameObject.tag == "Hand")&&(isFire)) {
            ResetRocketPunchWithSe();
        }
    }

    /// <summary>
    /// 音や振動をつけてリセット
    /// </summary>
    void ResetRocketPunchWithSe() {
        SingletonGeneral.instance.PlayOneShotNoAudioSource("RocketPunchCombine");
        SingletonGeneral.instance.VivrationController(
            gameObject.name, 
            0.5f, 
            0.5f, 
            0.2f);
        ResetRocketPunch();
    }

    /// <summary>
    /// これを呼び出すと、ロケットパンチの設定を一通りリセット
    /// </summary>
    public void ResetRocketPunch() {
        isFire = false;
        gameObject.transform.parent = anchor.transform;
        ResetPosition();
        rb.freezeRotation = false;
        rb.isKinematic = true;
        
    }

    /// <summary>
    /// ナックルの位置をリセットする
    /// </summary>
    void ResetPosition() {
        Vector3 localAnchorPos = anchor.transform.localPosition;
        Quaternion r;
        if (handName == "left") {
            r = SingletonGeneral.instance.GetLocalQuaternionToAddRotation(anchor,180f,90f,0f);
            localAnchorPos.x += -0.1f;
            localAnchorPos.y += -0.2f;
            localAnchorPos.z += 0.2f;
        }
        else {
            r = SingletonGeneral.instance.GetLocalQuaternionToAddRotation(anchor,0f,90f,0f);
            localAnchorPos.x += 0.1f;
            localAnchorPos.y += -0.2f;
            localAnchorPos.z += 0.2f;
        }
        gameObject.transform.localPosition = localAnchorPos;
        gameObject.transform.localRotation = r;
    }

    
}
