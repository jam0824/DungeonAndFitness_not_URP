using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{
    public AnimationClip walkAnimation;
    float speed = 1f;
    private float gravity = 9.8f;

    GameObject toAnchor;
    CharacterController controller;
    float stopDistance = 1f;
    ScenarioExec scenarioExec;
    int lineNo;

    public bool isLook { set; get; }

    private void Update() {
        if (toAnchor != null) Move(toAnchor);
        if (isLook) LookAt(gameObject);
    }

    /// <summary>
    /// Playerの顔の方を見る
    /// </summary>
    /// <param name="me"></param>
    void LookAt(GameObject me) {
        SingletonGeneral.instance.LookAt(
            SingletonGeneral.instance.face,
            me);
    }

    /// <summary>
    /// キャラクターの移動
    /// anchorとの距離が既定以内なら止まる
    /// </summary>
    /// <param name="anchor"></param>
    private void Move(GameObject anchor) {
        SingletonGeneral.instance.LookAt(anchor, gameObject);
        Vector3 moveDir = gameObject.transform.forward;
        
        moveDir.y = 0f;
        Vector3 pos = gameObject.transform.position;
        pos += moveDir * Time.deltaTime * speed;
        gameObject.transform.position = pos;
        
        if (IsDistanceAnchor(anchor, gameObject)) StopMove();
    }
    /// <summary>
    /// 2つのオブジェクト間の距離を測定して、既定距離以内でtrueを返す
    /// </summary>
    /// <param name="anchor"></param>
    /// <param name="me"></param>
    /// <returns></returns>
    bool IsDistanceAnchor(GameObject anchor, GameObject me) {
        float dist = FQCommon.Common.GetDistance(
            me.transform.position,
            anchor.transform.position);
        return (dist <= stopDistance) ? true : false;
    }

    /// <summary>
    /// 表情を変える
    /// キャラによって呼び出すファイルが違う
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="emotion"></param>
    public void SetFace(string characterName, string emotion) {
        switch (characterName) {
            case "Fei":
                FaceFei(emotion);
                break;
        }

    }

    void FaceFei(string emotion) {
        if (emotion == "NORMAL") {
            GetComponent<FaceFei>().ResetFace();
        }
        else {
            GetComponent<FaceFei>().SetFace(emotion);
        }
    }

    /// <summary>
    /// 歩きアニメーションのオンオフ
    /// </summary>
    /// <param name="isWalk"></param>
    public void SetWalk(bool isWalk) {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Walk", isWalk);
    }

    /// <summary>
    /// キャラクターをanchorまで移動させる。シナリオから呼ばれる
    /// </summary>
    /// <param name="exec"></param>
    /// <param name="no"></param>
    /// <param name="anchor"></param>
    public void MoveToObject(ScenarioExec exec, int no, GameObject anchor) {
        scenarioExec = exec;
        lineNo = no;
        toAnchor = anchor;
        controller = GetComponent<CharacterController>();
        SetWalk(true);
    }

    /// <summary>
    /// Moveを止めるときに呼ばれる
    /// </summary>
    void StopMove() {
        toAnchor = null;
        SetWalk(false);
        scenarioExec.StopCharacterMove(lineNo);
    }

}
