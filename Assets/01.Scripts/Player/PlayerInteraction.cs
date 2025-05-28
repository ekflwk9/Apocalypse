using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    [SerializeField] private ItemHandler closestItem;
    [SerializeField] private Vector3 halfExtents;
    
    private void Update()
    {
        colliders = Physics.OverlapBox(transform.position, halfExtents);
        foreach (Collider col in colliders)
        {
            Debug.Log(col.name);
        }
    }

    public void InvokePickUp()
    {
        var playerPosition = Player.Instance.transform.position;
        bool isFirstItem = true;
        float distance = 0f;
        foreach (var item in colliders)
        {
            if (isFirstItem)
            {
                distance = Vector3.Distance(playerPosition, item.transform.position);
                closestItem = item.gameObject.GetComponent<ItemHandler>();
                isFirstItem = false;
                continue;
            }

            var curDistance = Vector3.Distance(playerPosition, item.transform.position);
            if (distance > curDistance)
            {
                distance = curDistance;
                closestItem = item.gameObject.GetComponent<ItemHandler>();
            }
        }

        if (closestItem != null) closestItem.PickUpItem();
    }
}