using System.Collections;
using UnityEngine;

/// <summary>
/// This class is for demonstration
/// </summary>
public class Example : MonoBehaviour
{
    public AudioClip clip;
    
    [Space(10)]
    public bool loop = false;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0f, 2f)]
    public float pitch = 1.0f;
    [Range(0f, 1f)]
    public float spatialBlend = 1.0f;
    
    [Space(10)]
    public Material playingMaterial;

    private bool _isPlaying = false;
    private AudioEvent _e;
    private Material _defaultMaterial;

    private void Awake()
    {
        _defaultMaterial = GetComponent<Renderer>().material;
    }
    
    private void OnMouseDown()
    {
        if (_isPlaying)
        {
            AudioManager.Instance.Stop(_e);
            GetComponent<Renderer>().material = _defaultMaterial;
            _isPlaying = false;
        }
        else
        {
            _e = AudioManager.Instance.Play(clip, transform.position, loop, volume, pitch, spatialBlend);
            _e.SetExample(this);
            GetComponent<Renderer>().material = playingMaterial;
            _isPlaying = true;
        }
        
        if (!loop) StartCoroutine(OneShot());
    }

    public void Reset()
    {
        GetComponent<Renderer>().material = _defaultMaterial;
    }

    private IEnumerator OneShot()
    {
        yield return new WaitForSecondsRealtime(clip.length);
        GetComponent<Renderer>().material = _defaultMaterial;
    }
}
