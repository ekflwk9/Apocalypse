using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameItem
{
    public class ItemInfo : ScriptableObject
    {
        [Header("Info")]
        public int itemId;
        public string displayName;
        public string disciption;
        public ItemType itemType;
        public Sprite icon;
        public int cost;

        [Header("Data")]
        public int[] cell;
        public bool canStack;
        public int maxStack;
    }
}
