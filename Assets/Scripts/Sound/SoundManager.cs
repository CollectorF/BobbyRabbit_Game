using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
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

    internal void PlaySound(string type)
    {
        switch (type)
        {
            case "Bonus":
                bonus.Play();
                break;
            case "ButtonOnOff":
                button.Play();
                break;
            case "Carrot":
                carrot.Play();
                break;
            case "Win":
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
    internal void SetMusicVolume(float volume)
    {
        music.volume = Mathf.Clamp(volume, 0, 1);
    }
}
