using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemInfo firstSlotItem { get; private set; }
    public ItemInfo secondSlotItem { get; private set; }
    public List<ArmorInfo> equipped { get; private set; } = new List<ArmorInfo>();
    public List<ItemInfo> inventory { get; private set; } = new List<ItemInfo>();

    public void Add(int _itemId, bool isEquipped = false) // 아이템 추가시 호출
    {
        var item = ItemManager.Instance.GetItem(_itemId);

        if (item != null)
        {
            if (isEquipped)
            {
                inventory.Add(item);
            }

            else if (item is ArmorInfo isArmor)
            {
                equipped.Add(isArmor);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void RemoveInventory(int _itemId, bool isEquipped = false)
    {
        var item = ItemManager.Instance.GetItem(_itemId);

        if (item != null)
        {
            if (isEquipped && inventory.Contains(item))
            {
                inventory.Remove(item);
            }

            else if (item is ArmorInfo isArmor && equipped.Contains(isArmor))
            {
                equipped.Remove(isArmor);
            }
        }
    }

    /// <summary>
    /// 아이템 소모시 호출되는 메서드
    /// </summary>
    /// <param name="_itemId"></param>
    public void UseItem(int _itemId)
    {
        ItemInfo item = ItemManager.Instance.GetItem(_itemId);

        if (item != null)
        {
            if (inventory.Contains(item) && UiManager.instance.status.Remove(item))
            {
                inventory.Remove(item);
                ItemEffectManager.Instance.ItemEffect(item);
            }
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

        if (equipped[ranIndex].defense == 0)
        {
            UiManager.instance.status.HideArmorView(ranIndex);
            equipped.RemoveAt(ranIndex);
        }
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
