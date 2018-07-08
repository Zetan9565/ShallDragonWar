using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public enum UpdateType
{
    LateUpdate,
    FixedUpdate,
    Update
}

[AddComponentMenu("Other/Zetan/Third Person Camera Follow")]
public class CameraFollow : MonoBehaviour {

    public enum RotateType
    {
        UableRotate,
        HorizontalOnly,
        VerticalOnly,
        Both
    }

    public static CameraFollow Camera;
    
    public Transform target;
    public Vector3 headOffset = new Vector3(0, 1.7f, 0);
    public LayerMask collisionLayer;
    public UpdateType updateType;
    public bool smooth;
    [Range(0.1f, 2f)] public float smoothLevel = 0.5f;
    public RotateType rotateType;
    public float rotateSpeed = 10.0f;
    public float zoomSpeed = 5f;
    [Range(0.5f, 5)]
    public float minDistance = 3.0f;
    [Range(5, 20)]
    public float maxDistance = 15.0f;
    public float maxLookUpAngle = 70.0f;
    public float maxLookDownAngle = 30.0f;
    public bool rotateAble;
    public bool gizmos;

    float OriginDistance;
    float currentDistance;
    Vector3 direction;
    Vector3 disOffset;
    Vector3 visibleEuler;

    private void Awake()
    {
        Camera = this;
        ResetView();
    }

    // Update is called once per frame
    void Update() {
        if(updateType==UpdateType.Update)
        {
            visibleEuler = ConvertEulerAngle(transform.rotation.eulerAngles);
            float h = CrossPlatformInputManager.GetAxis("Mouse X");
            float v = CrossPlatformInputManager.GetAxis("Mouse Y");
            float zoom = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
            Translate(zoom);
            if(rotateAble) Rotate(h, v);
        }
    }

    private void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
        {
            visibleEuler = ConvertEulerAngle(transform.rotation.eulerAngles);
            float h = CrossPlatformInputManager.GetAxis("Mouse X");
            float v = CrossPlatformInputManager.GetAxis("Mouse Y");
            float zoom = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
            Translate(zoom);
            if(rotateAble) Rotate(h, v);
        }
    }

    private void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
        {
            visibleEuler = ConvertEulerAngle(transform.rotation.eulerAngles);
            float h = CrossPlatformInputManager.GetAxis("Mouse X");
            float v = CrossPlatformInputManager.GetAxis("Mouse Y");
            float zoom = CrossPlatformInputManager.GetAxis("Mouse ScrollWheel");
            Translate(zoom);
            if (rotateAble) Rotate(h, v);
        }
    }

    void Translate(float zoom)
    {
        if (!target) return;
        OriginDistance -= zoom * zoomSpeed;
        OriginDistance = Mathf.Clamp(OriginDistance, minDistance, maxDistance);
        RaycastHit hit;
        if (Physics.Raycast(target.position + headOffset, transform.position - target.position - headOffset, out hit, OriginDistance,
            collisionLayer, QueryTriggerInteraction.Ignore))//检测层依据需求更改
        {
            currentDistance = hit.distance;
        }
        else
        {
            currentDistance = OriginDistance;
        }
        disOffset = direction.normalized * currentDistance;
        if(smooth && rotateType==RotateType.Both) transform.position = Vector3.Lerp(transform.position, target.position + headOffset + disOffset, smoothLevel);//平滑移动，但累计差会导致相机逐渐回到某个特定视角，且伴随有轻微抖动
        else transform.position = target.position + headOffset + disOffset;//旋转时伴随轻微卡顿
        transform.LookAt(target.position + headOffset);
        disOffset = Vector3.zero;
    }

    void Rotate(float h, float v)
    {
        if (!target) return;
        if (rotateType != RotateType.UableRotate)
        {
            float finallyRotateV = v * rotateSpeed;
            if (finallyRotateV + visibleEuler.x >= maxLookUpAngle)
            {
                finallyRotateV = visibleEuler.x + finallyRotateV - maxLookUpAngle;
            }
            else if (finallyRotateV + visibleEuler.x <= -maxLookDownAngle)
            {
                finallyRotateV = visibleEuler.x + finallyRotateV + maxLookDownAngle;
            }           
            if (rotateType == RotateType.Both)
            {
                //左右旋转
                transform.RotateAround(target.position + headOffset, transform.up, h * rotateSpeed);
                //上下旋转
                transform.RotateAround(target.position + headOffset, transform.right, -finallyRotateV);
            }
            else if (rotateType == RotateType.HorizontalOnly)
            {
                transform.RotateAround(target.position + headOffset, Vector3.up, h * rotateSpeed);
            }
            else if (rotateType == RotateType.VerticalOnly)
            {
                transform.RotateAround(target.position + headOffset, transform.right, -finallyRotateV);
            }
            transform.LookAt(target.position + headOffset);
            direction = (transform.position - target.position - headOffset).normalized;
        }
    }

    public float ConvertAngle(float value)
    {
        float angle = value - 180;

        if (angle > 0)
            return angle - 180;

        return angle + 180;
    }

    public Vector3 ConvertEulerAngle(Vector3 euler)
    {
        return new Vector3(ConvertAngle(euler.x), ConvertAngle(euler.y), ConvertAngle(euler.z));
    }

    public void ResetView()
    {
        if (!target) return;
        if (minDistance > maxDistance)
        {
            maxDistance += minDistance;
            minDistance = maxDistance - minDistance;
            maxDistance -= minDistance;
        }
        if (rotateType == RotateType.VerticalOnly || rotateType == RotateType.Both) direction = -target.forward;
        else direction = new Vector3(1, 1, -1).normalized;
        disOffset = direction * (minDistance + maxDistance) / 2;
        transform.position = target.position + headOffset + disOffset;
        OriginDistance = disOffset.magnitude;
        disOffset = Vector3.zero;
        rotateAble = true;
    }

    public void SetTarget(Transform new_target)
    {
        target = new_target;
        ResetView();
    }

    private void OnDrawGizmos()
    {
        if (!target || !gizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position + headOffset, transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position + headOffset, minDistance);
        Gizmos.DrawLine(target.position + headOffset + target.forward * minDistance, target.position + headOffset + target.forward * maxDistance);
        Gizmos.DrawLine(target.position + headOffset - target.forward * minDistance, target.position + headOffset - target.forward * maxDistance);
        Gizmos.DrawLine(target.position + headOffset + target.up * minDistance, target.position + headOffset + target.up * maxDistance);
        Gizmos.DrawLine(target.position + headOffset - target.up * minDistance, target.position + headOffset - target.up * maxDistance);
        Gizmos.DrawLine(target.position + headOffset + target.right * minDistance, target.position + headOffset + target.right * maxDistance);
        Gizmos.DrawLine(target.position + headOffset - target.right * minDistance, target.position + headOffset - target.right * maxDistance);
        Gizmos.DrawWireSphere(target.position + headOffset, maxDistance);
    }
}
