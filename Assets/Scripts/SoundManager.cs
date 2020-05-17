using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoundManager
{
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip combatMusic;
    [SerializeField] AudioClip chaseMusic;
    [SerializeField] AudioClip weaponDrawn;

    AudioSource audioSource;

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

    public void PlayAudio(AudioClip audioClip)
    {
        if (!CompareAudio(audioClip))
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void PlayBGM()
    {
        PlayAudio(backgroundMusic);
    }

    public void PlayChaseMusic()
    {
        PlayAudio(chaseMusic);
    }

    public void PlayCombatMusic()
    {
        PlayAudio(combatMusic);
    }

    public void WeaponDrawn()
    {
        PlayAudio(weaponDrawn);
    }

    public bool CompareAudio(AudioClip newAudioClip)
    {
        return newAudioClip == audioSource.clip;
    }
}
