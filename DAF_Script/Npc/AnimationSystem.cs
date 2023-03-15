using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{
    float speed = 1f;
    private float gravity = 9.8f;

    GameObject toAnchor;
    CharacterController controller;
    float stopDistance = 1f;
    ScenarioExec scenarioExec;
    int lineNo;


    public bool isLook { set; get; }
    public GameObject lookTarget { set; get; }

    private void Update() {
        if (toAnchor != null) Move(toAnchor);
        if (isLook) LookAt(lookTarget, gameObject);
    }

    /// <summary>
    /// Player�̊�̕�������
    /// </summary>
    /// <param name="me"></param>
    void LookAt(GameObject target, GameObject me) {
        SingletonGeneral.instance.LookAt(
            target,
            me);
    }

    /// <summary>
    /// �L�����N�^�[�̈ړ�
    /// anchor�Ƃ̋���������ȓ��Ȃ�~�܂�
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
    /// 2�̃I�u�W�F�N�g�Ԃ̋����𑪒肵�āA���苗���ȓ���true��Ԃ�
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
    /// �\���ς���
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="emotion"></param>
    public void SetFace(string emotion) {
        IFace iFace = GetComponent<IFace>();
        SetIFace(emotion, iFace);
    }
    void SetIFace(string emotion, IFace iFace) {
        if (emotion == "NORMAL") {
            iFace.ResetFace();
        }
        else {
            iFace.SetFace(emotion);
        }
    }

    /// <summary>
    /// �����A�j���[�V�����̃I���I�t
    /// </summary>
    /// <param name="isWalk"></param>
    public void SetWalk(bool isWalk) {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Walk", isWalk);
    }

    /// <summary>
    /// Animator��Bool��ݒ肷��
    /// </summary>
    /// <param name="key"></param>
    /// <param name="isStart"></param>
    public void SetBoolAnimation(string key, bool isStart) {
        Animator animator = GetComponent<Animator>();
        animator.SetBool(key, isStart);
    }

    /// <summary>
    /// Animator��Trigger��ݒ肷��
    /// </summary>
    /// <param name="key"></param>
    public void SetTriggerAnimation(string key) {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger(key);
    }

    /// <summary>
    /// �L�����N�^�[��anchor�܂ňړ�������B�V�i���I����Ă΂��
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
    /// Move���~�߂�Ƃ��ɌĂ΂��
    /// </summary>
    void StopMove() {
        toAnchor = null;
        SetWalk(false);
        scenarioExec.StopCharacterMove(lineNo);
    }

}

/// <summary>
/// �\��ύX�̃C���^�[�t�F�C�X
/// </summary>
public interface IFace
{
    void Start();
    void Update();
    void FaceInit();
    void blink();
    IEnumerator EnableBlink(float waitTime);
    public void SetFace(string emotion);
    public void ResetFace();
}