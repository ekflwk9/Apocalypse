using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitShader : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Reset()
    {
        if (this.TryGetComponent<Animator>(out var target)) anim = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 Animator가 존재하지 않음");
    }

    public void Show(bool _isActive)
    {
        if(!this.gameObject.activeSelf) this.gameObject.SetActive(true);
        anim.Play(_isActive ? "Show" : "Hide", 0, 0);
    }
}
