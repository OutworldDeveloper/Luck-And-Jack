using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{

    public static void Shake(float intensity, float time)
    {
        foreach (var item in FindObjectsOfType<CinemachineShake>())
            item.ShakeCamera(intensity, time);
    }

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float shakeEndingTime;
    private bool isShaking;

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeEndingTime = Time.time + time;
        isShaking = true;
    }

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }

    private void Start()
    {
        Player.Luck.Damaged += Luck_Damaged;
    }

    private void OnDestroy()
    {
        Player.Luck.Damaged -= Luck_Damaged;
    }

    private void Luck_Damaged(int damage, int lastHealth, FlatVector direction)
    {
        ShakeCamera(2f, 0.15f);
    }

    private void Update()
    {
        if (!isShaking) return;
        if (Time.time >= shakeEndingTime)
        {
            isShaking = false;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }

}