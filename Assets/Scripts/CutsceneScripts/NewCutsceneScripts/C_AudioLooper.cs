using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ToDo: Make this loop better in the future, I think the idea is to use PlayDelayed() as this prompts the audio system to set the time directly rather than waiting for a Unity Update, which by design has big time gaps between calls
[AddComponentMenu("Cutscene/Audio Looper")]
public class C_AudioLooper : MonoBehaviour
{
    public float FadeSpeed = 1;

    private AudioSource _audioSource;

    private TimeLerp<float> _fadeLerp;
    private bool _fadingIn;

    private bool _started = false;

    private bool _beenEnabled = false;
    private bool _beenDisabled = false;



    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _fadeLerp = new TimeLerp<float>();
        _started = true;

        if (_beenEnabled)
        {
            OnEnable();
        }

        if (_beenDisabled)
        {
            OnDisable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    private void OnEnable()
    {
        if (_started)
        {
            _fadingIn = true;
            _fadeLerp.Prep(0, 1, FadeSpeed);
            StartCoroutine(FadeIn());
        }
        else
        {
            _beenEnabled = true;
        }
    }

    private void OnDisable()
    {
        if (_started)
        {
            _fadingIn = false;
            _fadeLerp.Prep(1, 0, FadeSpeed);
            StartCoroutine(FadeOut());
        }
        else
        {
            _beenDisabled = true;
        }
    }

    IEnumerator FadeIn()
    {
        while (_fadingIn)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }

            var currentFade = _fadeLerp.Go();
            _audioSource.volume = currentFade.position;

            if (currentFade.progress >= 1)
            {
                break;
            }

            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while (!_fadingIn)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }

            var currentFade = _fadeLerp.Go();
            _audioSource.volume = currentFade.position;

            if (currentFade.progress >= 1)
            {
                break;
            }

            yield return null;
        }
    }
}
