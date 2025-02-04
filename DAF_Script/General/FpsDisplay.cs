using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    int frameCount;
    float prevTime;
    float fps;

    // 初期化処理
    void Start() {
        frameCount = 0;
        prevTime = 0.0f;
    }
    // 更新処理
    void Update() {
        
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f) {
            fps = frameCount / time;
            DebugWindow.instance.DFFps(fps);

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
        
    }
}
