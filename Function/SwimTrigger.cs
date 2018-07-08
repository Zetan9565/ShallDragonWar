using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimTrigger : MonoBehaviour {

    [SerializeField]
    private List<PlayerController> playerControllers;
    [SerializeField]
    bool AbleBouncy;
    public float Bouncy = 20.0f;

	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
        if (playerControllers.Count > 0)
            AbleBouncy = true;
        else AbleBouncy = false;
    }

    private void FixedUpdate()
    {
        if (AbleBouncy)
        {
            foreach (PlayerController playerController in playerControllers)
                playerController.m_Rigidbody.AddForce(Vector3.up * Bouncy);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Head")
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController)
            {    //Debug.Log("HeadIntoWater");
                playerController.IsHeadWatered = true;
                playerController.IsGrounded = false;
                //playerController.m_Rigidbody.Sleep();
                playerController.m_Rigidbody.useGravity = false;
                playerController.moveAble = true;
                playerController.rotateAble = true;
            }
        }
        else if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController) playerController.IsBodyWatered = true;
        }
        else if (other.tag == "HeadCheck")
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController)
            {
                if (!playerControllers.Contains(playerController)) playerControllers.Add(playerController);
            }
        }
        else if (other.transform.root.tag == "Horse")
        {
            //Debug.Log("HorseIn");
            MalbersAnimations.Animal animal = other.GetComponentInParent<MalbersAnimations.Animal>();
            if (animal)
            {
                animal.RaycastWater();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Head")
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController)
            {
                //Debug.Log("HeadInWater");
                playerController.IsHeadWatered = true;
                playerController.IsGrounded = false;
            }
        }
        else if (other.tag == "Player")
        {
            //Debug.Log("BodyInWater");
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(playerController) playerController.IsBodyWatered = true;
        }
        else if (other.transform.root.tag == "Horse")
        {
            MalbersAnimations.Animal animal = other.GetComponentInParent<MalbersAnimations.Animal>();
            if (animal) animal.RaycastWater();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Head")
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController)
            {
                playerController.IsHeadWatered = false;
                playerController.m_Rigidbody.useGravity = true;
            }
        }
        else if (other.tag == "Player")
        {
            //Debug.Log("BodyOutWater");
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(playerController) playerController.IsBodyWatered = false;
        }
        else if (other.tag == "HeadCheck")
        {
            PlayerController playerController = other.GetComponentInParent<PlayerController>();
            if (playerController && playerControllers.Contains(playerController))
            {
                playerControllers.Remove(playerController);
            }
        }
        /*else if(other.tag == "HorseFoot")
        {
            MalbersAnimations.Animal animal = other.GetComponent<MalbersAnimations.Animal>();
            if (animal) animal.Swim = false;
        }*/
    }
}
