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

    private void Update() {
        if (toAnchor != null) Move(toAnchor);
    }

    private void Move(GameObject anchor) {
        SingletonGeneral.instance.LookAt(anchor, gameObject);
        Vector3 moveDir = gameObject.transform.forward;
        
        moveDir.y = 0f;
        Vector3 pos = gameObject.transform.position;
        pos += moveDir * Time.deltaTime * speed;
        gameObject.transform.position = pos;
        
        if (IsDistanceAnchor(anchor, gameObject)) StopMove();
    }

    bool IsDistanceAnchor(GameObject anchor, GameObject me) {
        float dist = FQCommon.Common.GetDistance(
            me.transform.position,
            anchor.transform.position);
        return (dist <= stopDistance) ? true : false;
    }

    


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

    void StopMove() {
        toAnchor = null;
        SetWalk(false);
        scenarioExec.StopCharacterMove(lineNo);
    }

}
