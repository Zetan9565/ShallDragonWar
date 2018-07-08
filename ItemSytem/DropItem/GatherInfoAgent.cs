using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEnums
{
    public enum GatherAgentType
    {
        Grass,
        HighGrass,
        Tree,
        Ore,
        Stone
    }
}

[RequireComponent(typeof(GatherTrigger))]
[RequireComponent(typeof(DropItemListAgent))]
public class GatherInfoAgent : MonoBehaviour {

    public string gatherID;
    public string Name;
    public float recoverTime;
    public float gatherTime;
    [Header("掉落品列表")]
    [Tooltip("格式：物品ID,最大掉落数量,掉落概率")]
    public string[] dropItemsInput;
    public MyEnums.GatherAgentType agentType;
    [HideInInspector]
    public GatherTrigger gatherTrigger;
    [HideInInspector]
    public DropItemListAgent dropItemListAgent;


    // Use this for initialization
    void Start () {
        gatherTrigger = GetComponentInChildren<GatherTrigger>();
        dropItemListAgent = GetComponentInChildren<DropItemListAgent>();
        //dropItemListAgent.dropItemList = new List<DropItemInfo>();
    }
	

    public void OnGatherFinished()
    {
        if (GatherManager.Instance.gatherInfoAgent == this) GatherManager.Instance.CantGather();
        dropItemListAgent.GetDropItems(dropItemsInput);
        PickUpManager.Instance.dropItemListAgent = dropItemListAgent;
        PickUpManager.Instance.pickUpButton.onClick.Invoke();
        gatherTrigger.gatherFinished = true;
        StartCoroutine(StartRecover());
    }

    public void OnRecover()
    {
        dropItemListAgent.dropItemList = new List<DropItemInfo>();
        gatherTrigger.gatherFinished = false;
        gatherTrigger.trigger.enabled = true;
    }

    IEnumerator StartRecover()
    {
        yield return new WaitForSeconds(recoverTime);
        OnRecover();
    }
}
