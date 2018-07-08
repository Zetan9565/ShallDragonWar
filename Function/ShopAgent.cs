using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEnums;
public class ShopInfo
{
    public string ID;
    public List<GoodsInfo> Goods;
}
namespace MyEnums
{
    public enum ShopType
    {
        IronShop,
        //ArmorShop,
        PropsShop,
        MaterialShop,
        MedicineShop,
        //ToolShop,
        GroceryShop
    }
}

[RequireComponent(typeof(BoxCollider))]
public class ShopAgent : MonoBehaviour {

    [SerializeField]
    bool active = true;
    public bool Active { get { return active; } set { active = value; } }
    public string ID;
    public ShopType ShopType;
    [HideInInspector]
    public string TypeName;
    public string shopName;
    //public int Capacity;
    [Tooltip("格式：物品ID,最大数量,1(表示可售罄)0(反之),当前数量")]
    public string[] goodsInput;
    [Tooltip("格式：物品ID,制造费用")]
    public string[] makingsInput;
    public List<GoodsInfo> Goods;
    public List<MakingInfo> Makings;

    public void Init()
    {
        ////WeaponItem weapon = new WeaponItem("WP0132", "铁戟", "力大之人方可随意舞弄的长杆武器", "Fangtian", 99, 50.0f, 500, 0, false, true, MyEnums.WeaponType.Halberd, 250,null);
        //WeaponItem weapon = DataBase.Self.GetWeaponItem("WP001");
        //MaterialItem material = new MaterialItem("MA0444", "止血草", "野外常见的药草", "Grass", 99, 0.01f, 35, 100, true, false);
        //MaterialItem iron = new MaterialItem("MA0333", "铁锭", "", "Iron", 99, 10.0f, 100, 50, true, false);
        //MaterialItem tan = new MaterialItem("MA0222", "炭块", "", "Tan", 99, 10.0f, 20, 10, true, false);
        //MaterialItem book = new MaterialItem("MA0111", "制作图纸", "", "Book", 99, 1.0f, 150, 75, true, false);
        //MedicineItem medicine = new MedicineItem("ME0445", "止血药", "药铺的抢手货", "Medicine", 15, 0.05f, 180, 100, true, true, 100, 100, 110);
        //medicine.AddMaterial(new MaterialInfo(material, 5));
        //weapon.AddMaterial(new MaterialInfo(iron, 3));
        //weapon.AddMaterial(new MaterialInfo(tan, 2));
        //weapon.AddMaterial(new MaterialInfo(book, 1));

        //GoodsInfo goods1 = new GoodsInfo(weapon, 1, false, 1);
        //GoodsInfo goods2 = new GoodsInfo(material, 50, true, 50);
        //GoodsInfo goods3 = new GoodsInfo(medicine, 20, true, 20);
        //GoodsInfo goods4 = new GoodsInfo(iron, 50, true, 50);
        //GoodsInfo goods5 = new GoodsInfo(tan, 50, true, 50);
        //GoodsInfo goods6 = new GoodsInfo(book, 5, true, 5);
        switch (ShopType)
        {
            case ShopType.IronShop:
                TypeName = "铁匠铺";
                break;
            /*case ShopType.ArmorShop:
                TypeName = "防具铺";
                break;*/
            case ShopType.PropsShop:
                TypeName = "珠宝商";
                break;
            case ShopType.MaterialShop:
                TypeName = "材料商";
                break;
            case ShopType.MedicineShop:
                TypeName = "药材铺";
                break;
            /*case ShopType.ToolShop:
                TypeName = "工具商";
                break;*/
            case ShopType.GroceryShop:
                TypeName = "杂货铺";
                break;
        }
        Goods = new List<GoodsInfo>();
        Makings = new List<MakingInfo>();

        GetGoods();
        GetMaking();
        StartCoroutine(ReplenishAllPro(300, 600, 5, 10));
    }

    IEnumerator ReplenishAllPro(float min_time, float max_time, int min_rep, int max_rep)
    {
        yield return new WaitForSeconds(Random.Range(min_time, max_time));
        foreach (GoodsInfo goods in Goods)
            goods.Replenish(Random.Range(min_rep, max_rep));
        StartCoroutine(ReplenishAllPro(min_time, max_time, min_rep, max_rep));
    }

    void GetGoods()
    {
        try
        {
            foreach (string info in goodsInput)
            {
                string[] goods = info.Split(',');
                ItemBase item = DataBase.Instance.GetItem(goods[0]);
                if (item != null)
                {
                    int mnum = 0;
                    int.TryParse(goods[1], out mnum);
                    int soa = 0;
                    int.TryParse(goods[2], out soa);
                    int snum = 0;
                    int.TryParse(goods[3], out snum);
                    Goods.Add(new GoodsInfo(item, mnum, soa == 0 ? false : true, snum));
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    void GetMaking()
    {
        try
        {
            foreach (string info in makingsInput)
            {
                string[] making = info.Split(',');
                ItemBase item = DataBase.Instance.GetItem(making[0]);
                if (item != null)
                {
                    int num = 0;
                    int.TryParse(making[1], out num);
                    Makings.Add(new MakingInfo(item, num));
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /*public void SellGoods(GoodsInfo goods, int sell_num, BagInfo bag)
    {
        GoodsInfo find = Goods.Find(g => g == goods);
        if (find == null) throw new System.Exception("不存在该商品");
        find.OnSell(sell_num, bag);
    }*/

    /// <summary>
    /// 单个补货
    /// </summary>
    /// <param name="goods">货物信息</param>
    /// <param name="rep_num">补充数量</param>
    public void Replenish(GoodsInfo goods, int rep_num)
    {
        GoodsInfo find = Goods.Find(g => g == goods);
        if (find == null) throw new System.Exception("不存在该商品");
        find.Replenish(rep_num);
    }

    /// <summary>
    /// 全部补货
    /// </summary>
    /// <param name="rep_num">每个货物的补充数量</param>
    public void ReplenishAll(int rep_num)
    {
        foreach (GoodsInfo goods in Goods)
            Replenish(goods, rep_num);
    }

    /// <summary>
    /// 全部以单个50%的概率补货
    /// </summary>
    /// <param name="rep_num">每个货物的补充数量</param>
    public void RepAllProbably(int rep_num)
    {
        foreach (GoodsInfo goods in Goods)
            if (goods.NumForSell <= goods.MaxNum)
                if (MyTools.Probability(50)) Replenish(goods, rep_num);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Active) return;
        if (other.tag == "Player")
            if (!ShopManager.Instance.shopAgent)
            {
                ShopManager.Instance.CanShop(this);
            }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Active) return;
        if (other.tag == "Player")
            if (!ShopManager.Instance.shopAgent && other.enabled)
            {
                ShopManager.Instance.CanShop(this);
            }
            else if (!other.enabled)
            {
                if (ShopManager.Instance.shopAgent && ShopManager.Instance.shopAgent == this)
                    ShopManager.Instance.CantShop();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            if (ShopManager.Instance.shopAgent && ShopManager.Instance.shopAgent == this)
                ShopManager.Instance.CantShop();
    }

    public ShopInfo ConvertToInfo()
    {
        return new ShopInfo()
        {
            ID = ID,
            Goods = Goods,
        };
    }

    public void LoadFromInfo(ShopInfo info)
    {
        if (info.Goods == null) return;
        foreach(GoodsInfo gi in info.Goods)
        {
            GoodsInfo tg = Goods.Find(g => g.Item.ID == gi.Item.ID);
            if (tg != null) tg.NumForSell = gi.NumForSell; 
        }
    }
}