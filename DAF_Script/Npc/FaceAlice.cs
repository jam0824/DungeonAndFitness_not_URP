using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAlice : MonoBehaviour,IFace
{
    int IDLE = 0;
    int BLINK = 1;
    int MTH_A = 2;
    int MTH_I = 3;
    int MTH_U = 4;
    int MTH_O = 5;
    int EYE_L_SMILE = 6;
    int EYE_R_SMILE = 7;
    int EYE_NO_HIGHLIGHT = 8;
    int BRW_L_SAD = 9;
    int BRW_R_SAD = 10;
    int BRW_L_ANGRY = 11;
    int BRW_R_ANGRY = 12;


    int FACE_MAX = 13;
    float blinkSpeed = 25f;
    float nowBlinkWeight = 0f;
    float BLINK_WAIT_TIME = 3f;

    public SkinnedMeshRenderer skinedMeshRenderer;
    bool isBlink = false;
    Dictionary<string, int[]> animationSet = new Dictionary<string, int[]>();
    Dictionary<string, float[]> animationEff = new Dictionary<string, float[]>();

    
    // Start is called before the first frame update
    public void Start()
    {
        FaceInit();
        StartCoroutine(EnableBlink(BLINK_WAIT_TIME));
    }

    public void FaceInit() {
        animationSet["BLINK"] = new int[] { BLINK };
        animationEff["BLINK"] = new float[] { 100f };
        animationSet["ANGRY"] = new int[] { BRW_L_ANGRY, BRW_R_ANGRY };
        animationEff["ANGRY"] = new float[] {100f,100f};
        animationSet["SMILE"] = new int[] { EYE_L_SMILE, EYE_R_SMILE };
        animationEff["SMILE"] = new float[] { 100f, 100f};
        animationSet["SAD"] = new int[] { BRW_L_SAD, BRW_R_SAD };
        animationEff["SAD"] = new float[] { 100f, 100f };
        animationSet["SURPRISE"] = new int[] { MTH_O };
        animationEff["SURPRISE"] = new float[] { 100f };
    }

    // Update is called once per frame
    public void Update()
    {
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
        for(int i = 0; i < FACE_MAX; i++) {
            skinedMeshRenderer.SetBlendShapeWeight(i,0f);
        }
    }
}
