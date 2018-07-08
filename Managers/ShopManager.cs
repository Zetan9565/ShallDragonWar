using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyEnums;

[DisallowMultipleComponent]
public class ShopManager : MonoBehaviour {
    public static ShopManager Instance;
    public readonly static string DataName = "/ShopsData.zetan";
    public List<ShopAgent> ShopAgents;
    public GameObject UI;
    [Space]
    public ShopAgent shopAgent;
    public Button shopButton;
    public Text shopName;
    [Space]
    public GameObject gridCellPrefab;
    [Header("商品栏")]
    public GameObject GoodsGrid;
    public Scrollbar GoodsGridScrollbar;
    public List<GameObject> GoodsGridCells;
    public List<GameObject> GoodsCells;
    public GameObject goodsCellPrefab;
    [Header("出售栏")]
    public GameObject BuyerGrid;
    public Scrollbar BuyerGridScrollbar;
    public List<GameObject> BuyerGridCells;
    //public GameObject buyerGridCellPrefab;
    public List<GameObject> BuyerItemCells;
    public GameObject buyerItemCellPrefab;
    [Header("制作栏")]
    public GameObject MakingGrid;
    public Scrollbar MakingGridScrollbar;
    public List<GameObject> MakingGridCells;
    //public GameObject makingGirdCellPrefab;
    public List<GameObject> MakingCells;
    public GameObject makingCellPrefab;
    [Space]
    public Text Money;
    public Text ShopName;
    public Toggle buyPage;
    [HideInInspector]
    public bool PlayerSelling;
    [HideInInspector]
    public bool PlayerMaking;

    private void Awake()
    {
        Instance = this;
        ShopAgents = new List<ShopAgent>();
        foreach (ShopAgent s in FindObjectsOfType<ShopAgent>())
        {
            s.Init();
            ShopAgents.Add(s);
        }
    }

    private void Start()
    {
        shopAgent = null;
        GoodsGridCells = new List<GameObject>();
        BuyerGridCells = new List<GameObject>();
        MakingGridCells = new List<GameObject>();
        shopButton.onClick.AddListener(OnShopButtonClick);
        CloseUI();
    }
	
	// Update is called once per frame
	private void Update() {
        CleanEmptyCells();
        if(BagManager.Instance.bagInfo != null) Money.text = BagManager.Instance.bagInfo.Money + "文";
	}

    void LoadFromShopAgent()
    {
        if (!shopAgent) return;
        foreach (GameObject go in GoodsGridCells)
            Destroy(go);
        GoodsGridCells.Clear();
        foreach (GameObject go in GoodsCells)
            Destroy(go);
        GoodsCells.Clear();
        foreach (GameObject go in MakingGridCells)
            Destroy(go);
        MakingGridCells.Clear();
        foreach (GameObject go in MakingCells)
            Destroy(go);
        MakingCells.Clear();
        SetGridCells();
        SetGoodsCells();
        SetMakingGridCells();
        SetMakingCells();
        ShopName.text = shopAgent.TypeName;
    }

    void SetGridCells()
    {
        if (!shopAgent) return;
        for (int i = 0; i < shopAgent.goodsInput.Length; i++)
        {
            GameObject temp = Instantiate(gridCellPrefab, GoodsGrid.transform) as GameObject;
            GoodsGridCells.Add(temp);
        }
        GoodsGridScrollbar.value = 1;
    }

    void SetGoodsCells()
    {
        if (!shopAgent) return;
        foreach (GoodsInfo goods in shopAgent.Goods)
        {
            foreach (GameObject gridCell in GoodsGridCells)
                if (gridCell.transform.childCount <= 0)
                {
                    GameObject temp = Instantiate(goodsCellPrefab, gridCell.transform) as GameObject;
                    temp.GetComponent<GoodsAgent>().goodsInfo = goods;
                    //temp.GetComponent<GoodsAgent>().ShowInfo();
                    GoodsCells.Add(temp);
                    break;
                }
        }
    }

    void SetBuyerGridCells()
    {
        if (PlayerInfoManager.Instance.PlayerInfo == null) return;
        for (int i = 0; i < BagManager.Instance.bagInfo.itemList.FindAll(ii => ii.Item.SellAble == true).Count; i++)
        {
            GameObject temp = Instantiate(gridCellPrefab, BuyerGrid.transform) as GameObject;
            BuyerGridCells.Add(temp);
        }
        BuyerGridScrollbar.value = 1;
    }

    void SetBuyerItemCells()
    {
        if (!shopAgent) return;
        foreach (ItemInfo info in BagManager.Instance.bagInfo.itemList)
        {
            if (info.Item.SellAble)
            {
                foreach (GameObject gridCell in BuyerGridCells)
                    if (gridCell.transform.childCount <= 0)
                    {
                        GameObject temp = Instantiate(buyerItemCellPrefab, gridCell.transform) as GameObject;
                        temp.GetComponent<BuyerItemAgent>().itemInfo = info;
                        //temp.GetComponent<BuyerItemAgent>().ShowInfo();
                        BuyerItemCells.Add(temp);
                        break;
                    }
            }
        }
    }

    public void OnPlayerSell()
    {
        if (!shopAgent) return;
        foreach (GameObject go in BuyerItemCells)
            Destroy(go);
        BuyerItemCells.Clear();
        foreach (GameObject go in BuyerGridCells)
            Destroy(go);
        BuyerGridCells.Clear();
        SetBuyerGridCells();
        SetBuyerItemCells();
    }

    void SetMakingGridCells()
    {
        if (!shopAgent) return;
        for (int i = 0; i < shopAgent.makingsInput.Length; i++)
        {
            GameObject temp = Instantiate(gridCellPrefab, MakingGrid.transform) as GameObject;
            MakingGridCells.Add(temp);
        }
        MakingGridScrollbar.value = 1;
    }

    void SetMakingCells()
    {
        if (!shopAgent) return;
        foreach (MakingInfo making in shopAgent.Makings)
        {
            foreach (GameObject gridCell in MakingGridCells)
                if (gridCell.transform.childCount <= 0)
                {
                    GameObject temp = Instantiate(makingCellPrefab, gridCell.transform) as GameObject;
                    temp.GetComponent<MakingAgent>().makingInfo = making;
                    //temp.GetComponent<MakingAgent>().ShowInfo();
                    MakingCells.Add(temp);
                    break;
                }
        }
    }

    public void OnPlayerMaking()
    {
        if (!shopAgent) return;
        foreach (GameObject go in MakingGridCells)
            Destroy(go);
        MakingGridCells.Clear();
        foreach (GameObject go in MakingCells)
            Destroy(go);
        MakingCells.Clear();
        SetMakingGridCells();
        SetMakingCells();
    }

    void CleanEmptyCells()
    {
        GoodsCells.RemoveAll(g => g == null);
        BuyerItemCells.RemoveAll(b => b == null);
    }

    void OnShopButtonClick()
    {
        LoadFromShopAgent();
        MyTools.SetActive(shopButton.gameObject, false);
        OpenUI();
    }

    public void CanShop(ShopAgent shopAgent)
    {
        this.shopAgent = shopAgent;
        shopName.text = shopAgent.shopName;
        MyTools.SetActive(shopButton.gameObject, true);
    }

    public void CantShop()
    {
        MyTools.SetActive(shopButton.gameObject, false);
        shopAgent = null;
        shopName.text = string.Empty;
        if (ItemTipsManager.Instance.UI.activeSelf && (ItemTipsManager.Instance.goodsAgent || ItemTipsManager.Instance.makingAgent || ItemTipsManager.Instance.buyerItemAgent))
        {
            ItemTipsManager.Instance.CloseUI();
        }
        ItemConfirmManager.Instance.CloseUI();
        CloseUI();
    }

    public void OpenUI()
    {
        MyTools.SetActive(shopButton.gameObject, false);
        MyTools.SetActive(UI.gameObject, true);
    }

    public void CloseUI()
    {
        shopAgent = null;
        buyPage.group.SetAllTogglesOff();
        buyPage.isOn = true;
        buyPage.onValueChanged.Invoke(true);
        MyTools.SetActive(UI.gameObject, false);
        MyTools.SetActive(shopButton.gameObject, false);
    }

    public void SaveToFile(string path, string key = "", bool encrypt = false)
    {
        try
        {
            List<string> infos = new List<string>();
            foreach (ShopAgent sa in ShopAgents)
            {
                if (encrypt && key.Length == 32) infos.Add(Encryption.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(sa.ConvertToInfo()), key));
                else infos.Add(Newtonsoft.Json.JsonConvert.SerializeObject(sa.ConvertToInfo()));
            }
            System.IO.File.WriteAllLines(path + DataName, infos.ToArray(), System.Text.Encoding.UTF8);
        }
        catch { }
    }

    public void LoadFromFile(string path, string key = "", bool dencrypt = false)
    {
        try
        {
            string[] infos = System.IO.File.ReadAllLines(path + DataName, System.Text.Encoding.UTF8);
            List<ShopInfo> sinfos = new List<ShopInfo>();
            foreach (string info in infos)
            {
                if (dencrypt && key.Length == 32) sinfos.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ShopInfo>(Encryption.Dencrypt(info, key)));
                else sinfos.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ShopInfo>(info));
            }
            foreach (ShopInfo si in sinfos)
            {
                //Debug.Log(si.ID + si.Goods);
                ShopAgent sa = ShopAgents.Find(s => s.ID == si.ID);
                sa.LoadFromInfo(si);
            }
        }
        catch { }
    }
}
