using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel;

    [SerializeField] private float shakeIntensity = 3f;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannel = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float shakeTime)
    {
        StartCoroutine(CamShake(shakeTime));
    }

    IEnumerator CamShake(float shakeTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeTime)
        {
            elapsedTime += Time.deltaTime;
            cinemachineBasicMultiChannel.m_AmplitudeGain = Mathf.SmoothStep(shakeIntensity, 0f, elapsedTime / shakeTime);
            yield return new WaitForEndOfFrame();
        }
        cinemachineBasicMultiChannel.m_AmplitudeGain = 0f;
    }
}
