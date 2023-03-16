using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAlice : FaceParent,IFace
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


    

    public SkinnedMeshRenderer childSkinedMeshRenderer;
    
    

    public override void FaceInit() {
        FACE_MAX = 13;
        skinedMeshRenderer = childSkinedMeshRenderer;
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
}
