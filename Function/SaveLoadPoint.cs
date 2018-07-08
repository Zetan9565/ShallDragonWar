using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadPoint : MonoBehaviour {

    public bool gizmos;
    public Color color = Color.white;
    Vector3 rayStartPoint;
    bool checkAnimalPos;

    //private void Awake()
    //{
    //    GameDataManager.Self.savePoints.Add(this);
    //}

    // Use this for initialization
    /*void Start () {
		
	}*/
	
	// Update is called once per frame
	void Update () {
        if(checkAnimalPos) rayStartPoint = new Vector3(transform.position.x + Random.Range(-1f * Mathf.Sqrt(2), 1f * Mathf.Sqrt(2)), transform.position.y + 500, transform.position.z + Random.Range(-1f * Mathf.Sqrt(2), 1f * Mathf.Sqrt(2)));
    }

    private void OnDrawGizmos()
    {
        if (!gizmos) return;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

    public float GetDistance()
    {
        if (PlayerInfoManager.Instance && PlayerInfoManager.Instance.isInit) return Vector3.Distance(PlayerInfoManager.Instance.Player.transform.position, transform.position);
        else return 0;
    }

    public void OnIconClick()
    {
        Confirm.Self.NewConfirm("确认前往该地？", OnConfirm);
    }

    void OnConfirm()
    {
        if (PlayerInfoManager.Instance.PlayerInfo.IsMounting)
        {
            NotificationManager.Instance.NewNotification("该状态下无法使用御风传送");
            return;
        }
        //GameDataManager.Self.MovePositon(transform.position);
        ScreenFader.Instance.onFadeInEnd.RemoveAllListeners();
        ScreenFader.Instance.onFadeInEnd.AddListener(delegate
        {
            CameraFollow.Camera.SetTarget(transform);
        });
        ScreenFader.Instance.onFadeOutStart.RemoveAllListeners();
        ScreenFader.Instance.onFadeOutStart.AddListener(MoveToHere);
        ScreenFader.Instance.AutoFadeInAndOut(2);
    }

    public void ResetToHere(bool relive)
    {
        ScreenFader.Instance.onFadeInEnd.RemoveAllListeners();
        ScreenFader.Instance.onFadeInEnd.AddListener(delegate
        {
            CameraFollow.Camera.SetTarget(transform);
        });
        ScreenFader.Instance.onFadeOutStart.RemoveAllListeners();
        if (relive) ScreenFader.Instance.onFadeOutStart.AddListener(delegate { MoveToHere(); PlayerInfoManager.Instance.PlayerInfo.Relive(); });
        else ScreenFader.Instance.onFadeOutStart.AddListener(MoveToHere);
        ScreenFader.Instance.AutoFadeInAndOut(2);
    }

    void MoveToHere()
    {
        CameraFollow.Camera.SetTarget(PlayerInfoManager.Instance.Player.transform);
        PlayerInfoManager.Instance.Player.transform.position = transform.position;
        if(transform.position.y <= 100) StartCoroutine(CheckGroudForAnimal());
    }

    IEnumerator CheckGroudForAnimal()
    {
        RaycastHit hit = new RaycastHit();
        checkAnimalPos = true;
        yield return new WaitUntil(() => Physics.Raycast(rayStartPoint, Vector3.down, out hit, Mathf.Infinity, PlayerLocomotionManager.Instance.playerController.groundLayer, QueryTriggerInteraction.Ignore)
        && hit.collider.tag != "Building");
        /*GameObject gameObject = new GameObject("dsadsad");
        gameObject.AddComponent<GizmoVisualizer>();
        gameObject.transform.position = hit.point;
        Debug.Log(hit.collider.tag);*/
        checkAnimalPos = false;
        FindObjectOfType<MalbersAnimations.Animal>().transform.position = hit.point + Vector3.up * 0.1f;
    }
}
