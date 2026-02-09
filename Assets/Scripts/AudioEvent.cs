using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioEvent : MonoBehaviour, IPoolable
{
    private AudioSource _audioListener;
    private float volume = 1f;
    private void Awake()
    {
        _audioListener = GetComponent<AudioSource>();
    }

    public void OnSpawn() // Currently empty implementation
    {}

    public void OnDespawn() // Currently empty implementation
    {}

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
        _audioListener.clip = clip;
        _audioListener.loop = loop;
        _audioListener.volume = volume;
        _audioListener.pitch = pitch;
        _audioListener.spatialBlend = spatialBlend;
        _audioListener.outputAudioMixerGroup = mixerGroup;
        
        transform.position = pos;
        _audioListener.Play();
        
        this.volume = _audioListener.volume;
    }

    /// <summary>
    /// Stop the audio source from playing
    /// Do NOT use it directly
    /// Use AudioManager.Instance.Stop(AudioEvent e)
    /// </summary>
    public void Stop()
    {
        _audioListener.Stop();
    }
    
    /// <summary>
    /// Get current audio clip
    /// </summary>
    /// <returns></returns>
    public AudioClip GetAudioClip() => _audioListener.clip;
    
    /// <summary>
    /// Get current position
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition() => transform.position;
    
    /// <summary>
    /// Get current volume
    /// </summary>
    /// <returns></returns>
    public float GetVolume() => volume;
}
