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
        else DebugHelper.ShowBugWindow($"{this.name}�� Animator�� �������� ����");
    }

    /// <summary>
    /// ���̵� �� => �ݹ� �޼��� ȣ�� & ���ǵ� ����
    /// </summary>
    /// <param name="_fadeFunc"></param>
    public void OnFede(Action _fadeFunc, float _fadeSpeed = 1f)
    {
        activeSelf = true;
        fadeFunc = _fadeFunc;
        anim.Play("FadeIn", 0, 0);
        anim.SetFloat("Speed", _fadeSpeed);
    }

    /// <summary>
    /// ���̵� �ƿ� & ���ǵ� ����
    /// </summary>
    public void OnFade(float _fadeSpeed = 1f)
    {
        anim.SetFloat("Speed", _fadeSpeed);

        if (fadeFunc != null)
        {
            fadeFunc();
            fadeFunc = null;
        }
    }

    private void EndFade()
    {
        //�ִϸ��̼� �̺�Ʈ ȣ�� ���� �޼���
        activeSelf = false;
    }
}
