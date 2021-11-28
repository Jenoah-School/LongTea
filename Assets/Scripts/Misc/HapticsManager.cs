using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticsManager : MonoBehaviour
{
    [SerializeField] private Toggle vibrationToggle;

    private bool canVibrate = true;

    private void Start()
    {
        Vibration.Init();
        canVibrate = PlayerPrefs.GetInt("hapticsEnabled", 1) == 1;
        if (vibrationToggle != null) vibrationToggle.isOn = canVibrate;
    }

    public void VeryQuickVibrate()
    {
        if (!canVibrate) return;
        Vibration.VibratePop();
    }

    public void QuickVibrate()
    {
        if (!canVibrate) return;
        Vibration.VibratePeek();
    }

    public void TripleVibrate()
    {
        if (!canVibrate) return;
        Vibration.VibrateNope();
    }

    public void ToggleVibration()
    {
        PlayerPrefs.SetInt("hapticsEnabled", vibrationToggle.isOn ? 1 : 0);
        canVibrate = vibrationToggle.isOn;
    }
}
