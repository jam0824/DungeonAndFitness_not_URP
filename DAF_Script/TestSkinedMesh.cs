using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkinedMesh : MonoBehaviour
{
    SkinnedMeshRenderer skinedMeshRenderer;
    Mesh skinnedMesh;

    // Start is called before the first frame update
    void Start()
    {
        skinedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        int blendShapeCount = skinnedMesh.blendShapeCount;
        
        Debug.Log("skinedMeshCount:" + blendShapeCount);
        for (int i = 0; i < blendShapeCount; i++) {
            Debug.Log("skinedMesh:" + skinnedMesh.GetBlendShapeName(i) + " / i=" + i);
        }
        skinedMeshRenderer.SetBlendShapeWeight(24, 100f);
        skinedMeshRenderer.SetBlendShapeWeight(8, 100f);
        skinedMeshRenderer.SetBlendShapeWeight(7, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
