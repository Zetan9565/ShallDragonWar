using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ScrollCircle : ScrollRect {
    float radius = 0.0f;
    public Vector2 movePoint;
    Vector2 start;
    Vector2 draging;
    public float moveAngle;
    
    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    // Use this for initialization
    protected override void Start () {
        base.Start();
        radius = (viewport.transform as RectTransform).sizeDelta.x * 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
        //if (Mathf.Abs( content.anchoredPosition.x) >= 0.5f || Mathf.Abs( content.anchoredPosition.y) >= 0.5f) movePoint = content.anchoredPosition.normalized;
        //else movePoint = Vector2.zero;
        //Debug.Log(movePoint);
	}

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        start = content.position - transform.position;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        Vector2 position = content.anchoredPosition;
        if(position.magnitude > radius)
        {
            position = position.normalized * radius;
            SetContentAnchoredPosition(position);
        }
        movePoint = content.anchoredPosition.normalized;
        draging = content.position - transform.position;
        m_HorizontalVirtualAxis.Update(movePoint.x);
        m_VerticalVirtualAxis.Update(movePoint.y);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        movePoint = Vector2.zero;
        //Debug.Log("转过了" + Vector2.Angle(start, draging) + "度");
        moveAngle = Vector2.Angle(start, draging);
        m_HorizontalVirtualAxis.Update(movePoint.x);
        m_VerticalVirtualAxis.Update(movePoint.y);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Horizontal");
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Vertical");
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // remove the joysticks from the cross platform input
        m_HorizontalVirtualAxis.Remove();
        m_VerticalVirtualAxis.Remove();
    }
}
