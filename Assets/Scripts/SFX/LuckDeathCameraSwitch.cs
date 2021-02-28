using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LuckDeathCameraSwitch : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera gameplayCamera;
    [SerializeField] private CinemachineVirtualCamera deathCamera;

    private void Start()
    {
        Player.Luck.Died += Luck_Died;
    }

    private void OnDestroy()
    {
        Player.Luck.Died -= Luck_Died;
    }

    private void Luck_Died()
    {
        gameplayCamera.gameObject.SetActive(false);
        deathCamera.gameObject.SetActive(true);
    }

}