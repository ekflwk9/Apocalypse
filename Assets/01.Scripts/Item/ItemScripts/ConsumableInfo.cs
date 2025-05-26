using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameItem
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "New Consumable")]
    public class ConsumableInfo : ItemInfo
    {
        public int value;
        public float coolDown;
    }
}