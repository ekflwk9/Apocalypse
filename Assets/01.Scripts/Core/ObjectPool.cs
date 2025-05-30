using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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

    // 가져올 때
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

    // 죽었을 때
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
