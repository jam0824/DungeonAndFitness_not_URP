using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSystem : MonoBehaviour
{

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
}
