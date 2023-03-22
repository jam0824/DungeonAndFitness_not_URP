using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyMoveParentTests
{
    private GameObject enemyGameObject;
    private EnemyMoveParent enemyMoveParent;
    private EnemyView enemyView;
    private EnemyConfig enemyConfig;
    private EnemyAnimation enemyAnimation;

    [SetUp]
    public void Setup() {
        enemyGameObject = new GameObject();
        enemyMoveParent = enemyGameObject.AddComponent<EnemyMoveParent>();
        enemyView = new EnemyView(); // ����́A���ۂ̃v���W�F�N�g��EnemyView�N���X����������Ă��邱�Ƃ�z�肵�Ă��܂��B
        enemyConfig = new EnemyConfig(); // ����́A���ۂ̃v���W�F�N�g��EnemyConfig�N���X����������Ă��邱�Ƃ�z�肵�Ă��܂��B
        enemyAnimation = new EnemyAnimation(); // ����́A���ۂ̃v���W�F�N�g��EnemyAnimation�N���X����������Ă��邱�Ƃ�z�肵�Ă��܂��B
        enemyMoveParent.EnemyMoveInit(enemyView);
        enemyConfig.NOTICE_DISTANCE = 50f;
        enemyConfig.BATTLE_DISTANCE = 30f;
        enemyConfig.BATTLE_END_DISTANCE = 120f;
    }

    [TearDown]
    public void Teardown() {
        Object.Destroy(enemyGameObject);
    }

    [Test]
    public void DetermineStatusByDistance_Walk_Test() {
        float dist = 100f;
        enemyMoveParent.config = enemyConfig;
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.WALK, result);
    }

    [Test]
    public void DetermineStatusByDistance_Notice_Test() {
        float dist = 40f;
        enemyMoveParent.config = enemyConfig;
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.NOTICE, result);
    }

    [Test]
    public void DetermineStatusByDistance_Battle_Test() {
        float dist = 20f;
        enemyMoveParent.config = enemyConfig;
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.BATTLE, result);
    }
    [Test]
    public void DetermineStatusByDistance_CurrentStateNotice_Test() {
        float dist = 100f;
        enemyMoveParent.config = enemyConfig;
        enemyMoveParent.config.SetEnemyState("Notice");
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.NOTICE, result);
    }

    [Test]
    public void DetermineStatusByDistance_CurrentStateBattle_Test() {
        float dist = 100f;
        enemyMoveParent.config = enemyConfig;
        enemyMoveParent.config.SetEnemyState("Battle");
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.BATTLE, result);
    }

    [Test]
    public void DetermineStatusByDistance_BattleEndDistance_Test() {
        float dist = 125f;
        enemyMoveParent.config = enemyConfig;
        enemyMoveParent.config.SetEnemyState("Battle");
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.WALK, result);
    }

    [Test]
    public void DetermineStatusByDistance_OnNoticeDistanceBoundary_Test() {
        float dist = 50f;
        enemyMoveParent.config = enemyConfig;
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.NOTICE, result);
    }

    [Test]
    public void DetermineStatusByDistance_OnBattleDistanceBoundary_Test() {
        float dist = 30f;
        enemyMoveParent.config = enemyConfig;
        var result = enemyMoveParent.DetermineStatusByDistance(dist, enemyConfig);
        Assert.AreEqual(EnemyMoveParent.STATE_TYPE.BATTLE, result);
    }
}
