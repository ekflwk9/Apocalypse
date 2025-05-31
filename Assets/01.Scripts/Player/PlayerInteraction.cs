using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Dictionary<string, GameObject> touchItems = new Dictionary<string, GameObject>();
    private IInteractionObject touchedItem;
    private bool isTouched;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private LayerMask itemLayer;

    private void Reset()
    {
        interactionCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            RaycastHit hit;
            
            isTouched = true;
            
            var origin = transform.position;
            var direction = other.transform.position - origin;
            
            if (Physics.Raycast(origin, direction, out hit, 5f, itemLayer))
            {
                if (hit.collider.CompareTag("Item") && hit.collider.TryGetComponent<IInteractionObject>(out var isItem))
                {
                    isTouched = true;
                    touchedItem = isItem;
                    touchedItem.OnSelected();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            isTouched = false;
            touchedItem.UnSelected();
            touchedItem = null;
        }
    }

    public void Interaction()
    {
        touchedItem?.Interaction();
    }


    public void InvokePickUp()
    {
        if (isTouched && touchedItem != null)
        {
            interactionCollider.enabled = false;
            touchedItem?.Interaction();
            touchedItem = null;
            interactionCollider.enabled = true;
        }
    }
}