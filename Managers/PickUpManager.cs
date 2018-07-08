using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PickUpManager : MonoBehaviour {

    public static PickUpManager Instance;

    public GameObject UI;
    [Space]
    public Button pickUpButton;
    public Button pickUpAllButton;
    public DropItemListAgent dropItemListAgent;
    public Transform[] gridCells;
    public GameObject itemCellPrefab;
    public List<DropItemAgent> itemCells;
    bool canPick;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        itemCells = new List<DropItemAgent>();
        pickUpAllButton.onClick.AddListener(OnPickUpAll);
        pickUpButton.onClick.AddListener(OnPickUpClick);
        CloseUI();
	}

    private void Update()
    {
        if (!dropItemListAgent) CloseUI();
        if (Input.GetButtonDown("PickUp"))
        {
            if (canPick && !UI.activeSelf) OnPickUpClick();
            else if(UI.activeSelf) OnPickUpAll();
        }
    }

    public void LoadDropItems()
    {
        Clear();
        if (dropItemListAgent)
        {
            foreach (DropItemInfo info in dropItemListAgent.dropItemList)
            {
                foreach (Transform grid in gridCells)
                    if (grid.childCount == 0)
                    {
                        GameObject itemcell = Instantiate(itemCellPrefab, grid) as GameObject;
                        DropItemAgent drop = itemcell.GetComponent<DropItemAgent>();
                        drop.dropItemInfo = info;
                        drop.parentAgent = dropItemListAgent;
                        itemCells.Add(drop);
                        break;
                    }
            }
        }
    }

    public void CanPick(DropItemListAgent dropItemListAgent)
    {
        MyTools.SetActive(pickUpButton.gameObject, true);
        this.dropItemListAgent = dropItemListAgent;
        canPick = true;
    }

    public void CantPick()
    {
        //Debug.Log("Cant");
        MyTools.SetActive(pickUpButton.gameObject, false);
        CloseUI();
        if (ItemTipsManager.Instance.UI.activeSelf && itemCells.Exists(d => d == ItemTipsManager.Instance.dropItemAgent))
        {
            ItemTipsManager.Instance.CloseUI();
        }
        ItemConfirmManager.Instance.CloseUI();
        dropItemListAgent = null;
        canPick = false;
        Clear();
    }

    void OnPickUpClick()
    {
        //Debug.Log("OnPickUpClick");
        LoadDropItems();
        if (itemCells.Count <= 0) CloseUI();
        else OpenUI();
        MyTools.SetActive(pickUpButton.gameObject, false);
    }

    void OnPickUpAll()
    {
        foreach(DropItemAgent agent in itemCells)
        {
            //agent.PickUpAll();
            try
            {
                BagManager.Instance.GetItem(agent.dropItemInfo.Item, agent.dropItemInfo.TryPickUpAll(BagManager.Instance.bagInfo));
                if (agent.dropItemInfo.Left <= 0)
                {
                    agent.parentAgent.dropItemList.RemoveAll(i => i == agent.dropItemInfo);
                    Destroy(agent.gameObject);
                }
            }
            catch (System.Exception ex)
            {
                NotificationManager.Instance.NewNotification(ex.Message);
            }
        }
        itemCells.RemoveAll(a => !a);
        if (itemCells.Count <= 0)
        {
            dropItemListAgent = null;
            CloseUI();
        }
        LoadDropItems();
        if (itemCells.Count <= 0) CloseUI();
    }

    void Clear()
    {
        foreach (Transform gc in gridCells)
            gc.DetachChildren();
        foreach (DropItemAgent agent in itemCells)
        {
            if(agent) Destroy(agent.gameObject);
        }
        itemCells.Clear();
    }

    public void OpenUI()
    {
        MyTools.SetActive(UI, true);
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
        MyTools.SetActive(pickUpButton.gameObject, false);
    }
}
