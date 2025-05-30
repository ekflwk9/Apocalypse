using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionObject
{
    public void Interaction();
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
    List<FarmingData> ItemKeys = new List<FarmingData>();

    public void Interaction()
    {
        foreach (var item in ItemKeys)
        {
            UiManager.instance.status.GetFarmingItem(item.id);
        }

        UiManager.instance.status.farming.gameObject.SetActive(true);
        UiManager.instance.status.inventory.gameObject.SetActive(true);
        UiManager.instance.status.equipped.gameObject.SetActive(true);
    }

    public void UpdateData()
    {

    }
}