using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemHandler : MonoBehaviour, IInteractionObject
{
    public ItemInfo itemInfo;
    [SerializeField] SelectedUI selectedUI;

    private Transform _playerTransform;
    private Transform PlayerTransform => _playerTransform ??= Player.Instance.transform;

    void Reset()
    {
        selectedUI = GetComponent<SelectedUI>();
    }

    public void Interaction() // 아이템 주을때 호출 (인풋시스템 연동 예정)
    {
        if (UiManager.instance.status.GetItem(itemInfo.itemId))
        {
            ItemManager.Instance.Inventory.Add(itemInfo.itemId);
            ObjectPool.Instance.Set(itemInfo.itemPrefab, this.gameObject);
        }
        // 이후 주울 수 있는지 else문 작성해야함
    }

    public void OnSelected()
    {
        if (selectedUI != null)
        {
            selectedUI.On();
        }
    }
    public void UnSelected()
    {
        if (selectedUI != null)
        {
            selectedUI.Off();
        }
    }
    public void DropItem()
    {
        GameObject gameObject = ObjectPool.Instance.Get(itemInfo.itemPrefab);
        gameObject.transform.position = PlayerTransform.position;
    }
}