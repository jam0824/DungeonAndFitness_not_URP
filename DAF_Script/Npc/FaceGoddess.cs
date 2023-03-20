using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGoddess : FaceParent, IFace
{
    int Eyes_down = 0;
    int Surprise = 1;
    int Eyes_closed = 2;
    int Blink_l = 3;
    int Blink_r = 4;
    int Brown_up = 5;
    int Brown_down = 6;
    int Offended = 7;
    int Painfully = 8;
    int Smile = 9;
    int Lips_down = 10;
    int Lips_left = 11;
    int Lips_right = 12;
    int Teeth = 13;
    int Mouth_open = 14;
    int Shock = 15;
    int Fear = 16;
    int Anger = 17;
    int Laughter = 18;
    int Flirting = 19;
    int Cheek_puff = 20;
    int Sight_r = 21;
    int Sight_l = 22;
    int Sight_down = 23;
    int Sight_up = 24;
    int Sorry = 25;
    int Sadness = 26;
    int Shyness_1 = 27;
    int Shyness_2 = 28;
    int Suspicion = 29;
    int Mouth_down = 30;
    int Mouth_pucker = 31;
    int Mouth = 32;
    int Mouth_open1 = 33;

    public SkinnedMeshRenderer charSkinedMeshRenderer;


    public override void FaceInit() {
        FACE_MAX = 34;
        skinedMeshRenderer = charSkinedMeshRenderer;
        animationSet["BLINK"] = new int[] { Blink_l, Blink_r };
        animationEff["BLINK"] = new float[] { 100f, 100f };
        animationSet["SURPRISE"] = new int[] { Surprise };
        animationEff["SURPRISE"] = new float[] { 100f };
        animationSet["OFFENDED"] = new int[] { Offended };
        animationEff["OFFENDED"] = new float[] { 100f };
        animationSet["PAINFULLY"] = new int[] { Painfully };
        animationEff["PAINFULLY"] = new float[] { 100f };
        animationSet["SMILE"] = new int[] { Smile };
        animationEff["SMILE"] = new float[] { 100f };
        animationSet["SHOCK"] = new int[] { Shock };
        animationEff["SHOCK"] = new float[] { 100f };
        animationSet["FEAR"] = new int[] { Fear };
        animationEff["FEAR"] = new float[] { 100f };
        animationSet["ANGER"] = new int[] { Anger };
        animationEff["ANGER"] = new float[] { 100f };
        animationSet["LAUGHTER"] = new int[] { Laughter };
        animationEff["LAUGHTER"] = new float[] { 100f };
        animationSet["Flirting"] = new int[] { Flirting };
        animationEff["Flirting"] = new float[] { 100f };
        animationSet["SORRY"] = new int[] { Sorry };
        animationEff["SORRY"] = new float[] { 100f };
        animationSet["SADNESS"] = new int[] { Sadness };
        animationEff["SADNESS"] = new float[] { 100f };
        animationSet["SHYNESS_1"] = new int[] { Shyness_1 };
        animationEff["SHYNESS_1"] = new float[] { 100f };
        animationSet["SHYNESS_2"] = new int[] { Shyness_2 };
        animationEff["SHYNESS_2"] = new float[] { 100f };
        animationSet["SUSPICION"] = new int[] { Suspicion };
        animationEff["SUSPICION"] = new float[] { 100f };
    }
}
