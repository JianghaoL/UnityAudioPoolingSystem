using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Generic class for object pool implementation
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _defaultTransform;
    private readonly int _maxNumber;
    
    private Queue<T> _allObjects;
    
    /// <summary>
    /// Constructor to create a new objectPool object.
    /// </summary>
    /// <param name="prefab">Object to be pooled</param>
    /// <param name="maxNumber">How many objects should be in the pool</param>
    /// <param name="defaultTransform">where to spawn the object if more needed</param>
    /// <exception cref="Exception">maxNumber is negative</exception>
    public ObjectPool(T prefab, int maxNumber, Transform defaultTransform = null)
    {
        if (maxNumber < 0) throw new Exception("Object Pool : Max number must be positive");
        
        _allObjects = new Queue<T>();
        _prefab = prefab;
        _defaultTransform = defaultTransform;
        _maxNumber = maxNumber;

        for (int i = 0; i < _maxNumber; i++)
        {
            AddObject();
        }
    }

    /// <summary>
    /// Get an object from the pool
    /// </summary>
    /// <returns>Object of designated type
    /// Null if max number reached</returns>
    /// <exception cref="Exception">Object pool not initialized</exception>
    public T Get()
    {
        T obj;
        if (_allObjects == null) throw new Exception("Object Pool : Object pool is not initialized");

        if (_allObjects.Count > 0) obj = _allObjects.Dequeue();
        else if (Count() < _maxNumber) obj = AddObject();
        else return null;
        
        obj.gameObject.SetActive(true);
        (obj as IPoolable)?.OnSpawn();
        return obj;
    }

    /// <summary>
    /// Release an object back to the object pool
    /// </summary>
    /// <param name="obj"></param>
    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        (obj as IPoolable)?.OnDespawn();
        _allObjects.Enqueue(obj);
    }
    
    /// <summary>
    /// Get the current number of objects in the pool
    /// </summary>
    /// <returns></returns>
    public int Count() => _allObjects.Count;
    
    
    
    /// <summary>
    /// Creates and enqueue an object
    /// </summary>
    /// <returns></returns>
    private T AddObject()
    {
        T obj = Object.Instantiate(_prefab);
        obj.gameObject.SetActive(false);
        
        _allObjects.Enqueue(obj);
        return obj;
    }
}
