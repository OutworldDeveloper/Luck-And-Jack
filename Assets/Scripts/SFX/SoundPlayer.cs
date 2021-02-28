using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private AudioClip[] clips;

    public bool IsPlaying => audioSource.isPlaying;

    private AudioSource audioSource;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    public void PlaySound()
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }

}