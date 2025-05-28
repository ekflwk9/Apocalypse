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
        ItemManager.Instance.PickUp(itemInfo.itemId);
        this.gameObject.SetActive(false);
    }

    public void DropItem()
    {
        this.transform.position = PlayerTransform.position;
        this.gameObject.SetActive(true);
    }
}