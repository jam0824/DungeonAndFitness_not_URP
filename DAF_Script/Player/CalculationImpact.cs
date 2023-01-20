using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationImpact : MonoBehaviour
{
    int LIST_MAX = 10;
    List<Vector3> listPosition;

    // Start is called before the first frame update
    void Start()
    {
        listPosition = new List<Vector3>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (listPosition.Count == LIST_MAX) {
            listPosition.RemoveAt(LIST_MAX - 1);
        }
        Vector3 pos = this.transform.position;
        listPosition.Insert(0, pos);
    }

    public List<Vector3> GetListPosition() {
        return listPosition;
    }
}
