using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Scriptable Objects/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [Header("Audio Configuration")]
    [Tooltip("Select desired audio recycle strategy")]
    public AudioRecycleStrategy recycleStrategy;
    
    [Space(10)]
    [Tooltip("Specify the maximum number of audio allowed on different platforms")]
    public List<PlatformObjectNumber> objectPerPlatform;
    
    private Dictionary<RuntimePlatform, PlatformObjectNumber> _objectsPerPlatformDictionary;

    /// <summary>
    /// Get max number of audio objects per specific platforms
    /// Platform is automatically detected.
    /// </summary>
    /// <returns></returns>
    public int GetMaxNumber()
    {
        // Creates a dictionary for platform matching if not already.
        if (_objectsPerPlatformDictionary == null)
        {
            _objectsPerPlatformDictionary = new Dictionary<RuntimePlatform, PlatformObjectNumber>();

            foreach (var platformObject in objectPerPlatform)
            {
                _objectsPerPlatformDictionary.Add(platformObject.platform, platformObject);
            }
        }

        return _objectsPerPlatformDictionary[Application.platform].number;
    }
}

[Serializable]
public struct PlatformObjectNumber
{
    public RuntimePlatform platform;
    public int number;
}
