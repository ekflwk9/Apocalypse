using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchUi : MonoBehaviour
{
    private void SetTouch(Transform _ui, bool _isActive)
    {
        this.transform.position = _ui.position;
        this.transform.localScale = _ui.localScale;

        this.gameObject.SetActive(_isActive);
    }
}
