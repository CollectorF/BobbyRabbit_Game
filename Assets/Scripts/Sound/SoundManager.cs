using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    public AudioMixerGroup MasterVolume;
    public AudioMixerGroup MusicVolume;
    public AudioMixerGroup SFXVolume;

    [SerializeField]
    [Range(-20f, 20f)]
    private float allVolume = 0;
    [SerializeField]
    [Range(-20f, 20f)]
    private float musicVolume = 0;
    [SerializeField]
    [Range(-20f, 20f)]
    private float sfxVolume = 0;

    [Space(20)]
    [SerializeField]
    private AudioSource bonus;
    [SerializeField]
    private AudioSource button;
    [SerializeField]
    private AudioSource carrot;
    [SerializeField]
    private AudioSource win;
    [SerializeField]
    private AudioSource music;

    private float volume;


    private void Start()
    {
        mixer.SetFloat("Master", allVolume);
        mixer.SetFloat("Music", musicVolume);
        mixer.SetFloat("SFX", sfxVolume);
    }


    internal void PlaySound(TileType type)
    {
        switch (type)
        {
            case TileType.Bonus:
                bonus.Play();
                break;
            case TileType.ButtonOnOff:
                button.Play();
                break;
            case TileType.Carrot:
                carrot.Play();
                break;
            case TileType.FinishPoint:
                win.Play();
                break;
        }
    }


    internal void PlayMusic()
    {
        music.Play();
    }


    internal void StopMusic()
    {
        music.Stop();
    }

    internal float GetVolume(AudioMixerGroup group)
    {
        mixer.GetFloat(group.ToString(), out volume);
        return volume;
    }

    internal void SetVolume(AudioMixerGroup group, float volume)
    {
        mixer.SetFloat(group.ToString(), volume);
    }
}
