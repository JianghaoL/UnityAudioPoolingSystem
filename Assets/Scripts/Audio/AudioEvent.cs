using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioEvent : MonoBehaviour, IPoolable
{
    public event Action OnActive;
    public event Action OnDeactive;
    
    private AudioSource _audioSource;
    private float _volume = 1f;


    private Example _e;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnSpawn() // Currently empty implementation
    {}

    public void OnDespawn() // Currently empty implementation
    {
        _audioSource.loop = false;
        _audioSource.volume = 1f;
        _audioSource.pitch = 1f;
        _audioSource.spatialBlend = 1f;
        _audioSource.outputAudioMixerGroup = null;
    }

    /// <summary>
    /// Play audio at specified location.
    /// </summary>
    /// <param name="clip">Audio clip to play</param>
    /// <param name="pos">Where the audio should be placed</param>
    /// <param name="loop">Default to non-looping</param>
    /// <param name="volume">Default to full volume</param>
    /// <param name="pitch">Default to original pitch</param>
    /// <param name="spatialBlend">Default to 3D, 0f for 2D</param>
    /// <param name="mixerGroup">Default to null</param>
    public void Play(AudioClip clip,
        Vector3 pos,
        bool loop = false,
        float volume = 1f,
        float pitch = 1f,
        float spatialBlend = 1f,
        AudioMixerGroup mixerGroup = null)
    {
        _audioSource.clip = clip;
        _audioSource.loop = loop;
        _audioSource.volume = volume;
        _audioSource.pitch = pitch;
        _audioSource.spatialBlend = spatialBlend;
        _audioSource.outputAudioMixerGroup = mixerGroup;
        
        transform.position = pos;
        _audioSource.Play();
        
        _volume = _audioSource.volume;
        
        OnActive?.Invoke();
    }

    /// <summary>
    /// Stop the audio source from playing
    /// Do NOT use it directly
    /// Use AudioManager.Instance.Stop(AudioEvent e)
    /// </summary>
    public void Stop()
    {
        _audioSource.Stop();
        OnDeactive?.Invoke();
        
        
        // This is for debug and showcase
        // Delete for future use
        _e.Reset();
    }
    
    /// <summary>
    /// Get current audio clip
    /// </summary>
    /// <returns></returns>
    public AudioClip GetAudioClip() => _audioSource.clip;
    
    /// <summary>
    /// Get current position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition() => transform.position;
    
    /// <summary>
    /// Get current volume
    /// </summary>
    /// <returns></returns>
    public float GetVolume() => _volume;
    
    /// <summary>
    /// Get current playing state
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying() => _audioSource.isPlaying;



    
    // ----------------------------------------------------------------------
    // Below are for debug and showcase.
    // Delete for future use
    public void SetExample(Example example)
    {
        _e = example;
    }

    public void DebugF()
    {
        _e.Reset();
    }
}
