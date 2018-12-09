using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    static SoundManager _instance;

    const float MaxVolume_BGM = 1.0f;
    const float MaxVolume_SFX = 0.1f;
    static float CurrentVolumeNormalized_BGM = 1.0f;
    static float CurrentVolumeNormalized_SFX = 1.0f;
    static bool isMuted = false;

    List<AudioSource> sfxSources;
    AudioSource bgmSource;

    public static SoundManager GetInstance() {
        if (!_instance) {
            GameObject soundManager = new GameObject("SoundManager");
            _instance = soundManager.AddComponent<SoundManager>();
            _instance.Initialize();
        }
        return _instance;
    }

    void Initialize() {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = GetBGMVolume();
        DontDestroyOnLoad(gameObject);
    }

    // Volume Getters
    static float GetBGMVolume() {
        return isMuted ? 0.0f : MaxVolume_BGM * CurrentVolumeNormalized_BGM;
    }
    public static float GetSFXVolume() {
        return isMuted ? 0.0f : MaxVolume_SFX * CurrentVolumeNormalized_SFX;
    }

    //BGM Utils
    void FadeBGMOut(float fadeDuration) {
        SoundManager soundMan = GetInstance();
        float delay = 0.0f;
        float toVolume = 0.0f;

        soundMan.StartCoroutine(FadeBGM(toVolume, delay, fadeDuration));
    }
    void FadeBGMIn(AudioClip bgmClip, float delay, float fadeDuration) {
        SoundManager soundMan = GetInstance();
        soundMan.bgmSource.clip = bgmClip;
        soundMan.bgmSource.Play();

        float toVolume = GetBGMVolume();

        soundMan.StartCoroutine(FadeBGM(toVolume, delay, fadeDuration));
    }
    IEnumerator FadeBGM(float fadeToVolume, float delay, float duration) {
        yield return new WaitForSeconds(delay);

        SoundManager soundMan = GetInstance();
        float elapsed = 0.0f;
        while(duration > 0) {
            float t = (elapsed / duration);
            float volume = Mathf.Lerp(0.0f, fadeToVolume, t);
            soundMan.bgmSource.volume = volume;

            elapsed += Time.deltaTime;
            yield return 0;
        }
    }

    //BGM Functions
    public static void PlayBGM(AudioClip bgmClip, bool fade, float fadeDuration) {
        SoundManager soundMan = GetInstance();
        if (fade) {
            if (soundMan.bgmSource.isPlaying) {
                soundMan.FadeBGMOut(fadeDuration / 2);
                soundMan.FadeBGMIn(bgmClip, fadeDuration / 2, fadeDuration / 2);
            }
            else {
                float delay = 0f;
                soundMan.FadeBGMIn(bgmClip, delay, fadeDuration);
            }
        }
        else {
            soundMan.bgmSource.volume = GetBGMVolume();
            soundMan.bgmSource.clip = bgmClip;
            soundMan.bgmSource.Play();
        }
    }
    public static void StopBGM(bool fade, float fadeDuration) {
        SoundManager soundMan = GetInstance();
        if (soundMan.bgmSource.isPlaying) {
            if (fade) {
                soundMan.FadeBGMOut(fadeDuration);
            }
            else {
                soundMan.bgmSource.Stop();
            }
        }
    }

    //SFX Utils
    AudioSource GetSFXSource() {
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = GetSFXVolume();

        if(sfxSources == null) {
            sfxSources = new List<AudioSource>();
        }
        sfxSources.Add(sfxSource);

        return sfxSource;
    }
    IEnumerator RemoveSFXSource(AudioSource sfxSource) {
        yield return new WaitForSeconds(sfxSource.clip.length);
        sfxSources.Remove(sfxSource);
        Destroy(sfxSource);
    }
    IEnumerator RemoveSFXSourceFixedLength(AudioSource sfxSource, float length) {
        yield return new WaitForSeconds(length);
        sfxSources.Remove(sfxSource);
        Destroy(sfxSource);
    }

    //SFX Functions
    public static void PlaySFX(AudioClip sfxClip) {
        SoundManager soundMan = GetInstance();
        AudioSource source = soundMan.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = sfxClip;
        source.Play();

        soundMan.StartCoroutine(soundMan.RemoveSFXSource(source));
    }
    public static void PlaySFX(AudioClip[] sfxClips) {
        AudioClip sfxClip = sfxClips[Random.Range(0, sfxClips.Length)];
        SoundManager soundMan = GetInstance();
        AudioSource source = soundMan.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = sfxClip;
        source.Play();

        soundMan.StartCoroutine(soundMan.RemoveSFXSource(source));
    }

    public static void PlaySFXRandomized(AudioClip sfxClip) {
        SoundManager soundMan = GetInstance();
        AudioSource source = soundMan.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = sfxClip;
        source.pitch = Random.Range(0.85f, 1.2f);
        source.Play();

        soundMan.StartCoroutine(soundMan.RemoveSFXSource(source));
    }
    public static void PlaySFXRandomized(AudioClip[] sfxClips) {
        AudioClip sfxClip = sfxClips[Random.Range(0, sfxClips.Length)];
        SoundManager soundMan = GetInstance();
        AudioSource source = soundMan.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = sfxClip;
        source.pitch = Random.Range(0.85f, 1.2f);
        source.Play();

        soundMan.StartCoroutine(soundMan.RemoveSFXSource(source));
    }

    public static void PlaySFXFixedDuration(AudioClip sfxClip, float duration, float volumeMultiplier = 1.0f) {
        SoundManager soundMan = GetInstance();
        AudioSource source = soundMan.GetSFXSource();
        source.volume = GetSFXVolume() * volumeMultiplier;
        source.clip = sfxClip;
        source.loop = true;
        source.Play();

        soundMan.StartCoroutine(soundMan.RemoveSFXSourceFixedLength(source, duration));
    }
    public static void PlaySFXFixedDuration(AudioClip[] sfxClips, float duration, float volumeMultiplier = 1.0f) {
        AudioClip sfxClip = sfxClips[Random.Range(0, sfxClips.Length)];
        SoundManager soundMan = GetInstance();
        AudioSource source = soundMan.GetSFXSource();
        source.volume = GetSFXVolume() * volumeMultiplier;
        source.clip = sfxClip;
        source.loop = true;
        source.Play();

        soundMan.StartCoroutine(soundMan.RemoveSFXSourceFixedLength(source, duration));
    }

    //Volume Control Functions
    public static void DisableSoundImmediate() {
        SoundManager soundMan = GetInstance();
        soundMan.StopAllCoroutines();
        if(soundMan.sfxSources != null) {
            foreach(AudioSource source in soundMan.sfxSources) {
                source.volume = 0.0f;
            }
        }
        soundMan.bgmSource.volume = 0.0f;
        isMuted = true;
    }
    public static void EnableSoundImmediate() {
        SoundManager soundMan = GetInstance();
        if (soundMan.sfxSources != null) {
            foreach (AudioSource source in soundMan.sfxSources) {
                source.volume = GetSFXVolume();
            }
        }
        soundMan.bgmSource.volume = GetBGMVolume();
        isMuted = false;
    }
    public static void SetGlobalVolume(float newVolume) {
        CurrentVolumeNormalized_SFX = newVolume;
        CurrentVolumeNormalized_BGM = newVolume;
        AdjustSoundImmediate();
    }
    public static void SetSFXVolume(float newVolume) {
        CurrentVolumeNormalized_SFX = newVolume;
        AdjustSoundImmediate();
    }
    public static void SetBGMVolume(float newVolume) {
        CurrentVolumeNormalized_BGM = newVolume;
        AdjustSoundImmediate();
    }
    public static void AdjustSoundImmediate() {
        SoundManager soundMan = GetInstance();
        if(soundMan.sfxSources != null) {
            foreach(AudioSource source in soundMan.sfxSources) {
                source.volume = GetSFXVolume();
            }
        }
        soundMan.bgmSource.volume = GetBGMVolume();
    }
}
