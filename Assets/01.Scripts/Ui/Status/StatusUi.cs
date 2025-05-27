using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatusUi : MonoBehaviour
{
    public DragUi drag { get => fieldDrag; }
    [SerializeField] private DragUi fieldDrag;

    public ItemInfoUi itemInfo { get => fieldItemInfo; }
    [SerializeField] private ItemInfoUi fieldItemInfo;

    public GameObject inventory { get => fieldInventory; }
    [SerializeField] private GameObject fieldInventory;

    public GameObject equipped { get => fieldEquipped; }
    [SerializeField] private GameObject fieldEquipped;

    public GameObject storage { get => fieldStorage; }
    [SerializeField] private GameObject fieldStorage;

    public GameObject farming { get => fieldFarming; }
    [SerializeField] private GameObject fieldFarming;

    public GameObject shop { get => fieldShop; }
    [SerializeField] private GameObject fieldShop;

    [SerializeField] private InventorySlot[] inventorySlot;
    [SerializeField] private InventorySlot[] storageSlot;
    [SerializeField] private InventorySlot[] farminSlot;
    [SerializeField] private EquippedSlot[] equippedSlot;
    [SerializeField] private ShopSlot[] shopSlot;

    private void Reset()
    {
        fieldDrag = this.GetComponentInChildren<DragUi>(true);
        if (fieldDrag == null) DebugHelper.Log($"{this.name}�� DragImage��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldItemInfo = this.GetComponentInChildren<ItemInfoUi>(true);
        if (fieldItemInfo == null) DebugHelper.Log($"{this.name}�� ItemInfoUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldItemInfo = this.GetComponentInChildren<ItemInfoUi>(true);
        if (fieldItemInfo == null) DebugHelper.Log($"{this.name}�� ItemInfoUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldInventory = Helper.FindChild(this.transform, "Inventory").gameObject;
        inventorySlot = GetComponentArray<InventorySlot>(fieldInventory.transform);

        fieldEquipped = Helper.FindChild(this.transform, "Equipped").gameObject;
        equippedSlot = GetComponentArray<EquippedSlot>(fieldEquipped.transform);

        fieldStorage = Helper.FindChild(this.transform, "Storage").gameObject;
        storageSlot = GetComponentArray<InventorySlot>(fieldStorage.transform);

        fieldFarming = Helper.FindChild(this.transform, "Farming").gameObject;
        farminSlot = GetComponentArray<InventorySlot>(fieldFarming.transform);

        fieldShop = Helper.FindChild(this.transform, "Shop").gameObject;
        shopSlot = GetComponentArray<ShopSlot>(fieldShop.transform);
    }

    private T[] GetComponentArray<T>(Transform _parent) where T : class
    {
        var childCount = _parent.childCount;
        var tempList = new List<T>();

        for (int i = 0; i < childCount; i++)
        {
            var child = _parent.GetChild(i);

            if (child.TryGetComponent<T>(out var target)) tempList.Add(target);
        }

        var tempArray = new T[tempList.Count];
        tempList.CopyTo(0, tempArray, 0, tempList.Count);

        return tempArray;
    }

    /// <summary>
    /// ������ ȹ�� ���� ���� (������ ������ �߰���)
    /// </summary>
    /// <param name="_itemId"></param>
    /// <returns></returns>
    public bool GetItem(int _itemId)
    {
        var item = ItemManager.Instance.itemDB[_itemId];

        //�ߺ� ȹ�� ������ �������ΰ�?
        if (item.canStack)
        {
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].itemId == 0)
                {
                    inventorySlot[i].SetItem(_itemId);
                    return true;
                }

                else if (inventorySlot[i].itemId == _itemId)
                {
                    inventorySlot[i].SetItemCount(_itemId);
                    return true;
                }
            }
        }

        //�Ұ��ɽ�
        else
        {
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].itemId == 0)
                {
                    inventorySlot[i].SetItem(_itemId);
                    return true;
                }
            }
        }

        return false;
    }
}
