using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemInfo : ScriptableObject
{
    [Header("Info")]
    public int itemId;
    public string itemName;
    public string disciption;
    public ItemType itemType;
    public Sprite icon;
    public int cost;

    [Header("Data")]
    public bool canStack;
    public int maxStack;
    public int weight;
}

