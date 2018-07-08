using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;

[RequireComponent(typeof(SimpleSQLManager))]
public class DataBase : MonoBehaviour {
    public static DataBase Instance;

    public SimpleSQLManager dbManager;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        Instance = this;
        dbManager = GetComponent<SimpleSQLManager>();
        DontDestroyOnLoad(gameObject);
    }

    #region 物品信息读取
    public ItemBase GetItem(string id)
    {
        if (id.Contains("WP")) return GetWeaponItem(id);
        else if (id.Contains("AR")) return GetArmorItem(id);
        else if (id.Contains("JE")) return GetJewelryItem(id);
        else if (id.Contains("ME")) return GetMedicineItem(id);
        else if (id.Contains("MA")) return GetMaterialItem(id);
        else return GetOtherItem(id);
    }

    public WeaponItem GetWeaponItem(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt =
                dbManager.QueryGeneric(
                    "SELECT " + "*" +
                    "FROM " +
                    "WeaponItem " +
                    "WHERE " + "ID LIKE " + ID);
            //Debug.Log("\"" + id + "\"" + dt.rows.Count);
            SuitEffectInfo setEffectInfo;
            if (dt.rows[0]["Suit"] != null) setEffectInfo = GetSuitEffectInfo(dt.rows[0]["Suit"].ToString());
            else setEffectInfo = null;
            WeaponItem weapon = new WeaponItem(dt.rows[0]["ID"].ToString(), dt.rows[0]["Name"].ToString(), dt.rows[0]["Description"].ToString(), dt.rows[0]["Icon"].ToString(), (int)dt.rows[0]["MaxCount"],
                float.Parse(dt.rows[0]["Weight"].ToString()), (int)dt.rows[0]["BuyPrice"], (int)dt.rows[0]["SellPrice"], (int)dt.rows[0]["SellAble"] == 1, (int)dt.rows[0]["Usable"] == 1,
                (MyEnums.WeaponType)dt.rows[0]["WeaponType"], (int)dt.rows[0]["ATK"], setEffectInfo);
            if (dt.rows[0]["Materials"] != null) weapon.MaterialsListInput = dt.rows[0]["Materials"].ToString();
            //Debug.Log("\"" + id + "\"" + "dasdasd");
            weapon.SetMaterials(this);
            return weapon;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }

    public ArmorItem GetArmorItem(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt = dbManager.QueryGeneric(
                                                    "SELECT " + "*" +
                                                    "FROM " +
                                                        "ArmorItem " +
                                                        "WHERE " + "ID LIKE " + ID
                                                    );
            SuitEffectInfo setEffectInfo;
            if (dt.rows[0]["Suit"] != null) setEffectInfo = GetSuitEffectInfo(dt.rows[0]["Suit"].ToString());
            else setEffectInfo = null;
            ArmorItem armor = new ArmorItem(dt.rows[0]["ID"].ToString(), dt.rows[0]["Name"].ToString(), dt.rows[0]["Description"].ToString(), dt.rows[0]["Icon"].ToString(), (int)dt.rows[0]["MaxCount"],
                float.Parse(dt.rows[0]["Weight"].ToString()), (int)dt.rows[0]["BuyPrice"], (int)dt.rows[0]["SellPrice"], (int)dt.rows[0]["SellAble"] == 1, (int)dt.rows[0]["Usable"] == 1,
                (MyEnums.ArmorType)dt.rows[0]["ArmorType"], (int)dt.rows[0]["DEF"], setEffectInfo);
            if (dt.rows[0]["Materials"] != null) armor.MaterialsListInput = dt.rows[0]["Materials"].ToString();
            armor.SetMaterials(this);
            return armor;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }

    public JewelryItem GetJewelryItem(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt = dbManager.QueryGeneric(
                                                    "SELECT " + "*" +
                                                    "FROM " +
                                                        "JewelryItem " +
                                                        "WHERE " + "ID LIKE " + ID
                                                    );
            PowerUps ups = new PowerUps(dt.rows[0]["PowerUps"].ToString());
            SuitEffectInfo setEffectInfo;
            if (dt.rows[0]["Suit"] != null) setEffectInfo = GetSuitEffectInfo(dt.rows[0]["Suit"].ToString());
            else setEffectInfo = null;
            JewelryItem jewelry = new JewelryItem(dt.rows[0]["ID"].ToString(), dt.rows[0]["Name"].ToString(), dt.rows[0]["Description"].ToString(), dt.rows[0]["Icon"].ToString(), (int)dt.rows[0]["MaxCount"],
                float.Parse(dt.rows[0]["Weight"].ToString()), (int)dt.rows[0]["BuyPrice"], (int)dt.rows[0]["SellPrice"], (int)dt.rows[0]["SellAble"] == 1, (int)dt.rows[0]["Usable"] == 1,
                (MyEnums.JewelryType)dt.rows[0]["JewelryType"], ups, setEffectInfo);
            if (dt.rows[0]["Materials"] != null) jewelry.MaterialsListInput = dt.rows[0]["Materials"].ToString();
            jewelry.SetMaterials(this);
            return jewelry;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }

    public MaterialItem GetMaterialItem(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt = dbManager.QueryGeneric(
                                                    "SELECT " + "*" +
                                                    "FROM " +
                                                        "MaterialItem " +
                                                        "WHERE " + "ID LIKE " + ID
                                                    );
            MaterialItem material = new MaterialItem(dt.rows[0]["ID"].ToString(), dt.rows[0]["Name"].ToString(), dt.rows[0]["Description"].ToString(), dt.rows[0]["Icon"].ToString(), (int)dt.rows[0]["MaxCount"],
                float.Parse(dt.rows[0]["Weight"].ToString()), (int)dt.rows[0]["BuyPrice"], (int)dt.rows[0]["SellPrice"], (int)dt.rows[0]["SellAble"] == 1, (int)dt.rows[0]["Usable"] == 1);
            if (dt.rows[0]["Materials"] != null) material.MaterialsListInput = dt.rows[0]["Materials"].ToString();
            material.SetMaterials(this);
            return material;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }

    public MedicineItem GetMedicineItem(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt = dbManager.QueryGeneric(
                                                    "SELECT " + "*" +
                                                    "FROM " +
                                                        "MedicineItem " +
                                                        "WHERE " + "ID LIKE " + ID
                                                    );
            MedicineItem medicine = new MedicineItem(dt.rows[0]["ID"].ToString(), dt.rows[0]["Name"].ToString(), dt.rows[0]["Description"].ToString(), dt.rows[0]["Icon"].ToString(), (int)dt.rows[0]["MaxCount"],
                float.Parse(dt.rows[0]["Weight"].ToString()), (int)dt.rows[0]["BuyPrice"], (int)dt.rows[0]["SellPrice"], (int)dt.rows[0]["SellAble"] == 1, (int)dt.rows[0]["Usable"] == 1,
                dt.rows[0]["HP_Rec"] == null ? 0 : (int)dt.rows[0]["HP_Rec"], dt.rows[0]["MP_Rec"] == null ? 0 : (int)dt.rows[0]["MP_Rec"], dt.rows[0]["Endurance_Rec"] == null ? 0 : (int)dt.rows[0]["Endurance_Rec"]);
            if (dt.rows[0]["Materials"] != null) medicine.MaterialsListInput = dt.rows[0]["Materials"].ToString();
            medicine.SetMaterials(this);
            return medicine;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }

    public ItemBase GetOtherItem(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt = dbManager.QueryGeneric(
                                                    "SELECT " + "*" +
                                                    "FROM " +
                                                        "OtherItem " +
                                                        "WHERE " + "ID LIKE " + ID
                                                    );
            ItemBase item = new ItemBase(dt.rows[0]["ID"].ToString(), dt.rows[0]["Name"].ToString(), dt.rows[0]["Description"].ToString(), dt.rows[0]["Icon"].ToString(), (int)dt.rows[0]["MaxCount"],
                float.Parse(dt.rows[0]["Weight"].ToString()), (int)dt.rows[0]["BuyPrice"], (int)dt.rows[0]["SellPrice"], (int)dt.rows[0]["SellAble"] == 1, (int)dt.rows[0]["Usable"] == 1);
            if (dt.rows[0]["Materials"] != null) item.MaterialsListInput = dt.rows[0]["Materials"].ToString();
            item.SetMaterials(this);
            return item;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }
    #endregion

    public SuitEffectInfo GetSuitEffectInfo(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id)) return null;
            string ID = "'" + id + "'";
            SimpleSQL.SimpleDataTable dt = dbManager.QueryGeneric(
                                                    "SELECT " + "*" +
                                                    "FROM " +
                                                        "SuitEffect " +
                                                        "WHERE " + "SuitID LIKE " + ID
                                                    );
            PowerUps ups1 = new PowerUps(dt.rows[0]["PowerUps1"].ToString());
            PowerUps ups2 = new PowerUps(dt.rows[0]["PowerUps2"].ToString());
            SuitEffectInfo setEffectInfo = new SuitEffectInfo(dt.rows[0]["SuitID"].ToString(), dt.rows[0]["SuitName"].ToString(), ups1, ups2, (int)dt.rows[0]["Suit1Num"], (int)dt.rows[0]["Suit2Num"]);
            return setEffectInfo;
        }
        catch (System.Exception ex)
        {
            Debug.Log("\"" + id + "\"" + ex.Message);
            return null;
        }
    }
}
