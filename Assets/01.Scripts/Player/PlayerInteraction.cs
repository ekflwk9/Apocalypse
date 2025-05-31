using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private LinkedList<IInteractionObject> touchedInteractions = new LinkedList<IInteractionObject>();
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
            if (true == other.TryGetComponent<IInteractionObject>(out var isItem))
            {

                LinkedListNode<IInteractionObject> InteractionableNode = touchedInteractions.AddFirst(isItem);
                isItem.OnSelected();

                if (null != InteractionableNode.Next)
                {
                    InteractionableNode.Next.Value.UnSelected();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            IInteractionObject Interactionable = other.GetComponent<IInteractionObject>();
            if (Interactionable != null)
            {
                Interactionable.UnSelected();
                touchedInteractions.Remove(Interactionable);
            }
        }
    }

    public void Interaction()
    {
        LinkedListNode<IInteractionObject> InteractionableNode = touchedInteractions.First;
        if (InteractionableNode == null)
        {
            return;
        }
        else
        {
            if (null != InteractionableNode.Next)
            {
                InteractionableNode.Next.Value.OnSelected();
            }
            IInteractionObject Interactionable = InteractionableNode.Value;
            Interactionable.UnSelected();
            Interactionable.Interaction();
            touchedInteractions.Remove(InteractionableNode);
        }
    }
}