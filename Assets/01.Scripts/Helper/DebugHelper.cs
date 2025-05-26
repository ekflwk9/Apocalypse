using UnityEditor;
using UnityEngine;

public class DebugHelper
{
    /// <summary>
    /// ������ ���� �˸�â�� ���� �޼��� => ��� ���� �Է�
    /// </summary>
    /// <param name="_message"></param>
    public static void ShowBugWindow(string _message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("Editor �˸�", _message, "Ȯ��");
#endif
    }

    /// <summary>
    /// ������ ���� �αװ� ������ �޼���
    /// </summary>
    /// <param name="_text"></param>
    public static void Log(string _text)
    {
#if UNITY_EDITOR
        Debug.Log(_text);
#endif
    }
}
