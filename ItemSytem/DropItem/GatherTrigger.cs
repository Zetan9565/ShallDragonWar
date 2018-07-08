using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherTrigger : MonoBehaviour {

    [HideInInspector]
    public GatherInfoAgent gatherInfoAgent;
    [HideInInspector]
    public Collider trigger;
    [HideInInspector]
    public bool gatherFinished;

	// Use this for initialization
	void Start () {
        gatherInfoAgent = GetComponentInParent<GatherInfoAgent>();
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
            if (collider.GetType() != typeof(MeshCollider))
            {
                trigger = collider;
                trigger.isTrigger = true;
                break;
            }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if (!PickUpManager.Instance.dropItemListAgent)
            {
                GatherManager.Instance.CanGather(gatherInfoAgent);
            }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            if (!GatherManager.Instance.gatherInfoAgent && !gatherFinished && other.enabled)
            {
                GatherManager.Instance.gatherInfoAgent = gatherInfoAgent;
                if(PlayerInfoManager.Instance.PlayerInfo.IsGathering) GatherManager.Instance.CanGather(gatherInfoAgent);
            }
            else if(!other.enabled)
            {
                if (GatherManager.Instance.gatherInfoAgent && GatherManager.Instance.gatherInfoAgent == gatherInfoAgent)
                    GatherManager.Instance.CantGather();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GatherManager.Instance.gatherInfoAgent && GatherManager.Instance.gatherInfoAgent == gatherInfoAgent)
            {
                GatherManager.Instance.CantGather();
                PlayerLocomotionManager.Instance.CancelGather();
            }
            if(PickUpManager.Instance.dropItemListAgent == gatherInfoAgent.dropItemListAgent) PickUpManager.Instance.CantPick();
            if (gatherFinished)
                trigger.enabled = false;
        }
    }
}
