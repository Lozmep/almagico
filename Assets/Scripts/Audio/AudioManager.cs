using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Audio[] audios;
    public static AudioManager Instance;

    float volume, lastVolumeValue, volSaved, volumeValue;

    public bool isPlaying;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }

        volSaved = PlayerPrefs.GetFloat("Volume");

        foreach (Audio a in audios)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.audioFile;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
    }

    public void Start()
    {
        volumeSlider.value = volSaved;
        volumeValue = volumeSlider.value;
        ChangeVolume();
        //Play("Musica");
        //Play("Tractor");
        
    }

    private void Update()
    {
        
        volumeValue = volumeSlider.value;
        if(volumeValue != lastVolumeValue)
        {
            ChangeVolume();
            SaveVol();
        }

        lastVolumeValue = volumeValue;

        StartCoroutine(PlaySound("Ambiente1"));
    }

    public void Play(string name)
    {
        Audio a = Array.Find(audios, audios => audios.name == name);

        if(a == null)
        {
            Debug.LogWarning("Nulo");
            return;
        }
        isPlaying = true;
        a.source.Play();
    }

    public void Stop(string name)
    {
        Audio a = Array.Find(audios, audios => audios.name == name);

        if (a == null)
        {
            Debug.LogWarning("Nulo");
            return;
        }
        isPlaying = false;
        a.source.Stop();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeValue;
    }

    public void SaveVol()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    private IEnumerator PlaySound(String songName)
    {
        Play(songName);
        yield return new WaitForSeconds(30f);
    }

}
