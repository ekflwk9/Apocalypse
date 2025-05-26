using System.Collections.Generic;
using GameItem;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ItemManager").AddComponent<ItemManager>();
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
    public void PickUp(int itemId)
    {
        ItemInfo item;
        // ItemManager.Inventory.GetItem(itemId);
    }
    public void UseItem(ItemInfo itemInfo)
    {

    }
}
