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

    [Header("Pool")]
    [SerializeField] private int sfxPoolSize = 8;
    private readonly List<AudioSource> sfxPool = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // build pool
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.loop = false;
            src.spatialBlend = 1f;    // 3D by default
            sfxPool.Add(src);
        }
    }
    private AudioSource GetFreeSfxSource()
    {
        foreach (var src in sfxPool)
            if (!src.isPlaying) return src;

        // if all busy, recycle the least important (first)
        return sfxPool[0];
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

    /// <summary>
    /// Plays a 3D SFX with custom pitch/loop/spatial settings from the pool.
    /// Returns the AudioSource (handle) so callers can stop it when needed.
    /// </summary>
    public AudioSource PlaySfxPooled(AudioClip clip, Transform follow = null,
                                     float pitch = 1f, bool loop = false,
                                     float spatialBlend = 1f)
    {
        if (clip == null) return null;
        var src = GetFreeSfxSource();

        src.Stop();
        src.clip = clip;
        src.loop = loop;
        src.pitch = pitch;
        src.spatialBlend = spatialBlend;

        // position follow (simple)
        if (follow != null) src.transform.position = follow.position;

        // if playing in reverse, start from end so it actually goes backwards
        if (pitch < 0f && clip.samples > 0)
            src.timeSamples = clip.samples - 1;
        else
            src.time = 0f;

        src.Play();
        UpdateVolumes();
        return src;
    }

    /// <summary>
    /// Convenience: forward OneShot + a separate reverse loop that the caller can stop.
    /// Returns the reverse AudioSource handle.
    /// </summary>
    public AudioSource PlayForwardAndReverse(AudioClip clip, Transform follow,
                                             float spatialBlend = 1f)
    {
        // forward one-shot (uses pooled source too so volumes are unified)
        var fwd = PlaySfxPooled(clip, follow, 1f, false, spatialBlend);
        // reverse sustained while the effect lasts
        var rev = PlaySfxPooled(clip, follow, -1f, false, spatialBlend);
        return rev;
    }

    /// <summary>
    /// Stop a pooled AudioSource (and free it for reuse).
    /// </summary>
    public void StopPooled(AudioSource src)
    {
        if (src == null) return;
        src.Stop();
        src.clip = null;
        src.loop = false;
        src.pitch = 1f;
    }
}