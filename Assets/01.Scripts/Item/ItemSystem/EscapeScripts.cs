using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeScripts : MonoBehaviour, IInteractionObject
{
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
