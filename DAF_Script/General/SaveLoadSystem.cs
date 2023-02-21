using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// これを呼び出すと全部セーブする
    /// </summary>
    public void Save() {
        SaveStatus();
        SaveItemData();
        SaveSwitch();
    }

    void SaveItemData() {
        //通常のアイテムの保存
        FQCommon.Common.SaveStringToFile(
            SingletonGeneral.instance.GetNormalItemSavePath(),
            CollectionListData(SingletonGeneral.instance.itemDb.playerItemList));
        //コレクションアイテムの保存
        FQCommon.Common.SaveStringToFile(
            SingletonGeneral.instance.GetCollectionItemSavePath(),
            CollectionListData(SingletonGeneral.instance.itemDb.playerCollectionList));
    }

    void SaveStatus() {
        string data = CollectionSaveData();
        FQCommon.Common.SaveStringToFile(
            SingletonGeneral.instance.GetStatusSavePath(),
            data);
    }

    void SaveSwitch() {
        string data = CollectSwitchData();
        FQCommon.Common.SaveStringToFile(
            SingletonGeneral.instance.GetSwitchSavePath(),
            data);
    }

    string CollectionSaveData() {
        string data = "";
        data += CollectPlayerPosition();
        data += CollectDungeonRootPosition();
        data += CollectPlayerConfig();
        return data;
    }
    string CollectPlayerPosition() {
        string data = "";
        GameObject player = GameObject.Find("Player");
        data += "PlayerX\t" + player.transform.position.x.ToString() + "\n";
        data += "PlayerY\t" + player.transform.position.y.ToString() + "\n";
        data += "PlayerZ\t" + player.transform.position.z.ToString() + "\n";
        return data;
    }
    string CollectDungeonRootPosition() {
        string data = "";
        Vector3 pos = SingletonGeneral.instance.dungeonRoot.transform.position;
        data += "DungeonX\t" + pos.x.ToString() + "\n";
        data += "DungeonY\t" + pos.y.ToString() + "\n";
        data += "DungeonZ\t" + pos.z.ToString() + "\n";
        return data;
    }

    string CollectPlayerConfig() {
        string data = "";
        PlayerConfig config = PlayerView.instance.config;
        data += "MaxHp\t" + config.GetMaxHP().ToString() + "\n";
        data += "NowHp\t" + config.GetHP().ToString() + "\n";
        data += "MaxMp\t" + config.GetMaxMP().ToString() + "\n";
        data += "NowMp\t" + config.GetMP().ToString() + "\n";
        data += "Atk\t" + config.GetATK().ToString() + "\n";
        data += "Def\t" + config.GetDEF().ToString() + "\n";
        data += "Satiation\t" + config.GetSatiation().ToString() + "\n";
        data += "MaxPageNo\t" + config.GetMaxPageNo().ToString() + "\n";
        return data;
    }

    string CollectionListData(List<string> listData) {
        string data = "";
        foreach (string no in listData) {
            data += no + "\n";
        }
        return data;
    }

    string CollectSwitchData() {
        string data = "";
        Dictionary<string, string> switchData = SingletonGeneral.instance.scenarioSystem.dictSwitch;
        foreach (KeyValuePair<string,string> item in switchData) {
            data += item.Key + "\t" + item.Value + "\n";
        }
        return data;
    }

    /// <summary>
    /// これを呼び出すと全部ロードする
    /// </summary>
    public void Load() {
        LoadSwitch();
        LoadStatus();
        SingletonGeneral.instance.itemDb.ItemDbInit();
        RedrawHud();
    }

    void RedrawHud() {
        PlayerView.instance.hud.RedrawHp();
        PlayerView.instance.hud.RedrawMp();
        PlayerView.instance.hud.RedrawSatiation();
    }

    void LoadStatus() {
        GameObject player = GameObject.Find("Player");
        GameObject dungeonRoot = SingletonGeneral.instance.dungeonRoot;
        PlayerConfig config = PlayerView.instance.config;

        List<string> statusData = FQCommon.Common.LoadSaveFile(
            SingletonGeneral.instance.GetStatusSavePath());
        foreach (string data in statusData) {
            string[] keyValue = data.Split("\t");
            SetLoadedSatus(
                keyValue[0], 
                keyValue[1], 
                player,
                dungeonRoot,
                config);
        }

    }

    void SetLoadedSatus(
        string key, 
        string value, 
        GameObject player,
        GameObject dungeonRoot,
        PlayerConfig config) 
    {
        Vector3 playerPos = player.transform.position;
        Vector3 dungeonPos = dungeonRoot.transform.position;
        switch (key) {
            case "PlayerX":
                playerPos.x = float.Parse(value);
                player.transform.position = playerPos;
                break;
            case "PlayerY":
                playerPos.y = float.Parse(value);
                player.transform.position = playerPos;
                break;
            case "PlayerZ":
                playerPos.z = float.Parse(value);
                player.transform.position = playerPos;
                break;
            case "DungeonX":
                dungeonPos.x = float.Parse(value);
                dungeonRoot.transform.position = dungeonPos;
                break;
            case "DungeonY":
                dungeonPos.y = float.Parse(value);
                dungeonRoot.transform.position = dungeonPos;
                break;
            case "DungeonZ":
                dungeonPos.z = float.Parse(value);
                dungeonRoot.transform.position = dungeonPos;
                break;
            case "MaxHp":
                config.SetMaxHp(int.Parse(value));
                break;
            case "MaxMp":
                config.SetMaxMp(int.Parse(value));
                break;
            case "NowHp":
                config.SetHp(int.Parse(value));
                break;
            case "NowMp":
                config.SetMp(int.Parse(value));
                break;
            case "Atk":
                config.SetAtk(int.Parse(value));
                break;
            case "Def":
                config.SetDef(int.Parse(value));
                break;
            case "Satiation":
                config.SetSatiation(float.Parse(value));
                break;
            case "MaxPageNo":
                config.SetMaxPageNo(int.Parse(value));
                break;

        }
        
    }

    void LoadSwitch() {
        Dictionary<string, string> dictSwitch = new Dictionary<string, string>();
        List<string> switchData = FQCommon.Common.LoadSaveFile(
            SingletonGeneral.instance.GetSwitchSavePath());
        foreach (string data in switchData) {
            string[] keyValue = data.Split("\t");
            dictSwitch[keyValue[0]] = keyValue[1];
        }
        SingletonGeneral.instance.scenarioSystem.dictSwitch = dictSwitch;
    }

}
