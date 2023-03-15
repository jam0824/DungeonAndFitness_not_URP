using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TurnOff(
            GameObject.Find("DungeonSystem")
            .GetComponent<DungeonSystem>()
            .GetIsDungeon());
    }

    void TurnOff(bool isDungeon) {
        if (!isDungeon) gameObject.SetActive(false);
    }
}
