using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourComparer : IEqualityComparer<MonoBehaviour>
{
    public bool Equals(MonoBehaviour x, MonoBehaviour y)
    {
        if (true == ReferenceEquals(x, y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(MonoBehaviour obj)
    {
        if (obj == null) return 0;
        return obj.GetHashCode();
    }
}

public class CoroutineManager : MonoBehaviour   // 작동은 함 오류
{
    private static CoroutineManager instance;

    public static CoroutineManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("CoroutineManager");
                instance = go.AddComponent<CoroutineManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private static Dictionary<MonoBehaviour, HashSet<Coroutine>> CoroutineDictionary = new Dictionary<MonoBehaviour, HashSet<Coroutine>>(new MonoBehaviourComparer());

    public Coroutine SetCoroutine(MonoBehaviour _Mono, IEnumerator _coroutine)
    {
        if (true == CoroutineDictionary.TryGetValue(_Mono, out HashSet<Coroutine> Hash))
        {
            Coroutine value = StartCoroutine(_coroutine);
            Hash.Add(value);
            return value;
        }
        else
        {
            CoroutineDictionary[_Mono] = new HashSet<Coroutine>();
            Coroutine value = StartCoroutine(_coroutine);
            CoroutineDictionary[_Mono].Add(value);
            return value;
        }
    }

    public void UnSetCoroutine(MonoBehaviour _Mono, Coroutine _Coroutine)
    {
        if (true == CoroutineDictionary.TryGetValue(_Mono, out HashSet<Coroutine> Hash))
        {
            if(true == Hash.TryGetValue(_Coroutine, out Coroutine routine))
            {
                StopCoroutine(routine);
                Hash.Remove(routine);
            }
        }
        _Coroutine = null;
    }

    public void UnSetAllCoroutine(MonoBehaviour _Mono)
    {
        if (true == CoroutineDictionary.TryGetValue(_Mono, out HashSet<Coroutine> Hash))
        {
            foreach (Coroutine routine in Hash)
            {
                StopCoroutine(routine);
            }
            CoroutineDictionary.Remove(_Mono);
        }
    }
}