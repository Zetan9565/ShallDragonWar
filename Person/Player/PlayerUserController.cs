using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerController))]
public class PlayerUserController : MonoBehaviour
{
    public static PlayerUserController Self;
    public PlayerController Player { get; private set; }
    // Use this for initialization
    void Start()
    {
        Player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Awake()
    {
        Self = this;
    }

    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 move = new Vector3();
        if (Camera.main)
        {
            move = h * Camera.main.transform.right + v * Camera.main.transform.forward;
        }
        else
        {
            move = h * Vector3.right + v * Vector3.forward;
        }
        bool IsJump = false;
        bool IsCrouch = false;
        bool IsWalk = false;
#if UNITY_ANDROID
        IsJump = CrossPlatformInputManager.GetButtonDown("Jump");
        IsCrouch = CrossPlatformInputManager.GetButton("Crouch");
        IsWalk = CrossPlatformInputManager.GetButton("Walk");
#else
        IsJump = Input.GetButtonDown("Jump");
        IsCrouch = Input.GetButton("Crouch");
        IsWalk = Input.GetButton("Walk");
#endif
        Player.Movement(move, IsJump, IsCrouch, IsWalk);
    }
}
