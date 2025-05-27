using System.Collections;
using System.Collections.Generic;
using GameItem;
using UnityEngine;

public class ItemEquipment : MonoBehaviour
{
    public WeaponInfo eqWeapon;
    public ArmorInfo[] eqArmor;
    private bool isWeaponEquiped;
    private bool[] isArmorEquiped = new bool[4];
    private void Reset()
    {
        isWeaponEquiped = false;
        for (int i = 0; i < 4; i++)
        {
            isArmorEquiped[i] = false;
        }
    }

    public void SelectEquipment(int itemId)
    {
        if (ItemManager.Instance.itemDB[itemId] is WeaponInfo weapon)
        {
            if (!isWeaponEquiped)
            {
                eqWeapon = weapon;
                isWeaponEquiped = true;
            }
            else { }
        }
        else if (ItemManager.Instance.itemDB[itemId] is ArmorInfo armor)
        {
            switch (armor.armorType)
            {
                case ArmorType.Head:
                    if (!isArmorEquiped[0])
                    {
                        eqArmor[0] = armor;
                        isArmorEquiped[0] = true;
                    }
                    break;
                case ArmorType.Top:
                    if (!isArmorEquiped[1])
                    {
                        eqArmor[1] = armor;
                        isArmorEquiped[1] = true;
                    }
                    break;
                // case ArmorType.Bottom:
                //     if (!isArmorEquiped[2])
                //     {
                //         eqArmor[2] = armor;
                //         isArmorEquiped[2] = true;
                //     }
                //     break;
                case ArmorType.Shoes:
                    if (!isArmorEquiped[3])
                    {
                        eqArmor[3] = armor;
                        isArmorEquiped[3] = true;
                    }
                    break;
            }
        }
    }

    public void SelectUnEquipment(int itemId)
    {
        if (ItemManager.Instance.itemDB[itemId] is WeaponInfo weapon)
        {
            if (isWeaponEquiped)
            {
                eqWeapon = null;
                isWeaponEquiped = false;
            }
            else { }
        }
        else if (ItemManager.Instance.itemDB[itemId] is ArmorInfo armor)
        {
            switch (armor.armorType)
            {
                case ArmorType.Head:
                    if (isArmorEquiped[0])
                    {
                        eqArmor[0] = null;
                        isArmorEquiped[0] = false;
                    }
                    break;
                case ArmorType.Top:
                    if (isArmorEquiped[1])
                    {
                        eqArmor[1] = null;
                        isArmorEquiped[1] = false;
                    }
                    break;
                // case ArmorType.Bottom:
                //     if (isArmorEquiped[2])
                //     {
                //         eqArmor[2] = null;
                //         isArmorEquiped[2] = false;
                //     }
                //     break;
                case ArmorType.Shoes:
                    if (isArmorEquiped[3])
                    {
                        eqArmor[3] = null;
                        isArmorEquiped[3] = false;
                    }
                    break;
            }
        }
    }

    public void EqWeapon()
    {

    }
}
