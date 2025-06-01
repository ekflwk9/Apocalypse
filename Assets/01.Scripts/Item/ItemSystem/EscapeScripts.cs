using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeScripts : MonoBehaviour, IInteractionObject
{

    [SerializeField] Collider _collider;
    [SerializeField] SelectedUI selectedUI;
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
        if (selectedUI != null)
        {
            selectedUI.On();
        }
    }
    public void UnSelected()
    {
        if (selectedUI != null)
        {
            selectedUI.Off();
        }
    }
}
