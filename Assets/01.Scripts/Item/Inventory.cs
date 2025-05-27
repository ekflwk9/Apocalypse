using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameItem
{
    public class Inventory : MonoBehaviour
    {
        int selectItemId;
        public List<ItemInfo> items = new List<ItemInfo>(); // 인벤토리

        public void GetItem(ItemInfo item) // 아이템 추가시 호출
        {
            items.Add(item);
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

        public void EqButton() // 장착 버튼 누를시 호출
        {
            if (ItemManager.Instance.itemDB[selectItemId] is WeaponInfo)
            {
                ItemManager.Instance.itemEquipment.SelectEquipment(selectItemId);
            }
            else if (ItemManager.Instance.itemDB[selectItemId] is ArmorInfo)
            {

            }
        }
    }
}