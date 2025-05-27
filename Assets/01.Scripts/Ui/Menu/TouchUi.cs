using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchUi : MonoBehaviour
{
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<RectTransform>(out var target)) pos = target;
        else DebugHelper.ShowBugWindow($"{this.name}�� RectTransform�� �������� ����");
    }

    /// <summary>
    /// Ư�� Ui ��ġ�� ����� ����
    /// </summary>
    /// <param name="_ui"></param>
    /// <param name="_isActive"></param>
    public void SetTouch(RectTransform _ui, bool _isActive)
    {
        pos.transform.position = _ui.position;
        pos.sizeDelta = _ui.rect.size;

        this.gameObject.SetActive(_isActive);
    }

    /// <summary>
    /// Ȱ��ȭ ���θ� ����
    /// </summary>
    /// <param name="_ui"></param>
    /// <param name="_isActive"></param>
    public void SetTouch(bool _isActive)
    {
        this.gameObject.SetActive(_isActive);
    }
}
