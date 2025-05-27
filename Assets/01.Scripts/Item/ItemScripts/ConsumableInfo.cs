using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameItem
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "New Consumable")]
    public class ConsumableInfo : ItemInfo
    {
        public int st;
        public int value;
        public float defense;
        public float coolDown;
    }
}