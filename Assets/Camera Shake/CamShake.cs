using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private float ShakeIntensity = 10f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _perlin;

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        DialogueManager.shakeCam += ShakeCam;
    }

    private void OnDisable()
    {
        DialogueManager.shakeCam -= ShakeCam;
    }

    private void Start()
    {
        StopCam();
    }

    public void ShakeCam(float time)
    {
        _perlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_AmplitudeGain = ShakeIntensity;
        timer = time;
    }
    
    void StopCam()
    {
        _perlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_AmplitudeGain = 0f;
        timer = 0;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopCam();
            }
        }
    }
}
