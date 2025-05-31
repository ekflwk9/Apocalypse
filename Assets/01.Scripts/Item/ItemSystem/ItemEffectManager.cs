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
                if (ItemManager.Instance.GetItem(itemInfo.itemId) is ArmorInfo armor)
                {

                }
                break;

            case ItemType.Consumable:
                if (ItemManager.Instance.GetItem(itemInfo.itemId) is ConsumableInfo consumable)
                {
                    Player.Instance.Heal(consumable.value);
                }
                break;
        }
    }
}
