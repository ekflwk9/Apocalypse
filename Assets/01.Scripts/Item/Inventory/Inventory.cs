using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemInfo firstSlotItem { get; private set; }
    public ItemInfo secondSlotItem { get; private set; }

    private ArmorInfo[] currentArmor = new ArmorInfo[Enum.GetValues(typeof(ArmorType)).Length];
    private List<ItemInfo> inventory = new List<ItemInfo>();
    private List<ItemInfo> storage = new List<ItemInfo>();

    public void Add(int itemId) // 아이템 추가시 호출
    {
        var item = ItemManager.Instance.GetItem(itemId);
        if (item != null) inventory.Add(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void RemoveInventoryItem(int index)
    {
        var item = ItemManager.Instance.GetItem(index);
        if (item != null) inventory.Remove(item);
    }

    /// <summary>
    /// 아이템 소모시 호출되는 메서드
    /// </summary>
    /// <param name="_itemId"></param>
    public void UseItem(int _itemId)
    {
        ItemInfo item = ItemManager.Instance.GetItem(_itemId);
        ItemEffectManager.Instance.ItemEffect(item);

        if (item != null)
        {
            if (inventory.Contains(item)) inventory.Remove(item);
            else DebugHelper.Log($"inventory에 {item.itemName}이라는 값은 추가된적 없음");
        }
    }

    public void ChangeDefense(int damage)
    {
        int ranNum = UnityEngine.Random.Range(0, currentArmor.Length);
        // currentArmor[ranNum] = 
    }

    public void ChangeMainItem(int _itemId, bool _isFirst)
    {
        ItemInfo item;

        if (_itemId != 0) item = ItemManager.Instance.GetItem(_itemId);
        else item = null;

        if (_isFirst) firstSlotItem = item;
        else secondSlotItem = item;

        Player.Instance.Equip.UnEquip();
    }
}
