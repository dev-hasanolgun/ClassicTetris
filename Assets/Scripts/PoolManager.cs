using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance => _instance;
    
    private readonly Dictionary<string, Queue<Component>> s_poolDictionary = new Dictionary<string, Queue<Component>>();

    public void PoolObject<T>(string poolName, T objectToPool)
    {
        if (s_poolDictionary.TryGetValue(poolName, out _))
        {
            s_poolDictionary[poolName].Enqueue(objectToPool as Component);
        }
        else
        {
            s_poolDictionary.Add(poolName, new Queue<Component>(new List<Component> {objectToPool as Component}));
        }
    }

    public T GetObjectFromPool<T>(string poolName, T objectToGet) where T : class, IPoolable
    {
        if (s_poolDictionary.TryGetValue(poolName, out var thisQueue))
        {
            if (thisQueue.Count == 0)
            {
                return CreateNewObject(objectToGet);
            }

            var obj = thisQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj as T;
        }
        return CreateNewObject(objectToGet);
    }

    public T CreateNewObject<T>(T objectToCreate) where T : class, IPoolable
    {
        var obj = Instantiate(objectToCreate as Component);
        obj.name = (objectToCreate as Component)?.name ?? "poolObj";
        return obj as T;
    }
    public bool TryGetObjectFromPool<T>(string poolName, out T value, bool isPeeking = false) where T : class, IPoolable
    {
        if (!s_poolDictionary.TryGetValue(poolName, out var thisQueue))
        {
            value = default;
            return false;
        }
        if (!(s_poolDictionary[poolName].Count > 0))
        {
            value = default;
            return false;
        }

        value = (!isPeeking ? thisQueue.Dequeue() : thisQueue.Peek()) as T;
        return true;
    }

    public void ClearPool()
    {
        s_poolDictionary.Clear();
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
