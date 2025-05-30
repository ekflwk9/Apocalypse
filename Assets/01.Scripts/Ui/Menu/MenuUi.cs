using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUi : MonoBehaviour
{
    public GameObject menuWindow { get => fieldMenu; }
    [SerializeField] private GameObject fieldMenu;

    public GameObject exitWindow { get => fieldExit; }
    [SerializeField] private GameObject fieldExit;

    private void Reset()
    {
        fieldMenu = Helper.FindChild(this.transform, "Menu").gameObject;
        fieldExit = Helper.FindChild(this.transform, "ExitWindow").gameObject;
    }
}
