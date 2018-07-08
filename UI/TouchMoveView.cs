using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TouchMoveView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Image Panel;
    Touch touch;
    Vector2 pointerLastPos;

    CrossPlatformInputManager.VirtualAxis m_MouseXVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_MouseYVirtualAxis; // Reference to the joystick in the cross platform input

    // Use this for initialization
    void Start () {
        Panel = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        pointerLastPos = touch.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        foreach (Touch touch in Input.touches)
            if (touch.position == eventData.pointerCurrentRaycast.screenPosition)
            {
                this.touch = touch;
                break;
            }
    }

    public void OnDrag(PointerEventData eventData)
    {

        foreach (Touch touch in Input.touches)
            if (touch.position == eventData.pointerCurrentRaycast.screenPosition)
            {
                this.touch = touch;
                break;
            }
        if (Input.touchCount > 0)
        {
            if (pointerLastPos != touch.position)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;
                m_MouseXVirtualAxis.Update(touchDeltaPosition.x);
                m_MouseYVirtualAxis.Update(touchDeltaPosition.y);
            }
            else
            {
                m_MouseXVirtualAxis.Update(0);
                m_MouseYVirtualAxis.Update(0);
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_MouseXVirtualAxis.Update(0);
        m_MouseYVirtualAxis.Update(0);
    }

    protected void OnEnable()
    {
        m_MouseXVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Mouse X");
        CrossPlatformInputManager.RegisterVirtualAxis(m_MouseXVirtualAxis);
        m_MouseYVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Mouse Y");
        CrossPlatformInputManager.RegisterVirtualAxis(m_MouseYVirtualAxis);
    }

    protected void OnDisable()
    {
        // remove the joysticks from the cross platform input
        m_MouseXVirtualAxis.Remove();
        m_MouseYVirtualAxis.Remove();
    }
}
