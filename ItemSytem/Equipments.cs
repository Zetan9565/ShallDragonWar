using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class Equipments
{
    public bool IsWpEquip;
    public bool IsSdEquip;
    public bool IsClEquip;
    public bool IsHmEquip;
    public bool IsWBEquip;
    public bool IsShEquip;
    public bool IsNlEquip;
    public bool IsBtEquip;
    public bool IsRgEquip_1;
    public bool IsRgEquip_2;

    public WeaponItem weapon;
    public WeaponItem shield;
    public ArmorItem clothes;
    public ArmorItem helmet;
    public ArmorItem wristband;
    public ArmorItem shoes;
    public JewelryItem necklace;
    public JewelryItem belt;
    public JewelryItem ring_1;
    public JewelryItem ring_2;
    public List<SuitEffectInfo> suitEffect;

    public Equipments()
    {
        IsWpEquip = IsSdEquip = IsClEquip = IsHmEquip = IsWBEquip = IsShEquip = IsNlEquip = IsBtEquip = IsRgEquip_1 = IsRgEquip_2 = false;
        suitEffect = new List<SuitEffectInfo>();
    }

    /*#region 单件装备
    public void EquipWeapon(WeaponItem weapon, PlayerInfo playerInfo)
    {
        //if (IsWpEquip) return;
        //if (this.weapon == null) this.weapon = weapon;
        //IsWpEquip = true;
        try
        {
            weapon.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipShield(WeaponItem shield, PlayerInfo playerInfo)
    {
        shield.Equip(playerInfo);
    }

    public void EquipClothes(ArmorItem clothes, PlayerInfo playerInfo)
    {
        try
        {
            clothes.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipHelmet(ArmorItem helmet, PlayerInfo playerInfo)
    {
        try
        {
            helmet.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipWristBand(ArmorItem wristband, PlayerInfo playerInfo)
    {
        try
        {
            wristband.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipShoes(ArmorItem shoes, PlayerInfo playerInfo)
    {
        try
        {
            shoes.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipNecklace(JewelryItem necklace, PlayerInfo playerInfo)
    {
        try
        {
            necklace.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipBelt(JewelryItem belt, PlayerInfo playerInfo)
    {
        try
        {
            belt.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void EquipRing(JewelryItem ring, PlayerInfo playerInfo)
    {
        try
        {
            ring.Equip(playerInfo);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion*/

    public void Unequip(bool weapon, bool shield, bool clothes, bool helmet, bool wristband, bool shoes, bool necklace, bool belt, bool ring_1, bool ring_2, PlayerInfo playerInfo)
    {
        try
        {
            if (weapon && IsWpEquip)
            {
                this.weapon.Unequip(playerInfo);
            }
            if (shield && IsSdEquip)
            {
                this.shield.Unequip(playerInfo);
            }
            if (clothes && IsClEquip)
            {
                this.clothes.Unequip(playerInfo);
            }
            if (helmet && IsHmEquip)
            {
                this.helmet.Unequip(playerInfo);
            }
            if (wristband && IsWBEquip)
            {
                this.wristband.Unequip(playerInfo);
            }
            if (shoes && IsShEquip)
            {
                this.shoes.Unequip(playerInfo);
            }
            if (necklace && IsNlEquip)
            {
                this.necklace.Unequip(playerInfo, 0);
            }
            if (belt && IsBtEquip)
            {
                this.belt.Unequip(playerInfo, 0);
            }
            if (ring_1 && IsRgEquip_1)
            {
                this.ring_1.Unequip(playerInfo, 1);
            }
            if (ring_2 && IsRgEquip_2)
            {
                this.ring_2.Unequip(playerInfo, 2);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    /*public void Save(string path, string key ="",bool encrypt=false)
    {
        File.WriteAllLines
    }*/
}