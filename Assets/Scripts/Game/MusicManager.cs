using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    
    public static MusicManager Instance { get; private set; }

    public event Action OnMusicEndedCallback;

    private const float muteSpeed = 0.4f;
    private const float unmuteSpeed = 1f;

    [SerializeField] private AudioClip tutorialMusic;
    [SerializeField] private AudioClip jackTheme;
    [SerializeField] private AudioClip[] combatPlaylist;

    private AudioSource audioSource;
    private float callbackTime;
    private bool shouldCallback;
    private bool isMuted;
    private AudioClip[] playlist;
    private float nextMusicTime;
    private bool isPlaylistMode;

    public void PlayMenuMusic() => PlayLoop(tutorialMusic);
    public void PlayTutorialMusic() => PlayLoop(tutorialMusic);
    public void PlayJackTheme() => PlayLoop(jackTheme);
    public void PlayDeathMusic() => PlayLoop(jackTheme);
    public void PlayCombatPlaylist() => Play(combatPlaylist);

    public void PlayOnce(AudioClip music)
    {
        //shouldCallback = true;
        //callbackTime = Time.time + music.length;
        isPlaylistMode = false;
        shouldCallback = false;
        audioSource.loop = false;
        playlist = null;
        if (music != audioSource.clip)
            StartCoroutine(ChangeMusic(music));
    }

    public void PlayLoop(AudioClip music)
    {
        isPlaylistMode = false;
        shouldCallback = false;
        audioSource.loop = true;
        playlist = null;
        if (music != audioSource.clip)
            StartCoroutine(ChangeMusic(music));
    }

    public void Play(AudioClip[] playlist)
    {
        shouldCallback = false;
        audioSource.loop = false;
        this.playlist = playlist;
        if (!isPlaylistMode)
            PlaylistNext();
        isPlaylistMode = true;
    }

    public void Stop()
    {
        isMuted = true;
        shouldCallback = false;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playlist != null && Time.realtimeSinceStartup > nextMusicTime)
            PlaylistNext();

        if (isMuted)
        {
            if (audioSource.volume > 0f)
                audioSource.volume -= Time.unscaledDeltaTime * unmuteSpeed;
        }
        else
        {
            if (audioSource.volume < 1f)
                audioSource.volume += Time.unscaledDeltaTime * muteSpeed;
        }
        if (!shouldCallback || Time.realtimeSinceStartup < callbackTime)
            return;
        shouldCallback = false;
        OnMusicEndedCallback?.Invoke();
    }

    private void PlaylistNext()
    {
        AudioClip nextMusic = GetRandomMusicFromPlaylist();
        nextMusicTime = Time.realtimeSinceStartup + nextMusic.length - 1f / muteSpeed;
        StartCoroutine(ChangeMusic(nextMusic));
    }

    private AudioClip GetRandomMusicFromPlaylist()
    {
        return playlist[UnityEngine.Random.Range(0, playlist.Length)];
    }

    private IEnumerator ChangeMusic(AudioClip newMusic)
    {
        if (!audioSource.clip)
        {
            audioSource.clip = newMusic;
            audioSource.volume = 1f;
            audioSource.Play();
            isMuted = false;
            yield break;
        }

        isMuted = true;

        yield return new WaitUntil(() => audioSource.volume == 0f);

        audioSource.clip = newMusic;
        audioSource.Play();
        isMuted = false;
    }

}