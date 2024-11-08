using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static GameObject instance;

    static private AudioManager sound;
    
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource soundEffects;

    [SerializeField] public AudioClip[] backgroundSong;
    [SerializeField] public AudioClip[] quickSound;

    private void Awake()
    {
        
        if (sound != null)
        {
            Destroy(this);
        }
        else
        {
            sound = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayMainMenuMusic();
        }
        else
        {
            PlayOverworldMusic();
        }
        
        
    }
    void Update()
    {

    }

    static public void PlayOverworldMusic()
    {
        if (sound != null)
        {
            if (sound.backgroundMusic != null)
            {
                sound.backgroundMusic.Stop();
                sound.backgroundMusic.clip = sound.backgroundSong[3];
                sound.backgroundMusic.Play();
            }
        }
        else
        {
            Debug.Log(sound.backgroundMusic.clip);

        }
        
    }

    static public void PlayBattleMusic()
    {
        if (sound != null)
        {
            if (sound.backgroundMusic != null)
            {
                sound.backgroundMusic.Stop();
                sound.backgroundMusic.clip = sound.backgroundSong[0];
                sound.backgroundMusic.Play();
            }
        }
    }

    static public void PlayVictoryMusic()
    {
        if (sound != null)
        {
            if (sound.backgroundMusic != null)
            {
                sound.backgroundMusic.Stop();
                sound.backgroundMusic.clip = sound.backgroundSong[2];
                sound.backgroundMusic.Play();
            }
        }
    }

    static public void PlayDefeatMusic()
    {
        if (sound != null)
        {
            if (sound.backgroundMusic != null)
            {
                sound.backgroundMusic.Stop();
                sound.backgroundMusic.clip = sound.backgroundSong[1];
                sound.backgroundMusic.Play();
            }
        }
    }

    static public void PlayMainMenuMusic()
    {
        if (sound != null)
        {
            if (sound.backgroundMusic != null)
            {
                sound.backgroundMusic.Stop();
                sound.backgroundMusic.clip = sound.backgroundSong[4];
                sound.backgroundMusic.Play();
            }
        }
    }

    static public void PlayOpenMenu()
    {
        if (sound != null)
        {
            if (sound.soundEffects != null)
            {
                sound.soundEffects.clip = sound.quickSound[1];
                sound.soundEffects.PlayOneShot(sound.soundEffects.clip);
            }
        }
    }
}
