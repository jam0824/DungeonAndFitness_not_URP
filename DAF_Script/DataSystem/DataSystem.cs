using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSystem : MonoBehaviour
{
    public static DataSystem instance;
    public DataPlayer dataPlayer { set; get; }
    public DataItem dataItem { set; get; }
    public DataScenario dataScenario { set; get; }
    public string log { set; get; }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DataSystemInit();
        }
        else {
            Destroy(gameObject);
        }
    }

    void DataSystemInit() {
        log = "";
        dataPlayer = GetComponent<DataPlayer>();
        dataPlayer.DataPlayerInit();
        dataItem = GetComponent<DataItem>();
        dataItem.DataItemInit();
        dataScenario = GetComponent<DataScenario>();
        dataScenario.DataScenarioInit();
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
