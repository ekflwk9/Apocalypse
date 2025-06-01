using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUi : MonoBehaviour
{
    private IInteractionObject interactionObj;

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

    public ShopUi firstShop { get => fieldFirstShop; }
    [SerializeField] private ShopUi fieldFirstShop;

    public ShopUi secondShop { get => fieldSecondShop; }
    [SerializeField] private ShopUi fieldSecondShop;

    [SerializeField] private InventorySlot[] inventorySlot;
    [SerializeField] private FarmingSlot[] farmingSlot;
    [SerializeField] private EquippedSlot[] equippedSlot;
    [SerializeField] private HandSlot[] handSlot;

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
        farmingSlot = GetComponentArray<FarmingSlot>(fieldFarming.transform);

        var handSlotPos = this.TryFindChild("Equipped");
        handSlot = GetComponentArray<HandSlot>(handSlotPos);

        fieldFirstShop = this.TryFindChildComponent<ShopUi>("FirstShop");
        fieldSecondShop = this.TryFindChildComponent<ShopUi>("SecondShop");
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

    /// <summary>
    /// 현재 캐비넷이 가지고 있는 데이터와 파밍이 끝났을 경우 호출될 메서드
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_func"></param>
    public void SetFarming(List<FarmingData> _data, IInteractionObject _interactionObj)
    {
        for (int i = 0; i < _data.Count; i++)
        {
            farmingSlot[_data[i].slotNumber].SetSlot(_data[i].id, _data[i].count);
        }

        interactionObj = _interactionObj;
        farmingData.Clear();
    }

    /// <summary>
    /// 파밍 메서드로 등록된 메서드 호출 (데이터 업데이트)
    /// </summary>
    public void UpdateFarmingSlot()
    {
        if (farming.activeSelf)
        {
            for (int i = 0; i < farmingSlot.Length; i++)
            {
                if (farmingSlot[i].itemId != 0)
                {
                    var itmeData = new FarmingData(farmingSlot[i].itemId, farmingSlot[i].count, i);

                    farmingData.Add(itmeData);
                    farmingSlot[i].SetSlot(0);
                }
            }

            if (interactionObj != null) interactionObj.Interaction();
            else DebugHelper.Log("EndFarming 메서드가 추가되지 않은 상태");
        }
    }

    public bool Remove(ItemInfo _item)
    {
        var hand = ItemManager.Instance.Inventory;
        var index = hand.firstSlotItem == _item ? 0 : 1;

        var count = handSlot[index].count - 1;
        handSlot[index].SetSlot(count);

        if (count == 0) return true;
        else return false;
    }

    public void HideArmorView(int _index)
    {
        equippedSlot[_index].SetSlot(0);
    }
}
