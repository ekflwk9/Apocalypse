using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusUi : MonoBehaviour
{
    public DragUi drag { get => fieldDrag; }
    [SerializeField] private DragUi fieldDrag;

    public ItemInfoUi itemInfo { get => fieldItemInfo; }
    [SerializeField] private ItemInfoUi fieldItemInfo;
    public GameObject inventory { get => fieldInventory; }
    [SerializeField] private GameObject fieldInventory;

    public GameObject storage { get => fieldStorage; }
    [SerializeField] private GameObject fieldStorage;

    public GameObject farming { get => fieldFarming; }
    [SerializeField] private GameObject fieldFarming;

    private Slot[] inventorySlot;
    private Slot[] storageSlot;
    private Slot[] statusSlot;

    private void Reset()
    {
        fieldDrag = this.GetComponentInChildren<DragUi>(true);
        if (fieldDrag == null) DebugHelper.Log($"{this.name}�� DragImage��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldItemInfo = this.GetComponentInChildren<ItemInfoUi>(true);
        if (fieldItemInfo == null) DebugHelper.Log($"{this.name}�� ItemInfoUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldInventory = Helper.FindChild(this.transform, "PlayerInventory").gameObject;
        Find(fieldInventory.transform, out statusSlot);

        fieldStorage = Helper.FindChild(this.transform, "Storage").gameObject;
        Find(fieldStorage.transform, out statusSlot);

        fieldFarming = Helper.FindChild(this.transform, "Farming").gameObject;
        Find(fieldFarming.transform, out statusSlot);
    }

    private void Find(Transform _parent, out Slot[] _slotArray)
    {
        var childCount = _parent.childCount;
        var tempList = new List<Slot>();

        for (int i = 0; i < childCount; i++)
        {
            var child = _parent.GetChild(i);
            if (child.TryGetComponent<Slot>(out var target)) tempList.Add(target);
        }

        _slotArray = new Slot[tempList.Count];
        tempList.CopyTo(0, _slotArray, 0, tempList.Count);
    }
}
