using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoint : MonoBehaviour
{
    private ItemInfo thisItem;

    private void Awake()
    {
        thisItem = ItemManager.Instance.GetItem(ItemDropGenerator.GetRandomItemId());
        ObjectPool.Instance.Get(thisItem.itemPrefab);
    }
}
