using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BagManager : MonoBehaviour
{
    public static BagManager Instance;

    public GameObject UI;

    public BagInfo bagInfo;
    [Space]
    public GameObject BagGrid;
    public List<GameObject> bagCells;
    public List<ItemAgent> itemAgents;
    public Text Money;
    public Text Weight;
    public Text Size;
    public GameObject bagCellPrefab;
    public GameObject itemCellPrefab;
    bool isInit;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    public void Init()
    {
        isInit = true;
        bagInfo = PlayerInfoManager.Instance.PlayerInfo.bag;
        bagCells = new List<GameObject>();
        itemAgents = new List<ItemAgent>();
        LoadFromBagInfo();
        CloseUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit) return;
        CleanEmptyItemCells();
        MoneyWeightAndSize();
        if (Input.GetButtonDown("BagWindow"))
        {
            if (!UI.activeSelf) OpenUI();
            else CloseUI();
        }
    }

    void MoneyWeightAndSize()
    {
        if (!isInit) return;
        if (bagInfo == null) return;
        Money.text = bagInfo.Money+"文";
        Size.text = bagInfo.Current_Size + "/" + bagInfo.MaxSize + "空间";
        string CurrentWeight = bagInfo.IsMaxWeight ? "<color=red>" + bagInfo.Current_Weight.ToString("F2") + "</color>" 
            : bagInfo.IsOverweight ? "<color=orange>" + bagInfo.Current_Weight.ToString("F2") + "</color>" 
                : bagInfo.Current_Weight.ToString("F2");
        Weight.text = CurrentWeight + "/" + bagInfo.MaxWeight.ToString("F2") + "斤";
    }

    void SetBagCells()
    {
        if (!isInit) return;
        if (bagInfo == null) return;
        GameObject temp;
        for (int i = 0; i < bagInfo.MaxSize; i++)
        {
            temp = Instantiate(bagCellPrefab, BagGrid.transform) as GameObject;
            bagCells.Add(temp);
        }
    }

    public void GetItem(ItemBase item, int get_num)
    {
        if (!isInit) return;
        if (bagInfo == null || item == null || get_num <= 0) return;
        int finallyGet = 0;
        if (item.StackAble)
            finallyGet = bagInfo.itemList.Find(i => i.ItemID == item.ID) == null ?
                0 : bagInfo.itemList.Find(i => i.ItemID == item.ID).Quantity;
        List<ItemInfo> itemsBefore = bagInfo.itemList.FindAll(i => i.ItemID == item.ID);
        try
        {
            bagInfo.GetItem(item, get_num);
        }
        catch(System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
            return;
        }
        List<ItemInfo> itemsAfter = bagInfo.itemList.FindAll(i => i.ItemID == item.ID);
        List<ItemInfo> difference = new List<ItemInfo>();
        if (itemsBefore.Count > 0)
        {
            foreach (ItemInfo info in itemsAfter)
                if (itemsBefore.Find(i => i.Item == info.Item) == null) difference.Add(info);
        }
        else difference = itemsAfter;
        if (item.StackAble)
            finallyGet = bagInfo.itemList.Find(i => i.ItemID == item.ID) == null ?
                0 : bagInfo.itemList.Find(i => i.ItemID == item.ID).Quantity
                - finallyGet;
        else
            finallyGet = difference.Count;
        if(finallyGet > 0) NotificationManager.Instance.NewNotification("获得了" + finallyGet + "个<color=orange>" + item.Name + "</color>");
        if (item.StackAble && itemAgents.Exists(ic => ic.GetComponent<ItemAgent>().itemInfo.ItemID == item.ID))
        {
            return;
        }
        if (difference.Count > 0)
        {
            foreach (ItemInfo info in difference)
            {
                for (int i = 0; i < bagCells.Count; i++)
                {
                    if (bagCells[i].transform.childCount <= 0)
                    {
                        ItemAgent tempCell = (Instantiate(itemCellPrefab, bagCells[i].transform) as GameObject).GetComponent<ItemAgent>();
                        tempCell.itemInfo = info;
                        tempCell.isStored = false;
                        itemAgents.Add(tempCell);
                        break;
                    }
                }
            }
        }
    }

    public void Refresh()
    {
        if (!isInit) return;
        LoadFromBagInfo();
    }

    void Clear()
    {
        if (!isInit) return;
        foreach (ItemAgent cell in itemAgents)
            Destroy(cell.gameObject);
        itemAgents.Clear();
        foreach (GameObject cell in bagCells)
            Destroy(cell);
        bagCells.Clear();
    }

    void CleanEmptyItemCells()
    {
        if (!isInit) return;
        itemAgents.RemoveAll(i => !i);
    }

    public void LoadFromBagInfo()
    {
        if (!isInit) return;
        bagInfo = PlayerInfoManager.Instance.PlayerInfo.bag;
        if (bagInfo == null) return;
        Clear();
        SetBagCells();
        //Debug.Log("当前背包中物品数量" + PlayerInfoManager.Self.playerInfo.bag.itemList.Count);
        //Debug.Log(PlayerInfoManager.Self.playerInfo.bag.itemList.Find(i => i.Item == item) != null ? PlayerInfoManager.Self.playerInfo.bag.itemList.Find(i => i.Item == item).Quantity.ToString() : string.Empty);
        foreach (ItemInfo item in bagInfo.itemList)
            for (int i = 0; i < bagCells.Count; i++)
            {
                if (bagCells[i].transform.childCount <= 0)
                {
                    if (item != null && item.Quantity > 0)
                    {
                        ItemAgent tempCell = (Instantiate(itemCellPrefab, bagCells[i].transform) as GameObject).GetComponent<ItemAgent>();
                        tempCell.itemInfo = item;
                        tempCell.isStored = false;
                        itemAgents.Add(tempCell);
                    }
                    break;
                }
            }
    }

    public void Sort()
    {
        if (!isInit) return;
        if (itemAgents == null || bagCells == null) return;
        bagInfo.Sort();
        LoadFromBagInfo();
        ItemTipsManager.Instance.CloseUI();
    }

    public void OpenUI()
    {
        MyTools.SetActive(UI, true);
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
    }
}
