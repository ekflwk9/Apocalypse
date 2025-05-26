using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameItem
{
    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 4,
        Stuff = 8
    }

    public enum ArmorType
    {
        Head,
        Top,
        Bottom,
        Shoes,
    }
}
