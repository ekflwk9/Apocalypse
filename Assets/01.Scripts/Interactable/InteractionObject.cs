using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionObject
{
    public void Interaction();

    public void OnSelected();
    public void UnSelected();
}

public struct FarmingData
{

    public int id { get; private set; }
    public int count { get; private set; }
    public int slotNumber { get; private set; }

    public FarmingData(int _id, int _count, int _slotNumber)
    {
        id = _id;
        count = _count;
        slotNumber = _slotNumber;

    }



}


public class InteractionObject : MonoBehaviour, IInteractionObject
{
    //List<FarmingData> ItemKeys = new List<FarmingData>();

    List<int> ItemKeys = new List<int>();
    BoxCollider _collider;

    void Reset()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        ItemInfo[] infos = ItemManager.Instance.GetRandomItems(5);

        foreach (ItemInfo info in infos)
        {
            ItemKeys.Add(info.itemId);
        }
    }


    public void Interaction()
    {
        foreach (var item in ItemKeys)
        {
            UiManager.instance.status.GetFarmingItem(item);
        }

        UiManager.instance.status.farming.gameObject.SetActive(true);
        UiManager.instance.status.inventory.gameObject.SetActive(true);
        UiManager.instance.status.equipped.gameObject.SetActive(true);
    }


    public void UpdateData()
    {

    }


    public void OnSelected()
    {

    }

    public void UnSelected()
    {

    }
}

