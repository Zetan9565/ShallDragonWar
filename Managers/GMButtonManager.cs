using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GMButtonManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Button>().interactable = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Button>().interactable = true;
    }

    /*// Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }*/
}
