using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Animator m_Animator;
    [HideInInspector]
    public Rigidbody m_Rigidbody;
    [HideInInspector]
    public CapsuleCollider Body;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    [Space]
    public LayerMask groundLayer = 1;
    public LayerMask waterLayer = 0;
    public LayerMask crouchIgnoreLayer = 0;
    public float MoveSpeed = 4.0f;
    public float TurnSpeed = 12.0f;
    [Range(6, 20)]
    public float JumpPower = 8.0f;
    public float GroundCheckDistance = 0.3f;
    float OriginGroundCheckDistance;
    //public float stepHeight;
    [Space]
    public bool IsGrounded;
    public bool Falling;
    public bool IsHeadWatered;
    public bool IsBodyWatered;
    public float waterLevel;
    [Space]
    public bool Fighting;
    public bool Crouching;
    public bool Walking;
    [Space]
    public bool moveAble = true;
    public bool rotateAble = true;
    //public bool jumpAble = true;
    public float m_GravityMultiplier = 1.0f;
    [Space]
    public bool gizmos;
    [HideInInspector]
    public Vector3 GroundNormal;

    // Use this for initialization
    void Awake()
    {
        Body = GetComponent<CapsuleCollider>();
        m_CapsuleCenter = Body.center;
        m_CapsuleHeight = Body.height;
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        OriginGroundCheckDistance = GroundCheckDistance;
    }

    public void Movement(Vector3 move, bool jump, bool crouch, bool walk)
    {
        GroundCheck();
        if (move.magnitude > 1) move.Normalize();
        Rotating(move.x, move.z);
        Move(move);
        Jump(jump);
        Walking = walk;
        if (!IsGrounded)
        {
            if (!IsHeadWatered)
            {
                Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
                m_Rigidbody.AddForce(extraGravityForce);
            }
            GroundCheckDistance = m_Rigidbody.velocity.y > 0 ? 0.01f : OriginGroundCheckDistance;
        }
        if (IsHeadWatered)
        {
            m_Rigidbody.useGravity = false;
            crouch = false;
        }
        else m_Rigidbody.useGravity = true;
        ScaleCapsuleForCrouching(crouch);
        PreventStandingInLowHeadroom();
        CheckWaterLevel();
        UpdateAnimation(move);
    }

    void Move(Vector3 moveDir)
    {
        if (moveAble)
        {
            if (IsGrounded && !IsHeadWatered)
                m_Rigidbody.velocity = Vector3.ProjectOnPlane(Vector3.ProjectOnPlane(transform.forward * moveDir.magnitude, GroundNormal), transform.right).normalized *
                    MoveSpeed * (Crouching || (Walking && !Fighting) ? 0.4f : Fighting ? 0.8f : 1) + Vector3.down;
            else if (IsHeadWatered) m_Rigidbody.velocity = (transform.forward * moveDir.magnitude).normalized * MoveSpeed * 0.4f;
            else if (Falling && !IsGrounded)
            {
                if (new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.z).magnitude < 5.0f)
                    m_Rigidbody.velocity = m_Rigidbody.velocity + new Vector3(moveDir.x, 0, moveDir.z).normalized * MoveSpeed * 0.05f;
            }
        }
    }

    void Rotating(float h, float v)
    {
        if ((h == 0 && v == 0)) return;
        if (rotateAble)
        {
            if (IsGrounded || (!IsGrounded && Falling) || IsHeadWatered)
            {
                Vector3 targetDir = new Vector3(h, 0, v);
                Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);//使transform.forward对齐targetDir所需的旋转量
                Quaternion newQuaternion = Quaternion.Lerp(m_Rigidbody.rotation, targetRotation, IsHeadWatered ? 0.4f * TurnSpeed : TurnSpeed * Time.deltaTime);
                m_Rigidbody.MoveRotation(newQuaternion);
            }
        }
    }

    void Jump(bool jump)
    {
        if (jump && (IsGrounded || IsHeadWatered) && !Crouching)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || m_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Move"))
            {
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, JumpPower, m_Rigidbody.velocity.z);
                IsGrounded = false;
                GroundCheckDistance = 0.1f;
            }
        }
    }

    void ScaleCapsuleForCrouching(bool crouch)
    {
        if ((IsGrounded && crouch))
        {
            if (Crouching) return;
            Body.height = Body.height / 1.5f;
            Body.center = Body.center / 1.5f;
            Crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * Body.radius * 0.5f, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - Body.radius * 0.5f;
            RaycastHit hit;
            if (Physics.SphereCast(crouchRay, Body.radius * 0.5f, out hit, crouchRayLength, ~crouchIgnoreLayer, QueryTriggerInteraction.Ignore))
            {
                Crouching = true;
                return;
            }
            Crouching = false;
            Body.height = m_CapsuleHeight;
            Body.center = m_CapsuleCenter;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        if (!Crouching)
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * Body.radius * 0.5f, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - Body.radius * 0.5f;
            if (Physics.SphereCast(crouchRay, Body.radius * 0.5f, crouchRayLength, ~crouchIgnoreLayer, QueryTriggerInteraction.Ignore))
                Crouching = true;
        }
    }

    void UpdateAnimation(Vector3 move)
    {
        m_Animator.SetFloat(Animator.StringToHash("Forward"), move.magnitude);
        m_Animator.SetBool(Animator.StringToHash("IsGrounded"), IsGrounded);
        m_Animator.SetBool(Animator.StringToHash("Crouch"), Crouching);
        m_Animator.SetBool(Animator.StringToHash("Walking"), Walking);
        m_Animator.SetFloat(Animator.StringToHash("DownSpeed"), m_Rigidbody.velocity.y);
        m_Animator.SetBool(Animator.StringToHash("IsHeadWatered"), IsHeadWatered);
        m_Animator.SetBool(Animator.StringToHash("IsBodyWatered"), IsBodyWatered);
        Falling = m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Falling");
    }

    void GroundCheck()
    {
        bool SecondGroundCheck = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, GroundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore))
            IsGrounded = true;
        else IsGrounded = false;
        if (Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius * transform.forward,
            transform.position + Vector3.up * 0.3f + Body.radius * transform.forward - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius * -transform.forward,
transform.position + Vector3.up * 0.3f + Body.radius * -transform.forward - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius * transform.right,
transform.position + Vector3.up * 0.3f + Body.radius * transform.right - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius * -transform.right,
transform.position + Vector3.up * 0.3f + Body.radius * -transform.right - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (transform.forward + transform.right),
transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (transform.forward + transform.right) - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (transform.forward - transform.right),
transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (transform.forward - transform.right) - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (-transform.forward + transform.right),
transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (-transform.forward + transform.right) - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore) ||

Physics.Linecast(transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (-transform.forward - transform.right),
transform.position + Vector3.up * 0.3f + Body.radius / Mathf.Sqrt(2) * (-transform.forward - transform.right) - Vector3.up * (0.2f + GroundCheckDistance), out hit, groundLayer, QueryTriggerInteraction.Ignore))
        {
            GroundNormal = GetGroundNormal();
            SecondGroundCheck = true;
        }
        IsGrounded = IsGrounded || SecondGroundCheck;
        if (!IsGrounded) GroundNormal = Vector3.up;
    }

    Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        Vector3 point1, point2, point3, point4;
        if (Physics.Raycast(transform.position + Vector3.up * 0.25f * m_CapsuleHeight + Body.radius * transform.forward,
Vector3.down, out hit, GroundCheckDistance + 0.25f * m_CapsuleHeight, groundLayer, QueryTriggerInteraction.Ignore))
            point1 = hit.point;
        else point1 = transform.position + Body.radius * transform.forward;

        if (Physics.Raycast(transform.position + Vector3.up * 0.25f * m_CapsuleHeight + Body.radius * -transform.forward,
        Vector3.down, out hit, GroundCheckDistance + 0.25f * m_CapsuleHeight, groundLayer, QueryTriggerInteraction.Ignore))
            point2 = hit.point;
        else point2 = transform.position + Body.radius * -transform.forward;

        if (Physics.Raycast(transform.position + Vector3.up * 0.25f * m_CapsuleHeight + Body.radius * transform.right,
        Vector3.down, out hit, GroundCheckDistance + 0.25f * m_CapsuleHeight, groundLayer, QueryTriggerInteraction.Ignore))
            point3 = hit.point;
        else point3 = transform.position + Body.radius * transform.right;

        if (Physics.Raycast(transform.position + Vector3.up * 0.25f * m_CapsuleHeight + Body.radius * -transform.right,
        Vector3.down, out hit, GroundCheckDistance + 0.25f * m_CapsuleHeight, groundLayer, QueryTriggerInteraction.Ignore))
            point4 = hit.point;
        else point4 = transform.position + Body.radius * -transform.right;

        /*Debug.DrawLine(point1, point2);
        Debug.DrawLine(point3, point4);*/
        return Vector3.Cross(point1 - point2, point3 - point4);
    }

    /*void CheckStep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * stepHeight / 2, transform.forward, Body.radius * 1.1f))
            if (!Physics.Raycast(transform.position + Vector3.up * stepHeight, transform.forward, Body.radius * 1.1f))
                if (Physics.Raycast(transform.position + transform.forward * Body.radius * 1.1f + Vector3.up * m_CapsuleHeight, Vector3.down, out hit))
                {
                    m_Rigidbody.MovePosition(Vector3.Lerp(transform.position, hit.point, 0.5f));
                }
                else transform.position = transform.position;
    }*/

    void CheckWaterLevel()
    {
        if (IsBodyWatered)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * m_CapsuleHeight, Vector3.down, out hit, m_CapsuleHeight, waterLayer, QueryTriggerInteraction.Collide))
            {
                waterLevel = m_CapsuleHeight - hit.distance;
            }
            else
            {
                waterLevel = 0.01f;
            }
        }
        else waterLevel = 0;
    }

    private void OnDrawGizmos()
    {
        if (!gizmos) return;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.01f, Vector3.up);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius
            * transform.forward, transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius * transform.forward - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius
            * -transform.forward, transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius * -transform.forward - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius
            * transform.right, transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius * transform.right - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius
            * -transform.right, transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius * -transform.right - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (transform.forward + transform.right),
            transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (transform.forward + transform.right) - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (transform.forward - transform.right),
            transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (transform.forward - transform.right) - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (-transform.forward + transform.right),
            transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (-transform.forward + transform.right) - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawLine(transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (-transform.forward - transform.right),
            transform.position + Vector3.up * 0.3f + GetComponent<CapsuleCollider>().radius / Mathf.Sqrt(2) * (-transform.forward - transform.right) - Vector3.up * (0.2f + GroundCheckDistance));
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.up * 0.1f + Vector3.down * GroundCheckDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GroundNormal);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.ProjectOnPlane(Vector3.ProjectOnPlane(transform.forward, GroundNormal), transform.right));
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, Vector3.ProjectOnPlane(transform.forward, GroundNormal));
        /*Gizmos.DrawLine(transform.position + Vector3.up * GetComponent<CapsuleCollider>().height + GetComponent<CapsuleCollider>().radius * 1.1f * transform.forward, 
            transform.position + Vector3.up * stepHeight + GetComponent<CapsuleCollider>().radius * 1.1f * transform.forward);*/
    }
}
