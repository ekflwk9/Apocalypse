using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemHandler : MonoBehaviour
{
    public ItemInfo itemInfo;
    private Transform _playerTransform;
    private Transform PlayerTransform => _playerTransform ??= Player.Instance.transform;

    public void PickUpItem() // 아이템 주을때 호출 (인풋시스템 연동 예정)
    {
        if (UiManager.instance.status.GetItem(itemInfo.itemId))
        {
            ItemManager.Instance.Inventory.GetItem(itemInfo.itemId);   // 이거를 하면 아이템이 들어오나요?
            ObjectPool.Instance.Set(itemInfo.itemPrefab, this.gameObject);
        }
        // 이후 주울 수 있는지 else문 작성해야함
    }

    public void DropItem()
    {
        GameObject gameObject = ObjectPool.Instance.Get(itemInfo.itemPrefab);
        gameObject.transform.position = PlayerTransform.position;
    }
}