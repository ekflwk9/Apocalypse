using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new ObjectPool();
            }
            return _instance;
        }
    }

    //                                    자기 자신 프리팹            객체 가져올 때 호출하는 함수
    public GameObject Get(GameObject _prefab)
    {
        poolDictionary.TryGetValue(_prefab.name, out Queue<GameObject> objectQueue);

        if (objectQueue != null && objectQueue.Count > 0)
        {
            GameObject obj = objectQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = MonoBehaviour.Instantiate(_prefab);
            newObj.SetActive(true);
            return newObj;
        }
    }


    //                    여기에 본인 프리팹                  자기자신 객체        죽었을 때 호출해야하는 함수
    public void Set(GameObject _prefab, GameObject _gameObject)
    {
        _gameObject.SetActive(false);
        if (true == poolDictionary.TryGetValue(_prefab.name, out Queue<GameObject> objectQueue))
        {
            objectQueue.Enqueue(_gameObject);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(_gameObject);
            poolDictionary.Add(_prefab.name, newQueue);
        }
    }
}
