using UnityEngine;
using UnityEngine.UI;

public class GatherManager : MonoBehaviour {

    public static GatherManager Instance;
    public Button gatherButton;
    public Text gatherName;

    public GatherInfoAgent gatherInfoAgent;
    bool canGather;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        MyTools.SetActive(gatherName.transform.parent.gameObject, false);
        MyTools.SetActive(gatherButton.gameObject, false);
        gatherButton.onClick.AddListener(OnGatherButtonClick);
    }

    private void Update()
    {
        if (gatherInfoAgent && gatherInfoAgent.dropItemListAgent
            && gatherInfoAgent.dropItemListAgent.dropItemList!= null
            && gatherInfoAgent.dropItemListAgent.dropItemList.Count < 0)
            gatherInfoAgent = null;
        if (canGather && Input.GetButtonDown("Gather"))
            OnGatherButtonClick();
    }

    public void OnGatherButtonClick()
    {
        if (PlayerInfoManager.Instance.PlayerInfo.IsFighting || !PlayerLocomotionManager.Instance.AnimatorStateInfo.IsTag("Idle"))
        {
            NotificationManager.Instance.NewNotification("该状态下无法采集");
            return;
        }
        PlayerLocomotionManager.Instance.SetPlayerGatherAnima();
        PlayerLocomotionManager.Instance.playerRigidbd.MoveRotation(Quaternion.LookRotation(Vector3.ProjectOnPlane(gatherInfoAgent.transform.position - PlayerLocomotionManager.Instance.playerController.transform.position, Vector3.up)));
        //MyTools.SetActive(gatherName.transform.parent.gameObject, false);
        MyTools.SetActive(gatherButton.gameObject, false);
    }

    public void CanGather(GatherInfoAgent gatherInfoAgent)
    {
        if(PlayerInfoManager.Instance.PlayerInfo.IsFighting) return;
        this.gatherInfoAgent = gatherInfoAgent;
        gatherName.text = gatherInfoAgent.Name;
        canGather = true;
        MyTools.SetActive(gatherName.transform.parent.gameObject, true);
        MyTools.SetActive(gatherButton.gameObject, true);
    }

    public void CantGather()
    {
        if (PickUpManager.Instance.dropItemListAgent && gatherInfoAgent && gatherInfoAgent.dropItemListAgent && PickUpManager.Instance.dropItemListAgent == gatherInfoAgent.dropItemListAgent)
            PickUpManager.Instance.CantPick();
        gatherInfoAgent = null;
        gatherName.text = string.Empty;
        canGather = false;
        MyTools.SetActive(gatherName.transform.parent.gameObject, false);
        MyTools.SetActive(gatherButton.gameObject, false);
    }
}
