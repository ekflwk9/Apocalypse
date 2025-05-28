using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    [SerializeField] private List<ItemHandler> items;
    private ItemHandler _closestItem;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Weapon"))
        {
            items.Add(other.GetComponent<ItemHandler>());
            Debug.Log("Item 찾음");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemHandler item)) items.Remove(item);
    }

    public void InvokePickUp()
    {
        var playerPosition = Player.Instance.transform.position;
        float distance = -1;
        foreach (var item in items)
        {
            if (distance == -1)
            {
                distance = Vector3.Distance(playerPosition, item.transform.position);
                _closestItem = item;
                continue;
            }

            var curDistance = Vector3.Distance(playerPosition, item.transform.position);
            if (distance > curDistance)
            {
                distance = curDistance;
                _closestItem = item;
            }
        }

        if (_closestItem != null) _closestItem.PickUpItem();
    }
}