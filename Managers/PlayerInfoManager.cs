using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using MyEnums;
using Newtonsoft.Json;

namespace MyEnums
{
    public enum PlayerCharacterType
    {
        Boy,
        Girl,
        LittleGirl
    }
}

[DisallowMultipleComponent]
public class PlayerInfoManager : MonoBehaviour {
    public static PlayerInfoManager Instance;
    public static readonly string DataName = "/PlayerInfoData.zetan";

    public GameObject Player;
    public bool isInit;

    public PlayerInfo PlayerInfo;
    public PlayerCharacterType characterType;

    public GameObject reliveWindow;
    [HideInInspector]
    float sittingTime;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void Init(GameObject playerPrefab)
    {
        Player = Instantiate(playerPrefab);
        PlayerInfo = new PlayerInfo
        {
            mount = new MountInfo(150, 150, 100, 100)
        };
        characterType = PlayerSelector.Instance.characterType;
        CameraFollow camera = FindObjectOfType<CameraFollow>();
        camera.SetTarget(Player.transform);
        Player.GetComponentInChildren<MalbersAnimations.HAP.Rider3rdPerson>().AnimalStored = FindObjectOfType<MalbersAnimations.HAP.Mountable>();
        switch (characterType)
        {
            case PlayerCharacterType.Boy:
                PlayerInfo.SetCharacter(new CharacterInfo(100001, "李骁", "字天杰", "", 5, 4, 3, 2, 1, 10, 8, 4, 5));
                camera.headOffset = new Vector3(0, 1.7f, 0);
                camera.transform.position = Player.transform.position + -Player.transform.forward * 2f + Player.transform.up * 1.7f;
                break;
            case PlayerCharacterType.Girl:
                PlayerInfo.SetCharacter(new CharacterInfo(100002, "李俏", "字婖洁", "", 4, 3, 3, 3, 2, 8, 6, 6, 5));
                camera.headOffset = new Vector3(0, 1.6f, 0);
                camera.transform.position = Player.transform.position + -Player.transform.forward * 2f + Player.transform.up * 1.6f;
                break;
            case PlayerCharacterType.LittleGirl:
                PlayerInfo.SetCharacter(new CharacterInfo(100003, "李筱", "字恬婕", "", 3, 2, 1, 5, 4, 6, 6, 2, 5));
                camera.headOffset = new Vector3(0, 1.2f, 0);
                camera.transform.position = Player.transform.position + -Player.transform.forward * 2f + Player.transform.up * 1.2f;
                break;
        }
        FindObjectOfType<bl_MiniMap>().SetTarget(Player);
        LevelUp(1);
        isInit = true;
        PlayerLocomotionManager.Instance.Init();
        PlayerWeaponManager.Instance.Init();
        BagManager.Instance.Init();
        WarehouseManager.Instance.Init();
        StartCoroutine(Auto_Recover());
    }
    // Use this for initialization
    /*void Start () {

    }*/
    // Update is called once per frame
    void Update() {
        if (!isInit) return;
        if (PlayerInfo.IsSitting)
        {
            sittingTime += Time.deltaTime;
            if (sittingTime > 5)
            {
                PlayerInfo.Current_HP += System.Convert.ToInt32(PlayerInfo.HP * 0.1f);
                PlayerInfo.Current_MP += System.Convert.ToInt32(PlayerInfo.MP * 0.1f);
                sittingTime = 0;
            }
        }
        else
        {
            sittingTime = 0;
        }
        if(PlayerInfo.status.Count > 0)
        {
            StatusCD();
        }
    }

    public void SavePlayerInfo(string path, string key = "", bool encrypt = false)
    {
        if (encrypt && key.Length == 32)
        {
            File.WriteAllText(path + "/PlayerInfoData.zetan", Encryption.Encrypt(
            JsonConvert.SerializeObject(PlayerInfo), key), Encoding.UTF8);
        }
        else
        {
            File.WriteAllText(path + "/PlayerInfoData.zetan", JsonConvert.SerializeObject(PlayerInfo));
        }
        PlayerInfo.bag.Save(path, key, encrypt);
        PlayerInfo.warehouseInfo.Save(path, key, encrypt);
    }
    public bool LoadPlayerInfo(string path, string key = "", bool dencrypt = false)
    {
        //try
        {
            if (File.Exists(path + "/PlayerInfoData.zetan"))
            {
                if (dencrypt && key.Length == 32) PlayerInfo.CopyInfo(JsonConvert.DeserializeObject<PlayerInfo>(Encryption.Dencrypt(File.ReadAllText(path + "/PlayerInfoData.zetan"), key)) ?? PlayerInfo);
                else PlayerInfo.CopyInfo(JsonConvert.DeserializeObject<PlayerInfo>(File.ReadAllText(path + "/PlayerInfoData.zetan")) ?? PlayerInfo);
                PlayerInfo.bag.Load(path, key, dencrypt);
                PlayerInfo.warehouseInfo.Load(path, key, dencrypt);
                #region 修复装备图标
                /*if (PlayerInfo.equipments.weapon != null)
                { 
                    WeaponItem newWeapon = PlayerInfo.equipments.weapon.Clone() as WeaponItem;
                    PlayerInfo.bag.itemList.Add(new ItemInfo(newWeapon));
                    newWeapon.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.shield != null)
                {
                    WeaponItem newShield = PlayerInfo.equipments.shield.Clone() as WeaponItem;
                    newShield.Equip(PlayerInfo);
                }

                if (PlayerInfo.equipments.clothes != null)
                {
                    ArmorItem newClothes = PlayerInfo.equipments.clothes.Clone() as ArmorItem;
                    newClothes.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.helmet != null)
                {
                    ArmorItem newHelmet = PlayerInfo.equipments.helmet.Clone() as ArmorItem;
                    newHelmet.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.wristband != null)
                {
                    ArmorItem newWristband = PlayerInfo.equipments.wristband.Clone() as ArmorItem;
                    newWristband.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.shoes != null)
                {
                    ArmorItem newShoes = PlayerInfo.equipments.shoes.Clone() as ArmorItem;
                    newShoes.Equip(PlayerInfo);
                }

                if (PlayerInfo.equipments.necklace != null)
                {
                    JewelryItem newNecklace = PlayerInfo.equipments.necklace.Clone() as JewelryItem;
                    newNecklace.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.belt != null)
                {
                    JewelryItem newBelt = PlayerInfo.equipments.belt.Clone() as JewelryItem;
                    newBelt.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.ring_1 != null)
                {
                    JewelryItem newRing_1 = PlayerInfo.equipments.ring_1.Clone() as JewelryItem;
                    newRing_1.Equip(PlayerInfo);
                }
                if (PlayerInfo.equipments.ring_2 != null)
                {
                    JewelryItem newRing_2 = PlayerInfo.equipments.ring_2.Clone() as JewelryItem;
                    newRing_2.Equip(PlayerInfo);
                }*/
                #endregion
            }
            else
                throw new System.Exception("存档不存在");
            switch (PlayerInfo.ID)
            {
                case 100001: characterType = PlayerCharacterType.Boy; break;
                case 100002: characterType = PlayerCharacterType.Girl; break;
                case 100003: characterType = PlayerCharacterType.LittleGirl; break;
            }
            PlayerWeaponManager.Instance.CheckWeaponTypeForCharactor();
            PlayerWeaponManager.Instance.EquipWeapon();
            PlayerWeaponManager.Instance.NoWeapon();
            PlayerSkillManager.Instance.CheckSkillButtonEnable();
            BagManager.Instance.LoadFromBagInfo();
            WarehouseManager.Instance.LoadFromWarehouseInfo();
            return true;
        }
        /*catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            //if (NotificationManager.Self) NotificationManager.Self.NewNotification(ex.Message);
            return false;
        }*/
    }

    public void LevelUp(int level)
    {
        if (PlayerInfo == null) return;
        int lastlevel = PlayerInfo.Level;
        if (PlayerInfo.LevelUp(level))
        {
            foreach (SkillInfoAgent sa in PlayerSkillManager.Instance.skillsList.FindAll(s => s.isNormalAtk))
                for (int i = 0; i < PlayerInfo.Level - lastlevel; i++)
                    sa.OnLevelUp();
        }
    }

    IEnumerator Auto_Recover()
    {
        if (PlayerInfo == null) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(20);
        PlayerInfo.AutoRecover();
        StartCoroutine(Auto_Recover());
    }

    public void BeAttack(EnemyInfoAgent enermyInfoAgent, SkillInfoAgent skillInfo, bool hurtForwad)
    {
        if (PlayerInfo == null || !PlayerInfo.IsAlive) return;
        DamageType damageType;
        StatuInfo statuInfo;
        int damageValue = MyTools.GetDamaged(true, PlayerInfo, enermyInfoAgent, skillInfo, out damageType, out statuInfo, hurtForwad);
        //Debug.Log("Pleyer is hurt:" + damageType + "||" + damageValue);
        PlayerInfo.Current_HP -= damageValue;
        if (statuInfo.Statu != Statu.None)
        {
            if (SetStatu(statuInfo))
            {
                switch (statuInfo.Statu)
                {
                    case Statu.Falled:  break;
                    case Statu.Giddy:  break;
                    case Statu.Rigidity:
                        PlayerLocomotionManager.Instance.playerAnima.speed = 0;
                        PlayerLocomotionManager.Instance.playerController.moveAble = false;
                        PlayerLocomotionManager.Instance.playerController.rotateAble = false;
                        break;
                }
                HUDTextManager.Instance.NewStatu(Player.transform, statuInfo.Statu);
            }
        }
        switch (damageType)
        {
            case DamageType.Miss:
                HUDTextManager.Instance.NewMiss(Player.transform);
                PlayerLocomotionManager.Instance.SetPlayerMissAnima(); break;
            case DamageType.HurtBackward:
                HUDTextManager.Instance.NewBackDamageValue(Player.transform, damageValue);
                if (statuInfo.Statu == Statu.None) PlayerLocomotionManager.Instance.SetPlayerHurtAnima(); break;
            case DamageType.BlockBroken:
                HUDTextManager.Instance.NewBlockBroken(Player.transform);
                SetStatu(new StatuInfo(Statu.BlockBroken, 5)); break;
            case DamageType.Block:
                HUDTextManager.Instance.NewBlock(Player.transform); break;
            case DamageType.Crit:
                HUDTextManager.Instance.NewCritDamageValue(Player.transform, damageValue);
                if (statuInfo.Statu == Statu.None) PlayerLocomotionManager.Instance.SetPlayerHurtAnima(); break;
            default:
                HUDTextManager.Instance.NewDamageValue(Player.transform, damageValue);
                if (statuInfo.Statu == Statu.None) PlayerLocomotionManager.Instance.SetPlayerHurtAnima(); break;
        }
        if (PlayerWeaponManager.Instance.currentWeaponTag == string.Empty)
            PlayerInfo.IsFighting = false;
        else PlayerInfo.IsFighting = true;
        if (!PlayerInfo.IsAlive)
        {
            MyTools.SetActive(reliveWindow, true);
        }
    }

    public void GetEXP(int exp)
    {
        int lastLevel = PlayerInfo.Level;
        PlayerInfo.GetEXP(exp);
        if(PlayerInfo.Level > lastLevel)
        {
            NotificationManager.Instance.NewNotification("修炼水平达到了<color=yellow>" + PlayerInfo.Level +"</color>级");
        }
    }

    #region 状态相关
    bool SetStatu(StatuInfo sinfo)
    {
        if (PlayerInfo.status == null)
        {
            PlayerInfo.status = new List<StatuInfo>();
        }
        return PlayerInfo.SetStatu(sinfo);
    }

    void StatusCD()
    {
        if (PlayerInfo == null || PlayerInfo.status == null) return;
        for (int i = 0; i < PlayerInfo.status.Count; i++)
        {
            PlayerInfo.status[i].SubTime(Time.deltaTime);
            if (PlayerInfo.status[i].IsEnd())
            {
                if (PlayerInfo.status[i].Statu == Statu.Rigidity)
                {
                    PlayerLocomotionManager.Instance.playerAnima.speed = 1;
                    PlayerLocomotionManager.Instance.playerController.moveAble = true;
                    PlayerLocomotionManager.Instance.playerController.rotateAble = true;
                }
                PlayerInfo.CleanStatu(PlayerInfo.status[i].Statu);
            }
        }
    }
    #endregion
}