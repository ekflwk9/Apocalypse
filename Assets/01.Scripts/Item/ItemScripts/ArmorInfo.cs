using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameItem
{
    [CreateAssetMenu(fileName = "Armor", menuName = "New Armor")]
    public class ArmorInfo : ItemInfo
    {
        public ArmorType armorType;
        public int durability;
        public int defense;
    }
}
