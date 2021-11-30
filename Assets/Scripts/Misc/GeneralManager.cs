using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GeneralManager : MonoBehaviour
{
    [SerializeField] private Toggle cameraToggle;

    private bool cameraSmoothingOn = true;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera = null;
    private CinemachineComposer cmc;

    private void Start()
    {
        if (cinemachineCamera) cmc = cinemachineCamera.GetCinemachineComponent<CinemachineComposer>();
        cameraSmoothingOn = PlayerPrefs.GetInt("cameraSmoothing", 1) == 1;
        if (cameraToggle != null) cameraToggle.isOn = cameraSmoothingOn;
    }

    public void ToggleCameraSmoothing()
    {

        if (cameraToggle != null) cameraSmoothingOn = cameraToggle.isOn;
        PlayerPrefs.SetInt("cameraSmoothing", cameraToggle.isOn ? 1 : 0);
        if (cmc == null) return;
        if (cameraSmoothingOn)
        {
            cmc.m_HorizontalDamping = 0.5f;
            cmc.m_VerticalDamping = 0.5f;
            cmc.m_LookaheadTime = 0.05f;
        }
        else
        {
            cmc.m_HorizontalDamping = 0f;
            cmc.m_VerticalDamping = 0f;
            cmc.m_LookaheadTime = 0f;
        }
    }
}
