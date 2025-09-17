using UnityEngine;
using UnityEngine.Audio;


/// <summary>
/// Sound_Settings is a singleton class that manages audio volume settings for general, music, and SFX volumes. 
/// It updates values in the AudioMixer and saves them in PlayerPrefs for persistence. The class provides 
/// methods to adjust volumes dynamically, which interact with Audio_Manager to apply the changes in real-time.
/// </summary>
public class Sound_Settings : MonoBehaviour
{
    // Singleton instance to ensure a single Sound_Settings across scenes
    public static Sound_Settings instance;

    // Reference to the AudioMixer to control volumes
    [SerializeField] private AudioMixer audioMixer;

    // Properties to expose volume levels for General, Music, and SFX
    public float GeneralVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }

    private void Awake()
    {
        // Set up singleton instance and prevent duplicates
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolumeSettings(); // Load saved volume settings on start
    }

    /// <summary>
    /// Sets and saves the General Volume, adjusts it in the audio mixer, and updates volumes
    /// </summary>
    /// <param name="volume"></param>
    public void SetGeneralVolume(float volume)
    {
        GeneralVolume = volume;
        audioMixer.SetFloat("GeneralVolume", Mathf.Log10(volume) * 20); // Convert to decibels
        PlayerPrefs.SetFloat("GeneralVolume", volume);

        // Notify Audio_Manager to update active audio volumes
        Audio_Manager.Instance?.UpdateVolumes();
    }

    /// <summary>
    /// Sets and saves the Music Volume, then updates the mixer volumes
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        UpdateVolumes();
    }

    /// <summary>
    /// Sets and saves the SFX Volume, then updates the mixer volumes
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        UpdateVolumes();
    }

    /// <summary>
    /// Updates the volume settings in the AudioMixer based on General, Music, and SFX values
    /// </summary>
    public void UpdateVolumes()
    {
        // Calculate final volumes with GeneralVolume as a multiplier, ensuring positive values for log conversion
        float finalMusicVolume = Mathf.Log10(Mathf.Max(GeneralVolume * MusicVolume, 0.0001f)) * 20;
        float finalSFXVolume = Mathf.Log10(Mathf.Max(GeneralVolume * SFXVolume, 0.0001f)) * 20;

        audioMixer.SetFloat("MusicVolume", finalMusicVolume);
        audioMixer.SetFloat("SFXVolume", finalSFXVolume);
    }

    /// <summary>
    /// Load saved volume levels from PlayerPrefs and apply them to sliders and mixer settings
    /// </summary>
    private void LoadVolumeSettings()
    {
        GeneralVolume = PlayerPrefs.HasKey("GeneralVolume") ? PlayerPrefs.GetFloat("GeneralVolume", 0.75f) : 0.75f;
        MusicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume", 0.75f) : 0.75f;
        SFXVolume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume", 0.75f) : 0.75f;

        UpdateVolumes();
    }

    /// <summary>
    /// Getter methods for sliders to access current volume values
    /// </summary>
    /// <returns></returns>
    public float GetGeneralVolume() => GeneralVolume;
    public float GetMusicVolume() => MusicVolume;
    public float GetSFXVolume() => SFXVolume;
}