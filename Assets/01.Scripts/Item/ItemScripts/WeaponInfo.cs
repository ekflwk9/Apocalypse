using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameItem
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "New Weapon")]
    public class WeaponInfo : ItemInfo
    {
        public int durability;
        public int power;
        public float AttackSpeed;
    }
}
