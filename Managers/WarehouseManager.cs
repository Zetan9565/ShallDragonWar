using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WarehouseManager : MonoBehaviour
{
    public static WarehouseManager Instance;

    public GameObject UI;
    [Space]
    public Button storeMoney;
    public Button withdrMoney;
    public GameObject HouseGrid;
    public List<GameObject> HouseCells;
    public List<ItemAgent> ItemCells;
    public Text Money;
    public Text Size;
    public Button warehouseBtn;
    public GameObject houseCellPrefab;
    public GameObject itemCellPrefab;
    public bool isStoring;

    bool isInit;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    public void Init()
    {
        isInit = true;
        HouseCells = new List<GameObject>();
        ItemCells = new List<ItemAgent>();
        LoadFromWarehouseInfo();
        storeMoney.onClick.AddListener(OnStoreMoneyClick);
        withdrMoney.onClick.AddListener(OnWithDrawClick);
        warehouseBtn.onClick.AddListener(OpenUI);
        MyTools.SetActive(warehouseBtn.gameObject, false);
        CloseUI();
    }

    // Update is called once per frame
    void Update()
    {
        CleanEmptyItemCells();
        MoneyAndSize();
    }

    public void MoneyAndSize()
    {
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.warehouseInfo == null) return;
        Money.text = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.Money + "文";
        Size.text = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.Current_Size + "/" + PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.MaxSize + "空间";
    }

    public void SetHouseCells()
    {
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.warehouseInfo == null) return;
        GameObject temp;
        for (int i = 0; i < PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.MaxSize; i++)
        {
            temp = Instantiate(houseCellPrefab, HouseGrid.transform) as GameObject;
            HouseCells.Add(temp);
        }
    }

    public void StoreItem(ItemBase item, int store_num)
    {
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.warehouseInfo == null || item == null || store_num <= 0) return;
        int finallyGet = 0;
        if (item.StackAble)
            finallyGet = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList.Find(i => i.ItemID == item.ID) == null ?
                0 : PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList.Find(i => i.ItemID == item.ID).Quantity;
        List<ItemInfo> itemsBefore = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList.FindAll(i => i.ItemID == item.ID);
        try
        {
            PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.StoreItem(item, BagManager.Instance.bagInfo, store_num);
        }
        catch (System.Exception ex)
        {
            NotificationManager.Instance.NewNotification(ex.Message);
            return;
        }
        List<ItemInfo> itemsAfter = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList.FindAll(i => i.ItemID == item.ID);
        List<ItemInfo> difference = new List<ItemInfo>();
        if (itemsBefore.Count > 0)
        {
            foreach (ItemInfo info in itemsAfter)
                if (itemsBefore.Find(i => i.Item == info.Item) == null) difference.Add(info);
        }
        else difference = itemsAfter;
        if (item.StackAble)
            finallyGet = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList.Find(i => i.ItemID == item.ID) == null ?
                0 : PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList.Find(i => i.ItemID == item.ID).Quantity
                - finallyGet;
        else
            finallyGet = difference.Count;
        if (finallyGet > 0) NotificationManager.Instance.NewNotification("存储了" + finallyGet + "个<color=orange>" + item.Name + "</color>");
        if (item.StackAble && ItemCells.Exists(ic => ic.GetComponent<ItemAgent>().itemInfo.ItemID == item.ID))
        {
            return;
        }
        if (difference.Count > 0)
        {
            foreach (ItemInfo info in difference)
            {
                for (int i = 0; i < HouseCells.Count; i++)
                {
                    if (HouseCells[i].transform.childCount <= 0)
                    {
                        ItemAgent tempCell = (Instantiate(itemCellPrefab, HouseCells[i].transform) as GameObject).GetComponent<ItemAgent>();
                        tempCell.itemInfo = info;
                        tempCell.isStored = true;
                        ItemCells.Add(tempCell);
                        break;
                    }
                }
            }
        }
    }

    public void OnStoreMoneyClick()
    {
        if (!isInit) return;
        MoneyConfirmManager.Instance.MaxNumber = BagManager.Instance.bagInfo.Money;
        MoneyConfirmManager.Instance.YesButton.onClick.AddListener(StoreMoney);
        MoneyConfirmManager.Instance.OpenUI();
    }

    public void StoreMoney()
    {
        if (!isInit) return;
        //Debug.Log("cunqian");
        MoneyConfirmManager.Instance.CloseUI();
        int lastMoney = BagManager.Instance.bagInfo.Money;
        BagManager.Instance.bagInfo.LoseMoney(MoneyConfirmManager.Instance.ItemNumber);
        PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.StoreMoney(lastMoney - BagManager.Instance.bagInfo.Money);
    }

    public void OnWithDrawClick()
    {
        if (!isInit) return;
        MoneyConfirmManager.Instance.MaxNumber = PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.Money;
        MoneyConfirmManager.Instance.YesButton.onClick.AddListener(WithdrawMoney);
        MoneyConfirmManager.Instance.OpenUI();
    }

    public void WithdrawMoney()
    {
        if (!isInit) return;
        //Debug.Log("quqian");
        MoneyConfirmManager.Instance.CloseUI();
        int lastMoney = BagManager.Instance.bagInfo.Money;
        BagManager.Instance.bagInfo.GetMoney(MoneyConfirmManager.Instance.ItemNumber);
        PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.WithdrawMoney(BagManager.Instance.bagInfo.Money - lastMoney);
    }

    public void Refresh()
    {
        if (!isInit) return;
        LoadFromWarehouseInfo();
    }

    public void Clear()
    {
        foreach (ItemAgent cell in ItemCells)
            Destroy(cell.gameObject);
        ItemCells.Clear();
        foreach (GameObject cell in HouseCells)
            Destroy(cell);
        HouseCells.Clear();
    }

    public void LoadFromWarehouseInfo()
    {
        if (!isInit) return;
        if (PlayerInfoManager.Instance.PlayerInfo.warehouseInfo == null) return;
        Clear();
        SetHouseCells();
        //Debug.Log("当前背包中物品数量" + PlayerInfoManager.Self.playerInfo.bag.itemList.Count);
        //Debug.Log(PlayerInfoManager.Self.playerInfo.bag.itemList.Find(i => i.Item == item) != null ? PlayerInfoManager.Self.playerInfo.bag.itemList.Find(i => i.Item == item).Quantity.ToString() : string.Empty);
        foreach (ItemInfo item in PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.itemList)
            for (int i = 0; i < HouseCells.Count; i++)
            {
                if (HouseCells[i].transform.childCount <= 0)
                {
                    if (item != null && item.Quantity > 0)
                    {
                        ItemAgent tempCell = (Instantiate(itemCellPrefab, HouseCells[i].transform) as GameObject).GetComponent<ItemAgent>();
                        tempCell.itemInfo = item;
                        tempCell.isStored = true;
                        ItemCells.Add(tempCell);
                    }
                    break;
                }
            }
    }

    public void Sort()
    {
        if (!isInit) return;
        if (ItemCells == null || HouseCells == null) return;
        PlayerInfoManager.Instance.PlayerInfo.warehouseInfo.Sort();
        LoadFromWarehouseInfo();
        ItemTipsManager.Instance.CloseUI();
    }

    public void CleanEmptyItemCells()
    {
        ItemCells.RemoveAll(i => i == null);
    }

    public void CanStore()
    {
        MyTools.SetActive(warehouseBtn.gameObject, true);
    }

    public void CantStore()
    {
        MyTools.SetActive(warehouseBtn.gameObject, false);
        if (ItemTipsManager.Instance.UI.activeSelf && ItemCells.Exists(i => i = ItemTipsManager.Instance.itemAgent))
        {
            ItemTipsManager.Instance.CloseUI();
        }
        ItemConfirmManager.Instance.CloseUI();
        CloseUI();
    }

    public void OpenUI()
    {
        MyTools.SetActive(UI, true);
        isStoring = true;
        LoadFromWarehouseInfo();
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
        isStoring = false;
    }

    /*public void SaveToFile(string path, string key = "", bool encrypt = false)
    {
        PlayerInfoManager.Self.PlayerInfo.warehouseInfo.Save(path, key, encrypt);
    }

    public void LoadFromFile(string path, string key = "", bool dencrypt = false)
    {
        PlayerInfoManager.Self.PlayerInfo.warehouseInfo.Load(path, key, dencrypt);
    }*/
}
