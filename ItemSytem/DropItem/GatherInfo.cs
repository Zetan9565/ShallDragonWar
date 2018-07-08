using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherInfo {

    public string ID;
    public string Name;
    public float recoverTime;
    public string dropItemsInput;

    public GatherInfo(string id, string name, float rec_time, string dropitems)
    {
        ID = id;
        Name = name;
        recoverTime = rec_time;
        dropItemsInput = dropitems;
    }
}
