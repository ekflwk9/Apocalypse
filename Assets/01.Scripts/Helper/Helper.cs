using UnityEngine;

public static class Helper
{
    /// <summary>
    /// GetComponent를 시도하는 예외처리 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T TryGetComponent<T>(this MonoBehaviour _thisPos) where T : class
    {
        if(_thisPos.TryGetComponent<T>(out var component)) return component;
        else DebugHelper.Log($"{_thisPos.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return null;
    }

    /// <summary>
    /// 자식 오브젝트에 특정 스크립트를 찾는 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <returns></returns>
    public static T TryFindChildComponent<T>(this MonoBehaviour _parent) where T : class
    {
        var component = _parent.GetComponentInChildren<T>(true);
        if (component == null) DebugHelper.Log($"{_parent.name} 자식오브젝트 중에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return component;
    }

    /// <summary>
    /// 특정 자식의 컴포넌트를 찾는 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <param name="_childName"></param>
    public static T TryFindChildComponent<T>(this MonoBehaviour _parent, string _childName) where T : class
    {
        var child = FindChild(_parent.transform, _childName);

        if (child != null)
        {
            if (child.TryGetComponent<T>(out var component))
            {
                return component;
            }

            else
            {
                DebugHelper.Log($"{_parent.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");
            }
        }

        else
        {
            DebugHelper.Log($"{_parent.name}에 {_childName}라는 자식 오브젝트는 존재하지 않음");
        }

        return null;
    }

    /// <summary>
    /// 찾고싶은 자식 오브젝트의 Transform을 반환함
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_findName"></param>
    /// <returns></returns>
    public static Transform TryFindChild(this MonoBehaviour _parent, string _findName)
    {
        var child = FindChild(_parent.transform, _findName);
        if (child == null) DebugHelper.Log($"{_parent.name}에 {_findName}라는 자식 오브젝트가 존재하지 않음");

        return child;
    }

    private static Transform FindChild(Transform _parent, string _findName)
    {
        //특정 이름의 자식을 찾는 재귀 메서드
        Transform findChild = null;

        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            findChild = child.name == _findName ? child : FindChild(child, _findName);
            if (findChild != null) break;
        }

        return findChild;
    }
}