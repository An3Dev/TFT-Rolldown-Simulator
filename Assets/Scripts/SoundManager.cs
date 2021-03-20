using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip selectCardSFX, refreshCardsSFX;
    public AudioSource audioSource;

    const string soundEffectsVolumePreferenceKey = "SoundEffectsVolumeKey";
    const string enableSoundEffectsPreferenceKey = "EnableSoundEffects";
    
    private float soundEffectVolumeLevel;
    private bool enableSoundEffects = true;

    public GameObject enabledSFXButton, disabledSFXButton;
    public Slider volumeSlider;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }

        soundEffectVolumeLevel = PlayerPrefs.GetFloat(soundEffectsVolumePreferenceKey, 0.5f);
        audioSource.volume = soundEffectVolumeLevel;

        enableSoundEffects = PlayerPrefs.GetInt(enableSoundEffectsPreferenceKey, 1) == 1 ? true : false;
        audioSource.mute = !enableSoundEffects;
        // update settings UI
        enabledSFXButton.SetActive(enableSoundEffects);
        disabledSFXButton.SetActive(!enableSoundEffects);

        volumeSlider.value = soundEffectVolumeLevel * 10;

    }

    public void SetVolumeLevel(float level)
    {
        audioSource.volume = level / 10;
        soundEffectVolumeLevel = level / 10;
        PlayerPrefs.SetFloat(soundEffectsVolumePreferenceKey, soundEffectVolumeLevel);
    }

    public void UpdateVolumeText(TextMeshProUGUI text)
    {
        text.text = (soundEffectVolumeLevel * 10).ToString();
    }

    public void EnableSoundEffects(bool enable)
    {
        audioSource.mute = !enable;
        enableSoundEffects = enable;
        PlayerPrefs.SetInt(enableSoundEffectsPreferenceKey, enable ? 1 : 0);
    }

    public void PlaySelectCardSFX()
    {
        audioSource.PlayOneShot(selectCardSFX);
    }

    public void PlayRefreshSFX()
    {
        audioSource.PlayOneShot(refreshCardsSFX);
    }
}
