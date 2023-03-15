using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioCommand : MonoBehaviour
{
    //Playerを見るようにする
    public void CommandLookAt(GameObject obj) {
        SingletonGeneral.instance.LookAt(
            SingletonGeneral.instance.face,
            obj);
    }

    /// <summary>
    /// 指定したオブジェクトがPlayerを見るようにする
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
    /// LookCharacterToCharacter,動作対象,見るキャラ
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

    //通常枠のアイテムを追加
    public void CommandNormalItemGet(string itemNo, ItemDB itemDb, string FULL_OF_ITEM_KEY) {
        if (itemDb.canAddItem(itemNo)) {
            DebugWindow.instance.DFDebug("アイテム追加：" + itemNo);
            itemDb.AddItem(itemNo);
        }
        else {
            SingletonGeneral.instance.labelInformationText.SetInformationLabel(FULL_OF_ITEM_KEY);
            Vector3 pos = gameObject.transform.position;
            pos.y += 1f;
            itemDb.MakeItemBag(itemNo, pos, gameObject.transform.rotation);
            DebugWindow.instance.DFDebug("アイテムバッグ作成：" + itemNo);
        }
    }

    //SEを鳴らす
    public void CommandSe(string seName) {
        DebugWindow.instance.DFDebug("SE:" + seName);
        SingletonGeneral.instance.PlayOneShotNoAudioSource(seName);
    }

    /// <summary>
    /// Saveする
    /// </summary>
    public void CommandSave() {
        DebugWindow.instance.DFDebug("SAVE");
        SingletonGeneral.instance.saveLoadSystem.Save();
        SingletonGeneral.instance.labelInformationText.SetInformationLabel("Save");
    }

    /// <summary>
    /// FeelIconを出す
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
    /// 画面下のインフォメーションにメッセージを表示する
    /// </summary>
    /// <param name="labelKey"></param>
    public void CommandInformation(string labelKey) {
        SingletonGeneral.instance.labelInformationText.SetInformationLabel(labelKey);
    }

    

    /// <summary>
    /// AnimationSet,charName,type(bool/triger),key,value(true/false : boolのとき)
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
    /// Playerのコントローラーでの移動を禁止する
    /// </summary>
    public void CommandNoControll() {
        PlayerView.instance.canControll = false;
        DebugWindow.instance.DFDebug("Playerの操作無効化");
    }
    public void CommandControll() {
        PlayerView.instance.canControll = true;
        DebugWindow.instance.DFDebug("Playerの操作可能");
    }

    /// <summary>
    /// 指定したオブジェクト名の表情を変える
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="emotion"></param>
    public void CommandFace(string characterName, string emotion) {
        //表情は大文字で。
        emotion = emotion.ToUpper();
        //英訳にピリオドがついていることがある
        emotion = emotion.Replace(".", "");

        GameObject.Find(characterName)
            .GetComponent<AnimationSystem>()
            .SetFace(emotion);
        DebugWindow.instance.DFDebug("表情変更:" + characterName + "->" + emotion);
    }


    


    /// <summary>
    /// AutoMessageTop,flag
    /// trueだと自動的にキャラクターの頭の上にメッセージを出す
    /// </summary>
    /// <param name="flag"></param>
    public bool CommandAutoMessageTop(string flag) {
        flag = flag.ToLower();
        bool f = (flag == "true") ? true : false;
        DebugWindow.instance.DFDebug("AutoMessageTop : " + f);
        return f;
    }
}
