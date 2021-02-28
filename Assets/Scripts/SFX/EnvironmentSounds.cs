using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnvironmentSounds : MonoBehaviour
{

    [SerializeField] private EnvironmentSound[] environmentSounds;
    [SerializeField] private float minCooldown;
    [SerializeField] private float maxCooldown;

    private AudioSource audioSource;
    private float nextSoundTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        nextSoundTime = Time.time + GenerateCooldown();
    }

    private void Update()
    {
        if (Time.time < nextSoundTime)
            return;
        nextSoundTime = Time.time + GenerateCooldown();
        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        int i = Random.Range(0, environmentSounds.Length);
        audioSource.clip = environmentSounds[i].AudioClip;
        audioSource.volume = Random.Range(environmentSounds[i].minVolume, environmentSounds[i].maxVolume);
        audioSource.pitch = Random.Range(environmentSounds[i].minPitch, environmentSounds[i].maxPitch);
        audioSource.Play();
    }

    private float GenerateCooldown()
    {
        return Random.Range(minCooldown, maxCooldown);
    }

    [System.Serializable]
    private struct EnvironmentSound
    {

        public AudioClip AudioClip;
        public float minPitch;
        public float maxPitch;
        public float minVolume;
        public float maxVolume;

    }

}