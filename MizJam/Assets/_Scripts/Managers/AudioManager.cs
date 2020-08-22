using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource soundSource;

    public void Initialize() {
        
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f) {
        soundSource.PlayOneShot(clip, volume * soundSource.volume);
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
