using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private Volume ppVolume;
    [SerializeField] private TMPro.TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle ppfxToggle;

    private void Start()
    {
        if (ppVolume == null) ppVolume = FindObjectOfType<Volume>();
        int qualityLevel = PlayerPrefs.GetInt("QualitySettings", 1);
        bool postProcessingToggled = PlayerPrefs.GetInt("ppfxEnabled", 1) == 1;

        if (qualityDropdown != null) qualityDropdown.SetValueWithoutNotify(qualityLevel);
        if (ppfxToggle != null) ppfxToggle.isOn = postProcessingToggled;

        QualitySettings.SetQualityLevel(qualityLevel, true);
        TogglePostProcessing(postProcessingToggled);
    }

    public void UpdateQualityWithDropdown()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value, true);
        PlayerPrefs.SetInt("QualitySettings", qualityDropdown.value);
        if (qualityDropdown != null) qualityDropdown.SetValueWithoutNotify(qualityDropdown.value);
    }

    public void UpdatePFXWithToggle()
    {
        PlayerPrefs.SetInt("ppfxEnabled", ppfxToggle.isOn ? 1 : 0);
        ppVolume.enabled = ppfxToggle.isOn;
    }

    public void TogglePostProcessing(bool ppfxEnabled)
    {
        ppVolume.enabled = ppfxEnabled;
        PlayerPrefs.SetInt("ppfxEnabled", ppfxEnabled ? 1 : 0);
        if (ppfxToggle != null) ppfxToggle.isOn = ppfxEnabled;
    }
}
