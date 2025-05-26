using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    /// <summary>
    /// ã����� �ڽ� ������Ʈ�� Transform�� ��ȯ��
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_findName"></param>
    /// <returns></returns>
    public static Transform FindChild(Transform _parent, string _findName)
    {
        var child = TryFindChild(_parent, _findName);
        if (child == null) DebugHelper.Log($"{_parent.name}�� {_findName}��� �ڽ� ������Ʈ�� �������� ����");

        return child;
    }

    private static Transform TryFindChild(Transform _parent, string _findName)
    {
        //Ư�� �̸��� �ڽ��� ã�� ��� �޼���
        Transform findCHild = null;

        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            findCHild = child.name == _findName ? child : TryFindChild(child, _findName);
            if (findCHild != null) break;
        }

        return findCHild;
    }
}