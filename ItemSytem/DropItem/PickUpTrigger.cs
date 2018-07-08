using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrigger : MonoBehaviour {

    [HideInInspector]
    public DropItemListAgent dropItemListAgent;
    [HideInInspector]
    public Collider trigger;
    public float destroyTime;

    // Use this for initialization
    void Start () {
        dropItemListAgent = GetComponentInParent<DropItemListAgent>();
        trigger = GetComponent<Collider>();
        if (!trigger.isTrigger) trigger.isTrigger = true;
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        if (dropItemListAgent.dropItemList.Count <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if (!PickUpManager.Instance.dropItemListAgent)
            {
                PickUpManager.Instance.CanPick(dropItemListAgent);
            }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            if (!PickUpManager.Instance.dropItemListAgent && other.enabled)
            {
                PickUpManager.Instance.CanPick(dropItemListAgent);
            }
            else if(!other.enabled)
            {
                if (PickUpManager.Instance.dropItemListAgent && PickUpManager.Instance.dropItemListAgent == dropItemListAgent)
                    PickUpManager.Instance.CantPick();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            if (PickUpManager.Instance.dropItemListAgent && PickUpManager.Instance.dropItemListAgent == dropItemListAgent)
                PickUpManager.Instance.CantPick();
    }

    private void OnDisable()
    {
        if (PickUpManager.Instance.dropItemListAgent && PickUpManager.Instance.dropItemListAgent == dropItemListAgent)
            PickUpManager.Instance.CantPick();
    }

    private void OnDestroy()
    {
        if (PickUpManager.Instance.dropItemListAgent && PickUpManager.Instance.dropItemListAgent == dropItemListAgent)
            PickUpManager.Instance.CantPick();
    }
}
