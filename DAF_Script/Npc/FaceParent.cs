using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceParent : MonoBehaviour
{
    public SkinnedMeshRenderer skinedMeshRenderer;
    protected int FACE_MAX;
    protected Dictionary<string, int[]> animationSet = new Dictionary<string, int[]>();
    protected Dictionary<string, float[]> animationEff = new Dictionary<string, float[]>();


    float blinkSpeed = 25f;
    float nowBlinkWeight = 0f;
    float BLINK_WAIT_TIME = 3f;
    bool isBlink = false;

    public virtual void FaceInit() {

    }

    // Start is called before the first frame update
    public void Start() {
        FaceInit();
        StartCoroutine(EnableBlink(BLINK_WAIT_TIME));
    }

    

    // Update is called once per frame
    public void Update() {
        if (isBlink) blink();
    }

    /// <summary>
    /// まばたきの実行
    /// </summary>
    public void blink() {
        nowBlinkWeight += blinkSpeed;
        int[] animationBlink = animationSet["BLINK"];
        for (int i = 0; i < animationBlink.Length; i++) {
            skinedMeshRenderer.SetBlendShapeWeight(animationBlink[i], nowBlinkWeight);
        }

        if (nowBlinkWeight >= 100f) blinkSpeed *= -1;
        if (nowBlinkWeight <= 0f) {
            blinkSpeed *= -1;
            isBlink = false;
        }
    }
    /// <summary>
    /// 数秒おきにまばたきフラグを立てる
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    public IEnumerator EnableBlink(float waitTime) {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            nowBlinkWeight = 0f;
            isBlink = true;
        }
    }

    /// <summary>
    /// これを呼べば表情をセットする
    /// </summary>
    /// <param name="emotion"></param>
    public void SetFace(string emotion) {
        ResetFace();
        for (int i = 0; i < animationSet[emotion].Length; i++) {
            skinedMeshRenderer.SetBlendShapeWeight(
                animationSet[emotion][i],
                animationEff[emotion][i]);
        }
    }

    /// <summary>
    /// 表情をリセットする
    /// </summary>
    public void ResetFace() {
        for (int i = 0; i < FACE_MAX; i++) {
            skinedMeshRenderer.SetBlendShapeWeight(i, 0f);
        }
    }

    public void SetAnimationSet(Dictionary<string, int[]> dictAnimationSet) {
        animationSet = dictAnimationSet;
    }

    public void SetAnimationEff(Dictionary<string, float[]> dictAnimationEff) {
        animationEff = dictAnimationEff;
    }

    public void SetFaceMax(int faceMax) {
        FACE_MAX = faceMax;
    }
}
