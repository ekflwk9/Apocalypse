using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "New Consumable")]
public class ConsumableInfo : ItemInfo
{
    public int value;
    public float defense;
    public float coolDown;
}
