using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemInfo firstSlotItem;
    public ItemInfo secondSlotItem;
    public ArmorInfo[] currentArmor = new ArmorInfo[Enum.GetValues(typeof(ArmorType)).Length];
    public List<ItemInfo> items = new List<ItemInfo>(); // 인벤토리

    public void GetItem(int itemId) // 아이템 추가시 호출
    {
        items.Add(ItemManager.Instance.itemDB[itemId]);
    }

    public void RemoveInventoryItem(int index) // 아이템 제거, 판매시 호출
    {
        ItemInfo item = ItemManager.Instance.itemDB[index];
        items.Remove(item);
    }

    public void UseInventoryItem(int index) // 소모 아이템 사용시 호출
    {
        ItemInfo item = ItemManager.Instance.itemDB[index];
        ItemEffectManager.Instance.ItemEffect(item);
        items.Remove(item);
    }

    public void ChangeDefense(int damage)
    {
        int ranNum = UnityEngine.Random.Range(0, currentArmor.Length);
        // currentArmor[ranNum] = 
    }

    public void ChangeMainSlot(int _itemId, bool _isFirst)
    {
        ItemInfo item;

        if (_itemId != 0) item = ItemManager.Instance.itemDB[_itemId];
        else item = null;

        if (_isFirst) firstSlotItem = item;
        else secondSlotItem = item;

        Player.Instance.Equip.UnEquip();
    }
}
