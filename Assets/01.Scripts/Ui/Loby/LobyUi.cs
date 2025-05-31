using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobyUi : MonoBehaviour
{
    public GameObject title { get => fieldTitle; }
    [SerializeField] private GameObject fieldTitle;

    private void Reset()
    {
        fieldTitle = this.TryFindChild("LobyTitle").gameObject;
    }
}
