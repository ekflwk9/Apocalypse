using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUi : MonoBehaviour
{


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
    //[SerializeField] private InventorySlot[] storageSlot;

    private void Reset()
    {
        var weight = Helper.FindChild(this.transform, "WeightText");
        if(weight.TryGetComponent<TMP_Text>(out var isWeight)) fieldWeightText = isWeight;
        if (fieldWeightText == null) DebugHelper.Log($"{this.name}에 DragImage스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldDrag = this.GetComponentInChildren<SelectUi>(true);
        if (fieldDrag == null) DebugHelper.Log($"{this.name}에 DragImage스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldItemInfo = this.GetComponentInChildren<ItemInfoUi>(true);
        if (fieldItemInfo == null) DebugHelper.Log($"{this.name}에 ItemInfoUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldInventory = Helper.FindChild(this.transform, "Inventory").gameObject;
        inventorySlot = GetComponentArray<InventorySlot>(fieldInventory.transform);

        fieldEquipped = Helper.FindChild(this.transform, "Equipped").gameObject;
        equippedSlot = GetComponentArray<EquippedSlot>(fieldEquipped.transform);

        fieldStorage = Helper.FindChild(this.transform, "Storage").gameObject;
        //storageSlot = GetComponentArray<InventorySlot>(fieldStorage.transform);

        fieldFarming = Helper.FindChild(this.transform, "Farming").gameObject;
        farminSlot = GetComponentArray<InventorySlot>(fieldFarming.transform);

        var shopPos = Helper.FindChild(this.transform, "Shop");
        if (shopPos.TryGetComponent<ShopUi>(out var isShop)) fieldShop = isShop;
        else DebugHelper.ShowBugWindow($"{shopPos.name}에 스크립트가 있는 자식 오브젝트가 존재하지 않음");
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
        var item = ItemManager.Instance.itemDB[_itemId];

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

    public bool GetFarmingItem(int _itemId)
    {
        var item = ItemManager.Instance.itemDB[_itemId];

        //중복 획득 가능한 아이템인가?
        if (item.canStack)
        {
            for (int i = 0; i < farminSlot.Length; i++)
            {
                //슬롯에 아이템이 없을 경우
                if (farminSlot[i].itemId == 0)
                {
                    farminSlot[i].SetSlot(_itemId, 1);
                    return true;
                }

                //중복된 아이템이 있을 경우 / 최대 갯수를 넘지 않았을 경우
                else if (farminSlot[i].itemId == _itemId && farminSlot[i].count < item.maxStack)
                {
                    farminSlot[i].SetSlot(farminSlot[i].count + 1);
                    return true;
                }
            }
        }

        //불가능시
        else
        {
            for (int i = 0; i < farminSlot.Length; i++)
            {
                if (farminSlot[i].itemId == 0)
                {
                    farminSlot[i].SetSlot(_itemId, 1);
                    return true;
                }
            }
        }

        return false;
    }


    public void SetWeightText(int _weight)
    {
        //Player.Instance.Weight
        //weightText.text = $"{}";
    }
}
