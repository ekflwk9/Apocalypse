using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) UiManager.instance.status.GetItem(301);
        else if (Input.GetKeyDown(KeyCode.H)) UiManager.instance.status.GetItem(202);
        else if (Input.GetKeyDown(KeyCode.J)) UiManager.instance.status.GetItem(203);
    }
}