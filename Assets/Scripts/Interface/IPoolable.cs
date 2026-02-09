using UnityEngine;
/// <summary>
/// Implement this class should an object be poolable
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// Called when this object is activated from a pool
    /// </summary>
    void OnSpawn();
    
    /// <summary>
    /// Called when this object is deactivated and queued
    /// </summary>
    void OnDespawn();
}
