using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    private Collider[] colliders;
    private ItemHandler _closestItem;

    private void Update()
    {
        colliders = Physics.OverlapBox(transform.position, transform.localScale);
    }

    public void InvokePickUp()
    {
        var playerPosition = Player.Instance.transform.position;
        float distance = -1;
        foreach (var item in colliders)
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