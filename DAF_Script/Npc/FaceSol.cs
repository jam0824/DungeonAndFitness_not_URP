using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSol : FaceParent,IFace
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



    public SkinnedMeshRenderer childSkinedMeshRenderer;
    

   

    public override void FaceInit() {
        FACE_MAX = 29;
        skinedMeshRenderer = childSkinedMeshRenderer;

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
}
