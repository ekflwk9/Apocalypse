using System;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public bool activeSelf { get; private set; }

    private Action fadeFunc;
    [SerializeField] private Animator anim;

    private void Reset()
    {
        if (this.TryGetComponent<Animator>(out var fadeAnim)) anim = fadeAnim;
        else DebugHelper.ShowBugWindow($"{this.name}에 Animator가 존재하지 않음");
    }

    private void Start()
    { 
        anim.Play("FadeOut", 0, 0);
    }

    /// <summary>
    /// 페이드 인 => 콜백 메서드 호출 & 스피드 조절
    /// </summary>
    /// <param name="_fadeFunc"></param>
    public void OnFade(Action _fadeFunc, float _fadeSpeed = 1f)
    {
        activeSelf = true;
        fadeFunc = _fadeFunc;
        anim.Play("FadeIn", 0, 0);
        anim.SetFloat("Speed", _fadeSpeed);
    }

    /// <summary>
    /// 페이드 아웃 & 스피드 조절
    /// </summary>
    public void OnFade(float _fadeSpeed = 1f)
    {
        activeSelf = false;

        anim.Play("FadeOut", 0, 0);
        anim.SetFloat("Speed", _fadeSpeed);
    }

    private void EndFade()
    {
        //애니메이션 이벤트 호출 전용 메서드
        if (fadeFunc != null)
        {
            fadeFunc();
            fadeFunc = null;
        }
    }
}
