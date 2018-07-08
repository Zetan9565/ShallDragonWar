using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            WarehouseManager.Instance.CanStore();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            if (other.enabled)
                WarehouseManager.Instance.CanStore();
            else WarehouseManager.Instance.CantStore();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            WarehouseManager.Instance.CantStore();
    }
}
