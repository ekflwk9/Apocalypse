using System.Collections;
using System.Collections.Generic;
using GameItem;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemHandler : MonoBehaviour
{
    public int itemId;
    private bool isPlayerInRange;
    private bool canPickUp;
    private Transform playerTransform;

    private void OnEnable()
    {
        if (playerTransform == null)
        {
            //playerTransform = CharacterManager.Instance.Player.transform;
        }
    }

    private void OnDisable()
    {
        isPlayerInRange = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // playerTransform = CharacterManager.Instance.Player.transform;
        }
        else if (other.CompareTag("PlayerInsight"))
        {
            canPickUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerTransform = null;
        }
        else if (other.CompareTag("PlayerInsight"))
        {
            canPickUp = false;
        }
    }

    public void Init(ItemInfo itemInfo)
    {

    }

    public void PickUpItem() // 아이템 주을때 호출 (인풋시스템 연동 예정)
    {
        if (isPlayerInRange && canPickUp)
        {
            ItemManager.Instance.PickUp(itemId);
            this.gameObject.SetActive(false);
        }
    }

    public void DropItem()
    {
        if (playerTransform == null)
            // { playerTransform = CharacterManager.Instance.Player.transform; }

            this.transform.position = playerTransform.position;
        this.gameObject.SetActive(true);
    }
}