using System.Collections;
using System.Collections.Generic;
using GameItem;
using UnityEngine;

public class ItemEquipment
{
    public WeaponInfo eqWeapon;
    public ArmorInfo eqArmor;

    public void SelectEquipment(int itemId)
    {
        if (ItemManager.Instance.itemDB[itemId] is WeaponInfo weapon)
        {
            eqWeapon = weapon;
        }
        else if (ItemManager.Instance.itemDB[itemId] is ArmorInfo)
        {

        }
    }

    public void EqWeapon()
    {

    }
}
