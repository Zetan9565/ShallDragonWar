using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EquipmentsManager : MonoBehaviour {
    public static EquipmentsManager Instance;
    public GameObject UI;
    [Space]
    public GameObject WeaponCell;
    public GameObject ShieldCell;
    public GameObject ClothesCell;
    public GameObject HelmetCell;
    public GameObject WristBandCell;
    public GameObject ShoesCell;
    public GameObject NecklaceCell;
    public GameObject BeltCell;
    public GameObject Ring_1Cell;
    public GameObject Ring_2Cell;
    public Sprite empty;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        WeaponCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(1); });
        ShieldCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(2); });
        ClothesCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(3); });
        HelmetCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(4); });
        WristBandCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(5); });
        ShoesCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(6); });
        NecklaceCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(7); });
        BeltCell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(8); });
        Ring_1Cell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(9); });
        Ring_2Cell.GetComponent<Button>().onClick.AddListener(delegate () { OnIconClick(10); });
        CloseUI();
    }

    private void Update()
    {
        if(Input.GetButtonDown("PersonWindow"))
        {
            if (!UI.activeSelf) OpenUI();
            else CloseUI();
        }
    }

    public void CheckEquipment()
    {
        if (PlayerInfoManager.Instance.PlayerInfo.equipments.weapon != null)
        {
            WeaponCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.weapon.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            WeaponCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.shield != null)
        {
            ShieldCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.shield.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            ShieldCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.clothes != null)
        {
            ClothesCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.clothes.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            ClothesCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.helmet != null)
        {
            HelmetCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.helmet.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            HelmetCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.wristband != null)
        {
            WristBandCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.wristband.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            WristBandCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }



        if (PlayerInfoManager.Instance.PlayerInfo.equipments.shoes != null)
        {
            ShoesCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.shoes.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            ShoesCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.necklace != null)
        {
            NecklaceCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.necklace.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            NecklaceCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.belt != null)
        {
            BeltCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.belt.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            BeltCell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }




        if (PlayerInfoManager.Instance.PlayerInfo.equipments.ring_1 != null)
        {
            Ring_1Cell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.ring_1.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            Ring_1Cell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }
        if (PlayerInfoManager.Instance.PlayerInfo.equipments.ring_2 != null)
        {
            Ring_2Cell.transform.Find("Icon").GetComponent<Image>().overrideSprite = Resources.Load(PlayerInfoManager.Instance.PlayerInfo.equipments.ring_2.Icon, typeof(Sprite)) as Sprite;
        }
        else
        {
            Ring_2Cell.transform.Find("Icon").GetComponent<Image>().overrideSprite = empty;
        }
    }

    public void OnIconClick(int select)
    {
        switch (select)
        {
            case 1:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.weapon != null)
                {
                    ItemTipsManager.Instance.equipInfo =  PlayerInfoManager.Instance.PlayerInfo.equipments.weapon;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(1); });
                }
                break;
            case 2:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.shield != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.shield;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(2); });
                }
                break;
            case 3:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.clothes != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.clothes;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(3); });
                }
                break;
            case 4:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.helmet != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.helmet;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(4); });
                }
                break;
            case 5:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.wristband != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.wristband;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(5); });
                }
                break;
            case 6:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.shoes != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.shoes;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(6); });
                }
                break;
            case 7:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.necklace != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.necklace;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(7); });
                }
                break;
            case 8:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.belt != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.belt;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(8); });
                }
                break;
            case 9:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.ring_1 != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.ring_1;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(9); });
                }
                break;
            case 10:
                if (PlayerInfoManager.Instance.PlayerInfo.equipments.ring_2 != null)
                {
                    ItemTipsManager.Instance.equipInfo = PlayerInfoManager.Instance.PlayerInfo.equipments.ring_2;
                    ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
                    ItemTipsManager.Instance.UnequipButton.onClick.AddListener(delegate () { Unequip(10); });
                }
                break;
        }
        ItemTipsManager.Instance.TipsType = ItemTipsManager.ItemTipsType.EuipmentItem;
        ItemTipsManager.Instance.OpenUI();
    }

    public void Unequip(int select)
    {
        switch (select)
        {
            case 1:
                PlayerInfoManager.Instance.PlayerInfo.UnequipWeapon(false);
                CheckEquipment();
                PlayerWeaponManager.Instance.NoWeapon();
                break;
            case 2:
                PlayerInfoManager.Instance.PlayerInfo.UnequipWeapon(true);
                CheckEquipment();
                break;
            case 3:
                PlayerInfoManager.Instance.PlayerInfo.UnequipArmor(true, false, false, false);
                CheckEquipment();
                break;
            case 4:
                PlayerInfoManager.Instance.PlayerInfo.UnequipArmor(false, true, false, false);
                CheckEquipment();
                break;
            case 5:
                PlayerInfoManager.Instance.PlayerInfo.UnequipArmor(false, false, true, false);
                CheckEquipment();
                break;
            case 6:
                PlayerInfoManager.Instance.PlayerInfo.UnequipArmor(false, false, false, true);
                CheckEquipment();
                break;
            case 7:
                PlayerInfoManager.Instance.PlayerInfo.UnequipJewelry(true, false, false, false);
                CheckEquipment();
                break;
            case 8:
                PlayerInfoManager.Instance.PlayerInfo.UnequipJewelry(false, true, false, false);
                CheckEquipment();
                break;
            case 9:
                PlayerInfoManager.Instance.PlayerInfo.UnequipJewelry(false, false, true, false);
                CheckEquipment();
                break;
            case 10:
                PlayerInfoManager.Instance.PlayerInfo.UnequipJewelry(false, false, false, true);
                CheckEquipment();
                break;
        }
        ItemTipsManager.Instance.equipInfo = null;
        ItemTipsManager.Instance.UnequipButton.onClick.RemoveAllListeners();
        ItemTipsManager.Instance.CloseUI();
        BagManager.Instance.LoadFromBagInfo();
    }

    public void OpenUI()
    {
        CheckEquipment();
        MyTools.SetActive(UI, true);
    }

    public void CloseUI()
    {
        MyTools.SetActive(UI, false);
    }
}
