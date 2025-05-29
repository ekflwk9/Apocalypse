using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Dictionary<string, GameObject> touchItems = new Dictionary<string, GameObject>();
    private ItemHandler touchedItem;
    private bool isTouched;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private Collider closestItem;
    [SerializeField] private float positionOffset;
    [SerializeField] private Vector3 halfExtents;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            RaycastHit hit;
            
            isTouched = true;
            
            var origin = transform.position;
            var direction = other.transform.position - origin;

            if (Physics.Raycast(origin, direction, out hit, 5f))
            {
                if (hit.collider.CompareTag("Item") && hit.collider.TryGetComponent<ItemHandler>(out var isItem))
                {
                    touchedItem = isItem;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            isTouched = false;
        }
    }

    public void InvokePickUp()
    {
        if (isTouched)
        {
            touchedItem?.PickUpItem();
        }
        // var playerPosition = Player.Instance.transform.position;
        // bool isFirstItem = true;
        // float distance = 0f;
        // foreach (var itemCollider in colliders)
        // {
        //     if (!touchItems.ContainsKey(itemCollider.gameObject.name))
        //     {
        //         touchItems.Add(itemCollider.gameObject.name, itemCollider.gameObject);
        //     }
        //     
        //     if (isFirstItem)
        //     {
        //         distance = Vector3.Distance(playerPosition, itemCollider.transform.position);
        //         closestItem = itemCollider;
        //         isFirstItem = false;
        //         continue;
        //     }
        //
        //     var curDistance = Vector3.Distance(playerPosition, itemCollider.transform.position);
        //     
        //     if (distance > curDistance)
        //     {
        //         distance = curDistance;
        //         closestItem = itemCollider;
        //     }
        // }
        //
        // if (closestItem != null)
        // {
        //     Debug.Log(closestItem);
        //     closestItem = null;
        // }
    }
}