using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioCommand : MonoBehaviour
{
    //Player������悤�ɂ���
    public void CommandLookAt(GameObject obj) {
        SingletonGeneral.instance.LookAt(
            SingletonGeneral.instance.face,
            obj);
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g��Player������悤�ɂ���
    /// </summary>
    /// <param name="objName"></param>
    public void ComandLookFromCharacter(string objName) {
        GameObject obj = GameObject.Find(objName);
        AnimationSystem animationSystem = obj.GetComponent<AnimationSystem>();
        animationSystem.lookTarget = GameObject.Find("HitArea");
        animationSystem.isLook = true;
        DebugWindow.instance.DFDebug("LookFromCharacter : " + objName);
    }

    public void CommandLookOffFromCharacter(string objName) {
        GameObject obj = GameObject.Find(objName);
        AnimationSystem animationSystem = obj.GetComponent<AnimationSystem>();
        animationSystem.isLook = false;
        DebugWindow.instance.DFDebug("LookOffFromCharacter : " + objName);
    }

    /// <summary>
    /// LookCharacterToCharacter,����Ώ�,����L����
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="targetName"></param>
    public void CommandLookCharaceterToCharacter(string characterName, string targetName) {
        GameObject obj = GameObject.Find(characterName);
        AnimationSystem animationSystem = obj.GetComponent<AnimationSystem>();
        animationSystem.lookTarget = GameObject.Find(targetName);
        animationSystem.isLook = true;
        DebugWindow.instance.DFDebug("LookCharacterToCharacter : " + characterName + "->" + targetName);
    }

    //�ʏ�g�̃A�C�e����ǉ�
    public void CommandNormalItemGet(string itemNo, ItemDB itemDb, string FULL_OF_ITEM_KEY) {
        if (itemDb.canAddItem(itemNo)) {
            DebugWindow.instance.DFDebug("�A�C�e���ǉ��F" + itemNo);
            itemDb.AddItem(itemNo);
        }
        else {
            SingletonGeneral.instance.labelInformationText.SetInformationLabel(FULL_OF_ITEM_KEY);
            Vector3 pos = gameObject.transform.position;
            pos.y += 1f;
            itemDb.MakeItemBag(itemNo, pos, gameObject.transform.rotation);
            DebugWindow.instance.DFDebug("�A�C�e���o�b�O�쐬�F" + itemNo);
        }
    }

    //SE��炷
    public void CommandSe(string seName) {
        DebugWindow.instance.DFDebug("SE:" + seName);
        SingletonGeneral.instance.PlayOneShotNoAudioSource(seName);
    }

    /// <summary>
    /// Save����
    /// </summary>
    public void CommandSave() {
        DebugWindow.instance.DFDebug("SAVE");
        SingletonGeneral.instance.saveLoadSystem.Save();
        SingletonGeneral.instance.labelInformationText.SetInformationLabel("Save");
    }

    /// <summary>
    /// FeelIcon���o��
    /// Smile,Angry,Sad,Surprise,Tere
    /// </summary>
    /// <param name="iconKey"></param>
    public LabelFeelIcon CommandFeel(string iconKey, LabelFeelIcon labelFeelIcon) {
        if (labelFeelIcon == null) {
            GameObject feelObject = transform.Find("NpcSet/FeelIcon").gameObject;
            labelFeelIcon = feelObject.GetComponent<LabelFeelIcon>();
        }
        labelFeelIcon.SetIcon(iconKey, transform.rotation);
        return labelFeelIcon;
    }

    /// <summary>
    /// ��ʉ��̃C���t�H���[�V�����Ƀ��b�Z�[�W��\������
    /// </summary>
    /// <param name="labelKey"></param>
    public void CommandInformation(string labelKey) {
        SingletonGeneral.instance.labelInformationText.SetInformationLabel(labelKey);
    }

    

    /// <summary>
    /// AnimationSet,charName,type(bool/triger),key,value(true/false : bool�̂Ƃ�)
    /// </summary>
    /// <param name="line"></param>
    public void CommandAnimationSet(string[] line) {
        string type = line[2].ToLower();
        string key = line[3];
        AnimationSystem animationSystem = GameObject.Find(line[1]).GetComponent<AnimationSystem>();
        if (type == "bool") {
            bool value = (line[4] == "true") ? true : false;
            animationSystem.SetBoolAnimation(key, value);
        }
        else {
            animationSystem.SetTriggerAnimation(key);
        }
    }

    /// <summary>
    /// Player�̃R���g���[���[�ł̈ړ����֎~����
    /// </summary>
    public void CommandNoControll() {
        PlayerView.instance.canControll = false;
        DebugWindow.instance.DFDebug("Player�̑��얳����");
    }
    public void CommandControll() {
        PlayerView.instance.canControll = true;
        DebugWindow.instance.DFDebug("Player�̑���\");
    }

    /// <summary>
    /// �w�肵���I�u�W�F�N�g���̕\���ς���
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="emotion"></param>
    public void CommandFace(string characterName, string emotion) {
        //�\��͑啶���ŁB
        emotion = emotion.ToUpper();
        //�p��Ƀs���I�h�����Ă��邱�Ƃ�����
        emotion = emotion.Replace(".", "");

        GameObject.Find(characterName)
            .GetComponent<AnimationSystem>()
            .SetFace(emotion);
        DebugWindow.instance.DFDebug("�\��ύX:" + characterName + "->" + emotion);
    }


    


    /// <summary>
    /// AutoMessageTop,flag
    /// true���Ǝ����I�ɃL�����N�^�[�̓��̏�Ƀ��b�Z�[�W���o��
    /// </summary>
    /// <param name="flag"></param>
    public bool CommandAutoMessageTop(string flag) {
        flag = flag.ToLower();
        bool f = (flag == "true") ? true : false;
        DebugWindow.instance.DFDebug("AutoMessageTop : " + f);
        return f;
    }
}
