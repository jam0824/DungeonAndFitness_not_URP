using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FaceParentTest
{
    private GameObject testGameObject;
    private FaceParent faceParent;

    [SetUp]
    public void Setup() {
        testGameObject = new GameObject();
        faceParent = testGameObject.AddComponent<FaceParent>();
        faceParent.SetAnimationSet(
            new Dictionary<string, int[]>{
                { "EMOTION", new[] { 0 } }
            }
        );
        faceParent.SetAnimationEff(
            new Dictionary<string, float[]>{
                { "EMOTION", new[] { 100f } 
            }
        });
        SkinnedMeshRenderer skinnedMeshRenderer = testGameObject.AddComponent<SkinnedMeshRenderer>();
        Mesh mesh = new Mesh();
        mesh.AddBlendShapeFrame(
            "BlendShape0", 
            0f, 
            new Vector3[0], 
            new Vector3[0], 
            new Vector3[0]);
        skinnedMeshRenderer.sharedMesh = mesh;
        faceParent.skinedMeshRenderer = skinnedMeshRenderer;
        faceParent.SetFaceMax(1);
    }

    [TearDown]
    public void Teardown() {
        GameObject.Destroy(testGameObject);
    }

    [Test]
    public void FaceParent_FaceInit_Test() {
        faceParent.FaceInit();
        Assert.Pass(); // If no exceptions are thrown, it passes
    }

    [Test]
    public void FaceParent_SetFace_Test() {
        faceParent.SetFace("EMOTION");
        float blendShapeWeight = faceParent.skinedMeshRenderer.GetBlendShapeWeight(0);
        Assert.AreEqual(100f, blendShapeWeight);
    }

    [Test]
    public void FaceParent_ResetFace_Test() {
        faceParent.SetFace("EMOTION");
        faceParent.ResetFace();
        float blendShapeWeight = faceParent.skinedMeshRenderer.GetBlendShapeWeight(0);
        Assert.AreEqual(0f, blendShapeWeight);
    }
}
