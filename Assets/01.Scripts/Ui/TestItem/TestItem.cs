using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public int id;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)) UiManager.instance.status.GetItem(id);
    }
}
