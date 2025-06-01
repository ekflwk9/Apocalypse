using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeScripts : MonoBehaviour, IInteractionObject
{

    Collider _collider;

    private void Reset()
    {
        _collider = GetComponent<Collider>();

        gameObject.tag = TagHelper.Item;
        gameObject.layer = LayerHelper.InitLayer(LayerHelper.Item);
    }

    public void Interaction()
    {
        UiManager.instance.status.success.gameObject.SetActive(true);
    }

    public void OnSelected()
    {

    }

    public void UnSelected()
    {

    }
}
