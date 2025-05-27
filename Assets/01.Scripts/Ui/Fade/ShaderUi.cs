using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderUi : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Reset()
    {
        if (this.TryGetComponent<Animator>(out var target)) anim = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 Animator가 존재하지 않음");
    }

    public void SetActive(bool _isActive)
    {
        this.gameObject.SetActive(_isActive);
        anim.Play(_isActive ? "Show" : "Idle", 0, 0);
    }
}
