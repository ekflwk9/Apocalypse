using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "New Armor")]
public class ArmorInfo : ItemInfo
{
    public ArmorType armorType;
    public int defense;
}