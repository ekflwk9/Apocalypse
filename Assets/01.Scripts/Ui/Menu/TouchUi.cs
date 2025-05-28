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
        else DebugHelper.ShowBugWindow($"{this.name}에 RectTransform가 존재하지 않음");
    }

    /// <summary>
    /// 특정 Ui 위치와 사이즈를 맞춤
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
    /// 활성화 여부만 설정
    /// </summary>
    /// <param name="_ui"></param>
    /// <param name="_isActive"></param>
    public void SetTouch(bool _isActive)
    {
        this.gameObject.SetActive(_isActive);
    }
}
