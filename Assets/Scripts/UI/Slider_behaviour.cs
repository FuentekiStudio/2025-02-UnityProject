using UnityEngine;
using UnityEngine.UI;

public class Slider_behavior : MonoBehaviour
{
    public Slider slider;
    public enum SliderType { General, Music, SFX }
    public SliderType sliderType;

    private void Start()
    {
        if (Sound_Settings.instance != null)
        {
            // Set the slider based on its type
            switch (sliderType)
            {
                case SliderType.General:
                    slider.value = Sound_Settings.instance.GetGeneralVolume();
                    break;
                case SliderType.Music:
                    slider.value = Sound_Settings.instance.GetMusicVolume();
                    break;
                case SliderType.SFX:
                    slider.value = Sound_Settings.instance.GetSFXVolume();
                    break;
            }
        }

        // Add listener for when the user changes the slider value
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (Sound_Settings.instance != null)
        {
            // Update the proper volume based on the slider type
            switch (sliderType)
            {
                case SliderType.General:
                    Sound_Settings.instance.SetGeneralVolume(value);
                    break;
                case SliderType.Music:
                    Sound_Settings.instance.SetMusicVolume(value);
                    break;
                case SliderType.SFX:
                    Sound_Settings.instance.SetSFXVolume(value);
                    break;
            }
        }
    }
}