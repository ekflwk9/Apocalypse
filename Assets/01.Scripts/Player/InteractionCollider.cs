using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    [SerializeField] private List<ItemHandler> items;
    private ItemHandler _closestItem;

    public void InvokePickUp()
    {
        Vector3 playerPosition = Player.Instance.transform.position;
        float distance = -1;
        foreach (ItemHandler item in items)
        {
            if (distance == -1)
            {
                distance = Vector3.Distance(playerPosition, item.transform.position);
                _closestItem = item;
                continue;
            }
            float curDistance = Vector3.Distance(playerPosition, item.transform.position);
            if (distance > curDistance)
            {
                distance = curDistance;
                _closestItem = item;
            }
        }

        _closestItem.PickUpItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ItemHandler item))
        {
            items.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemHandler item))
        {
            items.Remove(item);
        }
    }
}
