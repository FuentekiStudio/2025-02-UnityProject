using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Audio_Manager is a singleton that manages audio playback for music and sound effects (SFX) in the game.
/// It updates audio volumes based on the current game state and player settings 
/// using references from Sound_Settings, including independent volume control for each audio 
/// source and the ability to pause or resume music.
/// </summary>

public class Audio_Manager : MonoBehaviour
{
    // Singleton instance for global access to Audio_Manager
    public static Audio_Manager Instance;

    // Audio sources for music and SFX playback
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    // List to hold music clips for different game states (e.g., MainMenu, Gameplay, Victory)
    [SerializeField] private List<AudioClip> musicClips;

    private void Awake()
    {
        // Ensure only one instance of Audio_Manager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    /// <summary>
    /// Play music corresponding to the provided game state
    /// </summary>
    /// <param name="state"></param>
    public void PlayMusicForState(GameManager.GameState state)
    {
        // Ensure the audio clip exists
        //if (musicAudio == null)
        //    return;
        //else
        //{
        //    musicSource.clip = musicAudio;
        //    musicSource.Play();
        //}

        AudioClip clipToPlay = null;

        // Assign a clip based on the current game state
        switch (state)
        {
            case GameManager.GameState.MainMenu:
                clipToPlay = musicClips[0]; // MainMenu music
                break;
            case GameManager.GameState.Gameplay:
                clipToPlay = musicClips[1]; // Gameplay music
                break;
            case GameManager.GameState.Victory:
                clipToPlay = musicClips[2]; // Victory music
                break;
        }

        // Check if the selected clip is different from the current one
        if (clipToPlay != null && musicSource.clip != clipToPlay)
        {
            musicSource.clip = clipToPlay;
            musicSource.volume = Sound_Settings.instance.GetMusicVolume();
            musicSource.Play();

//            Debug.Log("Playing clip: " + clipToPlay.name);
        }

        UpdateVolumes(); // Update volume levels
    }

    /// <summary>
    /// Play a specific SFX clip
    /// </summary>
    /// <param name="sfxAudio"></param>
    public void PlaySFX(AudioClip sfxAudio)
    {
        // Ensure the audio clip exists
        if (sfxAudio == null)
            return;
        else
            sfxSource.PlayOneShot(sfxAudio);

        UpdateVolumes(); // Update volume levels
    }

    /// <summary>
    /// Updates the volumes for both music and SFX sources based on player settings
    /// </summary>
    public void UpdateVolumes()
    {
        // Calculate final volumes by multiplying GeneralVolume with individual settings
        float generalVolume = Sound_Settings.instance.GetGeneralVolume();
        musicSource.volume = generalVolume * Sound_Settings.instance.GetMusicVolume();
        sfxSource.volume = generalVolume * Sound_Settings.instance.GetSFXVolume();

        // Mute sources if either the specific volume or GeneralVolume is set to 0
        musicSource.mute = Sound_Settings.instance.GetMusicVolume() == 0 || generalVolume == 0;
        sfxSource.mute = Sound_Settings.instance.GetSFXVolume() == 0 || generalVolume == 0;
    }

    // Pauses the music currently playing
    public void PauseMusic()
    {
        musicSource.Pause();
    }

    // Resumes the paused music
    public void UnpauseMusic()
    {
        musicSource.UnPause();
    }
}