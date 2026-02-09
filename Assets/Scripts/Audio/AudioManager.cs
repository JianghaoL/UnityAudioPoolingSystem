using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("General Settings")]
    public AudioConfig config;
    public AudioEvent audioEventPrefab;

    // Utilities variables
    private AudioRecycleStrategy _recycleStrategy;
    private ObjectPool<AudioEvent> _audioEventPool;
    private List<AudioEvent> _activeEvents;
    private AudioListener _audioListener;
    
    // Recycle related
    private Dictionary<AudioEvent, float> _playStartTimes = new Dictionary<AudioEvent, float>();
    
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("AudioManager : Multiple instances of AudioManager");
        }
        
        _recycleStrategy = config.recycleStrategy;
        _audioListener = FindAnyObjectByType<AudioListener>();
        _audioEventPool = new ObjectPool<AudioEvent>(audioEventPrefab, config.GetMaxNumber(), transform);
        _activeEvents = new List<AudioEvent>();
    }

    
    
    
    /// <summary>
    /// Play an audio at specified location
    /// </summary>
    /// <param name="clip">Audio clip to play</param>
    /// <param name="position">Where the audio should be placed</param>
    /// <param name="loop">Default to non-looping</param>
    /// <param name="volume">Default to full volume</param>
    /// <param name="pitch">Default to original pitch</param>
    /// <param name="spatialBlend">Default to 3D, 0f for 2D</param>
    /// <param name="mixerGroup">Default to null</param>
    /// <returns>AudioEvent, available for stopping</returns>
    public AudioEvent Play(
        AudioClip clip,
        Vector3 position,
        bool loop = false,
        float volume = 1f,
        float pitch = 1f,
        float spatialBlend = 1f,
        AudioMixerGroup mixerGroup = null)
    {
        if (_activeEvents.Count >= config.GetMaxNumber()) ApplyRecycle();
        
        AudioEvent audioEvent = _audioEventPool.Get();
        if (!audioEvent)
        {
            Debug.Log("AudioManager : Object pool reached maximum");
            return null;
        }
        
        audioEvent.Play(clip, position, loop, volume, pitch, spatialBlend, mixerGroup);
        _activeEvents.Add(audioEvent);
        _playStartTimes[audioEvent] = Time.time;
        
        if (!loop) StartCoroutine(ReleaseNonLoopingAudio(audioEvent));
        
        return audioEvent;
    }
    
    
    
    /// <summary>
    /// Stop a given audio event from playing
    /// </summary>
    /// <param name="audioEvent"></param>
    public void Stop(AudioEvent audioEvent)
    {
        audioEvent.Stop();
        ReleaseAudioEvent(audioEvent);
    }
    
    
    
    /// <summary>
    /// Apply recycle according to given strategy.
    /// It is recommended to let AudioManager handle all audio.
    /// This is for manually recycling audio events
    /// </summary>
    public void ApplyRecycle()
    {
        if (_activeEvents.Count == 0) return;

        AudioEvent target = null;

        switch (_recycleStrategy)
        {
            case AudioRecycleStrategy.ByTime:
                var earliest = _playStartTimes.Aggregate((a, b) => a.Value < b.Value ? a : b);
                target = earliest.Key;
                //Debug.Log($"Audio Manager : Audio Event Recycled : Strategy = {_recycleStrategy} : Time = {earliest.Value}");
                break;
            case AudioRecycleStrategy.ByVolume:
                target = _activeEvents
                    .OrderBy(ev => ev.GetVolume())
                    .FirstOrDefault();
                //Debug.Log($"Audio Manager : Audio Event Recycled : Strategy = {_recycleStrategy} : Volume = {target.GetVolume()}");
                break;
            case AudioRecycleStrategy.ByDistance:
                var listenerPos = _audioListener.transform.position;
                target = _activeEvents
                    .OrderBy(ev => Vector3.Distance(ev.GetPosition(), listenerPos))
                    .LastOrDefault();
                //Debug.Log($"Audio Manager : Audio Event Recycled : Strategy = {_recycleStrategy} : Distance = {Vector3.Distance(target.GetPosition(), listenerPos)}");
                break;
        }
        
        if (target) Stop(target);
    }


    /// <summary>
    /// Set the current recycle strategy at run time
    /// </summary>
    /// <param name="strategy"></param>
    public void ResetStrategy(AudioRecycleStrategy strategy)
    {
        _recycleStrategy = strategy;
    }

    /// <summary>
    /// Reset the current recycle strategy to config
    /// </summary>
    public void ResetStrategy()
    {
        _recycleStrategy = config.recycleStrategy;
    }
    
    
    
    
    
    
    private IEnumerator ReleaseNonLoopingAudio(AudioEvent audioEvent)
    {
        yield return new WaitForSecondsRealtime(audioEvent.GetAudioClip().length);
        ReleaseAudioEvent(audioEvent);
    }

    private void ReleaseAudioEvent(AudioEvent audioEvent)
    {
        _activeEvents.Remove(audioEvent);
        _playStartTimes.Remove(audioEvent);
        _audioEventPool.Release(audioEvent);
    }
}
