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
    private List<FarmingData> item = new List<FarmingData>();
    private BoxCollider _collider;

    void Reset()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public void Interaction()
    {
        var status = UiManager.instance.status;

        status.SetFarming(item, UpdateData);
        status.farming.gameObject.SetActive(true);
        status.inventory.gameObject.SetActive(true);
        status.equipped.gameObject.SetActive(true);
    }

    public void UpdateData()
    {
        item = UiManager.instance.status.farmingData;
    }

    public void OnSelected()
    {

    }

    public void UnSelected()
    {

    }
}