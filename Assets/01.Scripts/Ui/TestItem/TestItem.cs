using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) UiManager.instance.status.GetItem(100);
        else if(Input.GetKeyDown(KeyCode.H)) UiManager.instance.status.GetItem(200);
    }
}
