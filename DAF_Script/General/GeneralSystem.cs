using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSystem : MonoBehaviour
{
    public GameObject ItemCanvas;

    public GameObject DamageTextCanvas;
    public GameObject PlayerDamageTextCanvas;
    public GameObject NoticeTextCanvas;





    void Awake() {
        //OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }




    public GameObject GetItemCanvas() {
        return ItemCanvas;
    }
    public GameObject GetPrefabDamageTextCanvas() {
        return DamageTextCanvas;
    }
    public GameObject GetPrefabPlayerDamageTextCanvas() {
        return PlayerDamageTextCanvas;
    }
    public GameObject GetPrefabNoticeTextCanvas() {
        return NoticeTextCanvas;
    }


   

}
