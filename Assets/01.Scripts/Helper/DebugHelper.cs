using UnityEditor;
using UnityEngine;

public class DebugHelper
{
    /// <summary>
    /// 에디터 전용 알림창을 띄우는 메서드 => 출력 내용 입력
    /// </summary>
    /// <param name="_message"></param>
    public static void ShowBugWindow(string _message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("Editor 알림", _message, "확인");
#endif
    }

    /// <summary>
    /// 에디터 전용 로그가 찍히는 메서드
    /// </summary>
    /// <param name="_text"></param>
    public static void Log(string _text)
    {
#if UNITY_EDITOR
        Debug.Log(_text);
#endif
    }
}
