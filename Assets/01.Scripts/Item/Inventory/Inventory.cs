using System.Collections.Generic;
using UnityEngine;

public enum SlotType
{
    Inventory,
    Stotage,
    Equipped,
}

public class Inventory : MonoBehaviour
{
    public ItemInfo firstSlotItem { get; private set; }
    public ItemInfo secondSlotItem { get; private set; }
    public List<ArmorInfo> equipped { get; private set; } = new List<ArmorInfo>();
    public List<ItemInfo> inventory { get; private set; } = new List<ItemInfo>();
    public List<ItemInfo> storage { get; private set; } = new List<ItemInfo>();

    public void Add(int itemId) // 아이템 추가시 호출
    {
        var item = ItemManager.Instance.GetItem(itemId);
        if (item != null) inventory.Add(item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void RemoveInventory(int _itemId, SlotType _slotType)
    {
        var item = ItemManager.Instance.GetItem(_itemId);
        if (item != null) return;

        switch (_slotType)
        {
            case SlotType.Inventory:
                inventory.Remove(item);
                break;

            case SlotType.Stotage:
                storage.Remove(item);
                break;

            case SlotType.Equipped:
                var euqippItem = item as ArmorInfo;
                equipped.Remove(euqippItem);
                break;
        }
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

    /// <summary>
    /// 방어구 아이템 업데이트 콜백 함수
    /// </summary>
    /// <param name="damage"></param>
    public void Defense(int damage)
    {
        int ranIndex = Random.Range(0, equipped.Count);
        equipped[ranIndex].defense -= 1;

        UiManager.instance.status.UpdateArmorView(ranIndex, equipped[ranIndex]);
        if (equipped[ranIndex].defense <= 0) equipped[ranIndex] = null;
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
