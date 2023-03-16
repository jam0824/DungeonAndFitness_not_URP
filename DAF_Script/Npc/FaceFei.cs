using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceFei : FaceParent, IFace
{
    int BLINK = 0;
    int BLINK_L = 1;
    int BLINK_R = 2;
    int BLINK_DUPE = 3;
    int SMILE_EYE = 4;
    int WINK_L = 5;
    int WINK_R = 6;
    int RAKU_EYE = 7;
    int JITO_EYE = 8;
    int LARGE_EYE = 9;
    int ANGRY_EYE = 10;
    int SHOUT_EYE = 11;
    int SHIRINK_EYE = 12;
    int UP_BROWS = 13;
    int DOWN_BROWS = 14;
    int TROUBLE_BROWS = 15;
    int FLAT_BROWS = 16;
    int ANGRY_BROWS_01 = 17;
    int ANGRY_BROWS_02 = 18;
    int SMILE_MOUTH = 19;
    int NEGA_MOUTH = 20;
    int SCARE_MOUTH = 21;
    int ANGRY_MOUTH = 22;
    int OMEGA_MOUTH = 23;
    int CHEEK = 24;
    int LIP_AA_01 = 25;
    int LIP_AA_02 = 26;
    int LIP_OO = 27;
    int LIP_UU_01 = 28;
    int LIP_UU_02 = 29;
    int LIP_II_01 = 30;
    int LIP_II_02 = 31;
    int LIP_EE_01 = 32;
    int LIP_EE_02 = 33;
    int LIP_AA_01_DUPE = 34;
    int LIP_AA_02_DUPE = 35;
    int LIP_OO_DUPE = 36;
    int LIP_UU_01_DUPE = 37;
    int LIP_UU_02_DUPE = 38;
    int LIP_II_01_DUPE = 39;
    int LIP_II_02_DUPE = 40;
    int LIP_EE_01_DUPE = 41;
    int LIP_EE_02_DUPE = 42;

    public SkinnedMeshRenderer childSkinedMeshRenderer;

    public override void FaceInit() {
        FACE_MAX = 43;
        skinedMeshRenderer = childSkinedMeshRenderer;

        animationSet["BLINK"] = new int[] { BLINK };
        animationEff["BLINK"] = new float[] { 100f };
        animationSet["ANGRY"] = new int[] { ANGRY_BROWS_02, ANGRY_EYE, ANGRY_MOUTH,DOWN_BROWS };
        animationEff["ANGRY"] = new float[] {100f,100f,100f,50f };
        animationSet["DOYA"] = new int[] { ANGRY_BROWS_01, ANGRY_EYE, DOWN_BROWS, JITO_EYE, LIP_II_02_DUPE, SMILE_MOUTH };
        animationEff["DOYA"] = new float[] { 100f, 100f, 100f, 100f, 100f, 100f };
        animationSet["FUN"] = new int[] { LARGE_EYE, SMILE_MOUTH, UP_BROWS };
        animationEff["FUN"] = new float[] { 100f,100f,100f};
        animationSet["FUNYA"] = new int[] { CHEEK, OMEGA_MOUTH, TROUBLE_BROWS, UP_BROWS,RAKU_EYE};
        animationEff["FUNYA"] = new float[] { 100f,100f,100f,100f,100f};
        animationSet["JITO"] = new int[] { CHEEK,DOWN_BROWS,FLAT_BROWS,JITO_EYE,NEGA_MOUTH,SHIRINK_EYE,TROUBLE_BROWS};
        animationEff["JITO"] = new float[] { 100f,100f,100f,100f,100f,60f,10f};
        animationSet["JITO_N"] = new int[] { DOWN_BROWS, FLAT_BROWS, JITO_EYE, NEGA_MOUTH, SHIRINK_EYE, TROUBLE_BROWS };
        animationEff["JITO_N"] = new float[] { 100f, 100f, 100f, 100f, 60f, 10f };
        animationSet["JOY"] = new int[] { LARGE_EYE, LIP_II_02_DUPE,SMILE_MOUTH,UP_BROWS};
        animationEff["JOY"] = new float[] { 100f,100f,100f,100f};
        animationSet["KYOTON"] = new int[] { LARGE_EYE, LIP_UU_02_DUPE,UP_BROWS};
        animationEff["KYOTON"] = new float[] { 100f,100f,100f};
        animationSet["OKO"] = new int[] { ANGRY_BROWS_01, ANGRY_EYE,DOWN_BROWS,NEGA_MOUTH};
        animationEff["OKO"] = new float[] { 100f,100f,100f,100f};
        animationSet["SAD"] = new int[] { JITO_EYE,NEGA_MOUTH,TROUBLE_BROWS,UP_BROWS,RAKU_EYE};
        animationEff["SAD"] = new float[] { 55f,100f,100f,100f,10f};
        animationSet["SCARE"] = new int[] { LARGE_EYE,SCARE_MOUTH,SHIRINK_EYE,TROUBLE_BROWS};
        animationEff["SCARE"] = new float[] { 100f,66.4f,31.3f,40f};
        animationSet["SHOUT"] = new int[] { ANGRY_BROWS_01,ANGRY_MOUTH,DOWN_BROWS,SHOUT_EYE};
        animationEff["SHOUT"] = new float[] { 100f,100f,30f,100f};
        animationSet["SHY"] = new int[] { CHEEK,FLAT_BROWS,LARGE_EYE,SMILE_MOUTH,UP_BROWS,RAKU_EYE};
        animationEff["SHY"] = new float[] { 100f,100f,100f,60f,100f,20f};
        animationSet["SMILE"] = new int[] { SMILE_EYE,SMILE_MOUTH,UP_BROWS};
        animationEff["SMILE"] = new float[] { 100f,100f,100f};
    }

}
