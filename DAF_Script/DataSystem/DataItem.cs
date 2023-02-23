using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataItem : MonoBehaviour
{
    public List<string> playerItemList { set; get; }
    public List<string> playerCollectionList { set; get; }

    public void DataItemInit() {
        playerItemList = new List<string>();
        playerCollectionList = new List<string>();
    }
}
