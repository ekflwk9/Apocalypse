using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractionObject
{
    public void Interaction();
}


public class InteractionObject : MonoBehaviour, IInteractionObject
{
    List<int> ItemKeys = new List<int>();


    public void Awake()
    {
    }

    public void Start()
    {
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

}
