using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LuckLowHpAudio : MonoBehaviour
{

    [SerializeField] private int hpThreshold = 1;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = 0;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (Player.Luck.Health > hpThreshold || Player.Luck.IsDead)
        {
            if (audioSource.volume > 0)
                audioSource.volume -= Time.unscaledDeltaTime;
        }
        else
        {
            if (audioSource.volume < 1)
                audioSource.volume += Time.unscaledDeltaTime;
        }
    }

}