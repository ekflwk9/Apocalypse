using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUi : MonoBehaviour
{
    private Action endFarming;
    public List<FarmingData> farmingData = new List<FarmingData>();

    public SuccessWindow success { get => fieldSuccess; }
    [SerializeField] private SuccessWindow fieldSuccess;

    public DeadWindow dead { get => fieldDead; }
    [SerializeField] private DeadWindow fieldDead;

    public TMP_Text weightText { get => fieldWeightText; }
    [SerializeField] private TMP_Text fieldWeightText;

    public SelectUi drag { get => fieldDrag; }
    [SerializeField] private SelectUi fieldDrag;

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

    public ShopUi shop { get => fieldShop; }
    [SerializeField] private ShopUi fieldShop;

    [SerializeField] private InventorySlot[] inventorySlot;
    [SerializeField] private InventorySlot[] farminSlot;
    [SerializeField] private EquippedSlot[] equippedSlot;
    [SerializeField] private HandSlot[] handSlot;
    //[SerializeField] private InventorySlot[] storageSlot;

    private void Reset()
    {
        fieldDrag = this.TryFindChildComponent<SelectUi>();
        fieldItemInfo = this.TryFindChildComponent<ItemInfoUi>();
        fieldWeightText = this.TryFindChildComponent<TMP_Text>("WeightText");

        fieldInventory = this.TryFindChild("Inventory").gameObject;
        inventorySlot = GetComponentArray<InventorySlot>(fieldInventory.transform);

        fieldEquipped = this.TryFindChild("Equipped").gameObject;
        equippedSlot = GetComponentArray<EquippedSlot>(fieldEquipped.transform);

        fieldStorage = this.TryFindChild("Storage").gameObject;
        //storageSlot = GetComponentArray<InventorySlot>(fieldStorage.transform);

        fieldFarming = this.TryFindChild("Farming").gameObject;
        farminSlot = GetComponentArray<InventorySlot>(fieldFarming.transform);

        var handSlotPos = this.TryFindChild("Equipped");
        handSlot = GetComponentArray<HandSlot>(handSlotPos);

        fieldShop = this.TryFindChildComponent<ShopUi>("Shop");
        fieldDead = this.TryFindChildComponent<DeadWindow>();
        fieldSuccess = this.TryFindChildComponent<SuccessWindow>();
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
    /// 아이템 획득 성공 여부 (성공시 아이템 추가됨)
    /// </summary>
    /// <param name="_itemId"></param>
    /// <returns></returns>
    public bool GetItem(int _itemId)
    {
        var item = ItemManager.Instance.GetItem(_itemId);

        //중복 획득 가능한 아이템인가?
        if (item.canStack)
        {
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                //슬롯에 아이템이 없을 경우
                if (inventorySlot[i].itemId == 0)
                {
                    inventorySlot[i].SetSlot(_itemId, 1);
                    return true;
                }

                //중복된 아이템이 있을 경우 / 최대 갯수를 넘지 않았을 경우
                else if (inventorySlot[i].itemId == _itemId && inventorySlot[i].count < item.maxStack)
                {
                    inventorySlot[i].SetSlot(inventorySlot[i].count + 1);
                    return true;
                }
            }
        }

        //불가능시
        else
        {
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                if (inventorySlot[i].itemId == 0)
                {
                    inventorySlot[i].SetSlot(_itemId, 1);
                    return true;
                }
            }
        }

        return false;
    }

    public void ResetInventory()
    {
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            inventorySlot[i].SetSlot(0);
        }
    }

    public void SetWeightText(int _weight)
    {
        //Player.Instance.Weight
        //weightText.text = $"{}";
    }

    /// <summary>
    /// 현재 캐비넷이 가지고 있는 데이터와 파밍이 끝났을 경우 호출될 메서드
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_func"></param>
    public void SetFarming(List<FarmingData> _data, Action _func)
    {
        farmingData = _data;
        endFarming = _func;

        for (int i = 0; i < _data.Count; i++)
        {
            if (_data[i].id != 0)
            {
                farminSlot[_data[i].slotNumber].SetSlot(_data[i].id, _data[i].count);
            }
        }
    }

    public void UpdateSlot()
    {
        endFarming();
    }
}
