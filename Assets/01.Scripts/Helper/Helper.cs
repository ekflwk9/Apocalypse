using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    /// <summary>
    /// 찾고싶은 자식 오브젝트의 Transform을 반환함
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_findName"></param>
    /// <returns></returns>
    public static Transform FindChild(Transform _parent, string _findName)
    {
        var child = TryFindChild(_parent, _findName);
        if (child == null) DebugHelper.Log($"{_parent.name}에 {_findName}라는 자식 오브젝트가 존재하지 않음");

        return child;
    }

    private static Transform TryFindChild(Transform _parent, string _findName)
    {
        //특정 이름의 자식을 찾는 재귀 메서드
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