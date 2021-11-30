using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleVolume : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterGroup = null;
    [SerializeField] private string mixerName = "MasterVolume";
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private Toggle audioToggle;

    private float targetLevel = 0f;

    void Start()
    {
        targetLevel = PlayerPrefs.GetFloat(mixerName, 0f);
        Toggle();
    }

    public void Toggle()
    {
        if (audioToggle.isOn) { FadeIn(); } else { FadeOut(); }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeToTarget(targetLevel));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeToTarget(-80f));
    }

    public void FadeToVolumeLevel(float volumeLevel)
    {
        StopAllCoroutines();
        if (volumeLevel > targetLevel)
        {
            volumeLevel = targetLevel;
        }
        StartCoroutine(FadeToTarget(volumeLevel));
    }

    private IEnumerator FadeToTarget(float targetVolume)
    {
        masterGroup.audioMixer.GetFloat(mixerName, out float currentVolume);
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float newVolume = Mathf.SmoothStep(currentVolume, targetVolume, elapsedTime / fadeTime);
            masterGroup.audioMixer.SetFloat(mixerName, newVolume);
            yield return new WaitForEndOfFrame();
        }

        masterGroup.audioMixer.SetFloat(mixerName, targetVolume);
    }
}
