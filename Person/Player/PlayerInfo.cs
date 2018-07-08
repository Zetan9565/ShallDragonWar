using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MyEnums;

public class PlayerInfo
{
    public CharacterInfo characterInfo;
    public int ID;
    public string CharacterName;
    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            if (value >= 0 && value <= 40) level = value;
            else if (value > 40) level = 40;
            else level = 0;
        }
    }

    #region 状态判断
    public bool IsAlive;
    public bool IsTired;
    public bool IsExhausted;
    //以下不存储
    public bool IsMounting;
    public bool IsFighting;
    public bool IsBlocking;
    public bool IsBlockBroken;
    public bool IsSwimming;
    public bool IsGathering;
    public bool IsSitting;
    public bool IsDown;
    public bool IsGiddy;
    //public bool IsFloat;
    public bool IsRigid;
    public bool SuperArmor;
    #endregion

    #region 五维属性
    public int Li;
    public int Ti;
    public int Qi;
    public int Ji;
    public int Min;
    #endregion

    #region 实时属性
    public int HP;
    private int current_HP;
    public int Current_HP
    {
        get
        {
            return current_HP;
        }
        set
        {
            if (value > HP) current_HP = HP;
            else if (value <= 0) { current_HP = 0; IsAlive = false; }
            else current_HP = value;
        }
    }

    public int MP;
    private int current_MP;
    public int Current_MP
    {
        get
        {
            return current_MP;
        }
        set
        {
            if (value > MP) current_MP = MP;
            else if (value <= 0) current_MP = 0;
            else current_MP = value;
        }
    }

    public int Endurance;
    private int current_Endurance;
    public int Current_Endurance
    {
        get
        {
            return current_Endurance;
        }
        set
        {
            if (value > Endurance) current_Endurance = Endurance;
            else if (value <= 0) current_Endurance = 0;
            else current_Endurance = value;
        }
    }

    public int BlockAmount;
    private int current_BlockAmount;
    public int Current_BlockAmount
    {
        get { return current_BlockAmount; }
        set
        {
            if (value > BlockAmount) current_BlockAmount = BlockAmount;
            else if (value < 0) current_BlockAmount = 0;
            else current_BlockAmount = value;
        }
    }

    public int NextExp;
    private int current_Exp;
    public int Current_Exp
    {
        get { return current_Exp; }
        set
        {
            if (current_Exp < 0) current_Exp = 0;
            else current_Exp = value;
        }
    }

    public int SkillPointOne;
    public int SkillPointTwo;

    #endregion

    public int Neili;

    public int ATK;
    public int DEF;

    public float Hit;
    public float Dodge;
    public float Crit;

    #region 状态抗性
    public float Res_Rigidity;
    public float Res_Repulsed;
    public float Res_Stuned;
    public float Res_Floated;
    public float Res_Blowed;
    public float Res_Falled;
    #endregion

    //public float MoveSpeed;
    [System.NonSerialized]
    public List<StatuInfo> status;
    //public List<BuffInfo> buffs;
    public Equipments equipments;
    public BagInfo bag;
    public WarehouseInfo warehouseInfo;

    public MountInfo mount;

    public PlayerInfo()
    {
        ID = 100000;
        CharacterName = "大侠";
        Level = 0;
        IsAlive = true;
        IsMounting = false;
        IsTired = false;
        IsExhausted = false;
        Li = 5;
        Ti = 5;
        Qi = 5;
        Ji = 5;
        Min = 5;
        HP = Ti * 100;
        Current_HP = HP;
        MP = Li * 20 + Ti * 30;
        Current_MP = MP;
        Endurance = Ti * 2 + Qi;
        Current_Endurance = Endurance;
        Neili = Qi;
        ATK = Li * 3;
        DEF = Qi * 3;
        Hit = Ji * 0.4f;
        Dodge = Min;
        Crit = Ji * 0.1f;
        Res_Blowed =
        Res_Floated =
        Res_Repulsed =
        Res_Rigidity =
        Res_Stuned =
        Res_Falled = 0.0f;
        status = new List<StatuInfo>();
        //buffs = new List<BuffInfo>();
        equipments = new Equipments();
        bag = new BagInfo(39, 150);
        warehouseInfo = new WarehouseInfo(300);
        mount = null;
        //MoveSpeed = 5.0f;
    }

    /// <summary>
    /// 设置角色信息
    /// </summary>
    /// <param name="character">要读入的角色信息</param>
    public void SetCharacter(CharacterInfo character)
    {
        if (characterInfo != null) return;
        characterInfo = character;
        ID = character.ID;
        CharacterName = character.Name;
    }

    #region 读写相关
    ///   <summary>   
    ///   从路径读取玩家信息   
    ///   </summary> 
    ///   <param name="path">已存在的文件的路径</param>
    public void LoadFromFile(string path, string key = "", bool dencrypt = false)
    {
        if (File.Exists(path + "/PlayerInfoData.zetan"))
        {
            if(dencrypt && key.Length == 32) CopyInfo(JsonConvert.DeserializeObject<PlayerInfo>(Encryption.Dencrypt(File.ReadAllText(path + "/PlayerInfoData.zetan"), key)) ?? this);
            else CopyInfo(JsonConvert.DeserializeObject<PlayerInfo>(File.ReadAllText(path + "/PlayerInfoData.zetan")) ?? this);
            bag.Load(path, key, dencrypt);
            warehouseInfo.Load(path, key, dencrypt);
        }
        else
            throw new System.Exception("存档不存在");
    }
    ///   <summary>   
    ///   将玩家信息存到路径   
    ///   </summary> 
    ///   <param name="path">保存的路径</param>
    public void SaveToFile(string path, string key = "", bool encrypt = false)
    {
        if (encrypt && key.Length == 32)
        {
            File.WriteAllText(path + "/PlayerInfoData.zetan", Encryption.Encrypt(
            JsonConvert.SerializeObject(this),key),Encoding.UTF8);
        }
        else
        {
            File.WriteAllText(path + "/PlayerInfoData.zetan", JsonConvert.SerializeObject(this));
        }
        bag.Save(path, key, encrypt);
        warehouseInfo.Save(path, key, encrypt);
    }

    public void CopyInfo(PlayerInfo source)
    {
        ID = source.ID;
        CharacterName = source.CharacterName;
        level = source.level;
        IsAlive = source.IsAlive;
        IsTired = source.IsTired;
        IsExhausted = source.IsExhausted;
        equipments = source.equipments;
        Li = source.Li;
        Ti = source.Ti;
        Qi = source.Qi;
        Ji = source.Ji;
        Min = source.Min;
        HP = source.HP;
        Current_HP = source.Current_HP;
        MP = source.MP;
        Current_MP = source.Current_MP;
        Endurance = source.Endurance;
        Current_Endurance = source.Current_Endurance;
        BlockAmount = source.BlockAmount;
        Current_BlockAmount = source.Current_BlockAmount;
        NextExp = source.NextExp;
        Current_Exp = source.Current_Exp;
        SkillPointOne = source.SkillPointOne;
        SkillPointTwo = source.SkillPointTwo;
        Neili = source.Neili;
        ATK = source.ATK;
        DEF = source.DEF;
        Hit = source.Hit;
        Dodge = source.Dodge;
        Crit = source.Crit;
        Res_Blowed = source.Res_Blowed;
        Res_Floated = source.Res_Floated;
        Res_Repulsed = source.Res_Repulsed;
        Res_Rigidity = source.Res_Rigidity;
        Res_Stuned = source.Res_Stuned;
        Res_Falled = source.Res_Falled;
        //status = new List<StatuInfo>();
        //buffs = source.buffs;
        bag = source.bag;
        warehouseInfo = source.warehouseInfo;
        mount = source.mount;
        //MoveSpeed = source.MoveSpeed;
    }
    #endregion

    #region 恢复相关
    public void Recover(bool all)
    {
        if (!IsAlive) return;
        if (all)
        {
            if (status.Count != 0)
            {
                StatuInfo temp_f = new StatuInfo(Statu.Falled, status.Find(s => s.Statu == Statu.Falled).Duration);
                status.Clear();
                if (temp_f != null)
                    status.Add(temp_f);
            }
            /*if (buffs.Count != 0)
                for (int i = 0; i < buffs.Count; i++)
                {
                    if (buffs[i].isDebuff) buffs.Remove(buffs[i]);
                }*/
            IsTired = false;
            IsExhausted = false;
        }
        Current_HP = HP;
        Current_MP = MP;
        Current_Endurance = Endurance;
        current_BlockAmount = BlockAmount;
    }

    public void Relive()
    {
        if (IsAlive) return;
        IsFighting = false;
        IsGiddy = false;
        IsDown = false;
        IsRigid = false;
        IsAlive = true;
        IsTired = false;
        IsExhausted = false;
        status.Clear();
        Current_HP = HP;
        Current_MP = MP;
        Current_Endurance = Endurance;
        Current_BlockAmount = BlockAmount;
    }

    public void GetEXP(int exp)
    {
        if (exp < 0) return;
        Current_Exp += exp;
        while (Current_Exp >= NextExp)
        {
            LevelUp(1);
        }
    }

    public bool LevelUp(int level)
    {
        if (Level >= 40 || level <= 0) return false;
        for (int i = 0; i < level; i++)
        {
            Level++;
            SkillPointOne++;
            SkillPointTwo++;
            Li += characterInfo.Add_Li;
            Ti += characterInfo.Add_Ti;
            Qi += characterInfo.Add_Qi;
            Ji += characterInfo.Add_Ji;
            Min += characterInfo.Add_Min;
            HP += characterInfo.Add_HP + characterInfo.Add_Ti * 100;
            MP += characterInfo.Add_MP + characterInfo.Add_Li * 2 + characterInfo.Add_Ti * 3;
            Endurance += characterInfo.Add_Endurance + characterInfo.Add_Ti * 2 + characterInfo.Add_Qi;
            BlockAmount += System.Convert.ToInt32(HP * 0.5f);
            Current_BlockAmount = BlockAmount;
            Neili += characterInfo.Add_Neili + characterInfo.Add_Qi;
            Hit += characterInfo.Add_Ji * 0.4f;
            Dodge += characterInfo.Add_Min;
            Crit += (characterInfo.Add_Ji + characterInfo.Add_Min) * 0.2f;
            ATK += characterInfo.Add_Li * 3;
            DEF += characterInfo.Add_Qi * 3;
            Res_Rigidity += 1;
            Res_Falled += 1;
            Res_Stuned += 1;
            MyTools.GetNextEXP(this);
            bag.MaxWeight += 10;
            bag.MaxSize += 1;
            bag.CheckSizeAndWeight();
            if (Level >= 40)
                break;
        }
        Recover(true);
        return true;
    }
    #endregion

    #region 时间相关
    public void AutoRecover()
    {
        if (Endurance <= 0 || !IsAlive) return;
        else if (Endurance > 0 && Endurance < 5)
        {
            Current_HP += Neili;
            Current_MP += Neili;
        }
        else
        {
            Current_HP += Neili;
            Current_MP += Neili;
        }
    }

    public void SubEndurance(int num)
    {
        Current_Endurance -= num;
        if (Current_Endurance <= 5) IsTired = true;
        if (Current_Endurance <= 0) IsExhausted = true;
    }
    #endregion

    #region 乘骑相关
    public void Mount()
    {
        if (IsMounting) return;
        mount.OnMount(this);
    }

    public void Dismount()
    {
        if (!IsMounting) return;
        mount.OnDismount(this);
    }
    #endregion

    #region 装备相关
    public bool UnequipWeapon(bool shield)
    {
        try
        {
            if (shield)
                equipments.Unequip(false, true, false, false, false, false, false, false, false, false, this);
            else
                equipments.Unequip(true, false, false, false, false, false, false, false, false, false, this);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
    public bool UnequipArmor(bool clothes, bool helmet, bool wristband, bool shoes)
    {
        try
        {
            equipments.Unequip(false, false, clothes, helmet, wristband, shoes, false, false, false, false, this);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
    public bool UnequipJewelry(bool necklace, bool belt, bool ring_1, bool ring_2)
    {
        try
        {
            equipments.Unequip(false, false, false, false, false, false, necklace, belt, ring_1, ring_2, this);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
    #endregion

    #region 状态相关
    public bool SetStatu(StatuInfo sinfo)
    {
        if (sinfo == null || SuperArmor) return false;
        if (status == null) status = new List<StatuInfo>();
        if (CheckStatuRes(sinfo.Statu)) {/* Debug.Log("抵抗了状态" + sinfo.statu_name);*/ return false; }//如果抵抗了该状态
        //Debug.Log("设置了状态:" + sinfo.Statu.ToString() + ",持续时间为:" + sinfo.Duration);
        if (sinfo.Statu == Statu.Giddy && status.Exists(s => s.Statu == Statu.Falled))
        {
            return false;
        }
        if (sinfo.Statu == Statu.Falled && status.Exists(s => s.Statu == Statu.Giddy))
        {
            CleanStatu(Statu.Giddy);
        }
        if (status.Exists(s => s.Statu == sinfo.Statu))
        {
            return false;
        }
        status.Add(sinfo);
        switch (sinfo.Statu)
        {
            case Statu.BlockBroken: IsBlockBroken = true; break;
            case Statu.Giddy: IsGiddy = true; break;
            case Statu.Falled: IsDown = true; break;
            case Statu.Rigidity:IsRigid = true;break;
        }
        status.Sort((x, y) => { if (x.Statu > y.Statu) return -1; else if (x.Statu == y.Statu) return 0; else return 1; });
        return true;
    }

    public void CleanStatu(Statu statu)
    {
        if (status == null) return;
        //Debug.Log("清除了状态:" + statu.ToString());
        if (statu == Statu.BlockBroken) IsBlockBroken = false;
        status.RemoveAll(s => s.Statu == statu);
        switch (statu)
        {
            case Statu.BlockBroken: IsBlockBroken = false; break;
            case Statu.Giddy: IsGiddy = false; break;
            case Statu.Falled: IsDown = false; break;
            case Statu.Rigidity: IsRigid = false; break;
        }
    }

    public bool CheckStatuRes(Statu statu)
    {
        switch (statu)
        {
            case Statu.Rigidity: return MyTools.Probability(Res_Rigidity);
            case Statu.Repulsed: return MyTools.Probability(Res_Repulsed);
            case Statu.Giddy: return MyTools.Probability(Res_Stuned);
            case Statu.Floated: return MyTools.Probability(Res_Floated);
            case Statu.Blowed: return MyTools.Probability(Res_Blowed);
            case Statu.Falled: return MyTools.Probability(Res_Falled);
            default: return false;
        }
    }
    #endregion
}