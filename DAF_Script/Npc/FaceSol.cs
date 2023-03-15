using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSol : MonoBehaviour,IFace
{
    int EYES_CLOSED = 0;
    int BLINK_L = 1;
    int BLINK_R = 2;
    int BROW_DOWN = 3;
    int BROW_UP = 4;
    int SMILE = 5;
    int SIGHT_L = 6;
    int SIGHT_R = 7;
    int SIGHT_UP = 8;
    int SIGHT_DOWN = 9;
    int SMILE_1 = 10;
    int MOUTH = 11;
    int SURPRISE = 12;
    int CHEEK = 13;
    int LAUGHTER = 14;
    int FEAR = 15;
    int TEETH = 16;
    int MOUTH_OPEN = 17;
    int LIPS_UP = 18;
    int SOFT_SMILE = 19;
    int BIG_SMILE = 20;
    int SUSPICIOUS = 21;
    int DISCONTENT = 22;
    int SHYNESS = 23;
    int MOUTH_DOWN = 24;
    int SURPRISE_1 = 25;
    int SORRY = 26;
    int TEETH_1 = 27;
    int MOUTH_PUCKER = 28;


    int FACE_MAX = 29;
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
        animationSet["BLINK"] = new int[] { BLINK_L,BLINK_R };
        animationEff["BLINK"] = new float[] { 100f,100f };
        animationSet["SMILE"] = new int[] { SMILE };
        animationEff["SMILE"] = new float[] { 100f };
        animationSet["BIG_SMILE"] = new int[] { BIG_SMILE };
        animationEff["BIG_SMILE"] = new float[] { 100f };
        animationSet["NIKKORI"] = new int[] { SMILE_1 };
        animationEff["NIKKORI"] = new float[] { 100f };
        animationSet["SHINKEN"] = new int[] { BROW_DOWN, SUSPICIOUS };
        animationEff["SHINKEN"] = new float[] { 100f, 100f };
        animationSet["SURPRISE"] = new int[] { SURPRISE };
        animationEff["SURPRISE"] = new float[] { 100f };
        animationSet["SMALL_SURPRISE"] = new int[] { SURPRISE_1 };
        animationEff["SMALL_SURPRISE"] = new float[] { 100f };
        animationSet["LAUGHTER"] = new int[] { LAUGHTER};
        animationEff["LAUGHTER"] = new float[] { 100f };
        animationSet["FEAR"] = new int[] { FEAR };
        animationEff["FEAR"] = new float[] { 100f };
        animationSet["SUSPICIOUS"] = new int[] { SUSPICIOUS };
        animationEff["SUSPICIOUS"] = new float[] { 100f };
        animationSet["DISCONTENT"] = new int[] { DISCONTENT };
        animationEff["DISCONTENT"] = new float[] { 100f };
        animationSet["SHYNESS"] = new int[] { SHYNESS };
        animationEff["SHYNESS"] = new float[] { 100f };

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
