using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource musicSource;

    public float sfxVolume;
    public float musicVolume;
    public float vibration;
    

    private void Awake()
    {
        instance = this;
        musicSource = GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey("SfxVolume"))
        {
            PlayerPrefs.SetFloat("SfxVolume", 1);
            PlayerPrefs.SetFloat("MusicVolume", 1);
            PlayerPrefs.SetFloat("Vibration", 1);
        }

        sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        vibration = PlayerPrefs.GetFloat("Vibration");
        musicSource.volume = musicVolume;
    }

    void Start()
    {

    }

    public void Vibrate()
    {
#if !UNITY_WEBGL
        if (vibration != 0)
        {
            //Handheld.Vibrate();
        }
#endif
    }
    
    
    public void PlaySfx(string name)
    {

        Addressables.LoadAssetAsync<AudioClip>(name).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                AudioSource.PlayClipAtPoint(handle.Result, Camera.main.transform.position, sfxVolume);
            }
        };
    }

    
    public void PlayMusic()
    {
        if (musicSource == null) return;

        musicSource.Play();
    }
    public void PlayMusic(string name, bool loop=true)
    {
        Addressables.LoadAssetAsync<AudioClip>(name).Completed += handle =>
        {
            if (musicSource == null) return;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                musicSource.clip = handle.Result;
                musicSource.time = 0;
                musicSource.loop = loop;
                musicSource.Play();
            }
        };
    }
    public void PauseMusic()
    {
        if (musicSource == null) return;
        
        if (musicSource.isPlaying)
            musicSource.Pause();

    }
    public void StopMusic()
    {
        if (musicSource == null) return;

        if (musicSource.isPlaying)
            musicSource.Stop();
    }
    
    public bool isMusicPlaying()
    {
        return musicSource.isPlaying;
    }
    public int GetCurrentTime()
    {
        return (int)musicSource.time;
    }
    public AudioClip GetClip()
    {
        return musicSource.clip;
    }


    #region VolumeSetters

    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("SfxVolume", volume);
        sfxVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        if (musicSource == null) return;

        PlayerPrefs.SetFloat("MusicVolume", volume);
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }
    public void SetVibration(float volume)
    {
        PlayerPrefs.SetFloat("Vibration", volume);
        vibration = volume;
    }
    #endregion

}