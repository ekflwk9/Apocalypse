using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    private static ItemEffectManager _instance;
    public static ItemEffectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ItemEffectManager").AddComponent<ItemEffectManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ItemEffect(ItemInfo itemInfo)
    {
        switch (itemInfo.itemType)
        {
            case ItemType.Weapon:
                break;
            case ItemType.Armor:
                if (ItemManager.Instance.itemDB[itemInfo.itemId] is ArmorInfo armor)
                ChangeStat(2, armor.defense);
                break;

            case ItemType.Consumable:
                if (ItemManager.Instance.itemDB[itemInfo.itemId] is ConsumableInfo consumable)
                ChangeStat(3, consumable.value);
                break;
        }
    }

    public void ChangeStat(int key, object value)
    {
        switch (key)
        {
            case 1:
                break;
            case 2:
                // Defense += (float)value;
                break;
            case 3:
                Player.Instance.Health += (float)value;
                break;
        }
    }
}
