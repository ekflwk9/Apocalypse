using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int selectItemId;
    public ItemInfo firstId;
    public ItemInfo secondId;
    public List<ItemInfo> items = new List<ItemInfo>(); // 인벤토리

    public void GetItem(ItemInfo item) // 아이템 추가시 호출
    {
        items.Add(item);
        UiManager.instance.status.GetItem(item.itemId);
    }

    public void RemoveItem(ItemInfo item) // 아이템 제거, 판매시 호출
    {
        items.Remove(item);
    }

    public void UseItem(ItemInfo item) // 소모 아이템 사용시 호출
    {
        ItemEffectManager.Instance.ItemEffect(item);
        items.Remove(item);
    }

    public void ClickItem(ItemInfo item) // 아이템 클릭시 호출
    {
        selectItemId = item.itemId;
    }
}
