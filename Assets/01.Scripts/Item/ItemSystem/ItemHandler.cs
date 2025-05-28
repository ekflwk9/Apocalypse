using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemHandler : MonoBehaviour
{
    public ItemInfo itemInfo;
    private Transform playerTransform;

    private void OnEnable()
    {
        if (playerTransform == null)
        {
            playerTransform = Player.Instance.transform;
        }
    }

    public void PickUpItem() // 아이템 주을때 호출 (인풋시스템 연동 예정)
    {
        ItemManager.Instance.PickUp(itemInfo.itemId);
        this.gameObject.SetActive(false);
    }

    public void DropItem()
    {
        if (playerTransform == null)
        { playerTransform = Player.Instance.transform; }

        this.transform.position = playerTransform.position;
        this.gameObject.SetActive(true);
    }
}