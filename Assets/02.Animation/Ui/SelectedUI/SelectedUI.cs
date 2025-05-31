using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUI : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Canvas MyCanvas;

    private void Reset()
    {
        MyCanvas = GetComponent<Canvas>();
    }

    public void On() => gameObject.SetActive(true);

    public void Off() => gameObject.SetActive(false);

}
