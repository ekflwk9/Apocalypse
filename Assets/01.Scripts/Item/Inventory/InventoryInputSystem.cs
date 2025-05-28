using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryInputSystem : UiButton
{
    private ItemInfo currentItem;

    public void SetItem(ItemInfo item)
    {
        currentItem = item;
        info.text = item.itemName;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            ItemManager.Instance.inventory.ClickItem(currentItem);
        }
    }
}