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
        fieldMenu = this.TryFindChild("Menu").gameObject;
        fieldExit = this.TryFindChild("ExitWindow").gameObject;
    }
}
