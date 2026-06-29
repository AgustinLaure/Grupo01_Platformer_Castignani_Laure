using UnityEngine;
using UnityEngine.Audio;

public class AudioManager
{
    private AudioMixer audioMixer;
    private AudioSource buttonPressedSound;

    private const float initialMasterVolume = 0f;
    private const float initialSfxVolume = 0f;
    private const float initialMusicVolume = 0f;

    private string wasGameOpenedBefore = "WasGameOpenedBefore";
    private const string masterVolumeKey = "MasterVolume";
    private const string sfxVolumeKey = "SfxVolume";
    private const string musicVolumeKey = "MusicVolume";

    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; audioMixer.SetFloat(masterVolumeKey, masterVolume); } }
    public float SfxVolume { get { return sfxVolume; } set { sfxVolume = value; audioMixer.SetFloat(sfxVolumeKey, sfxVolume); } }
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; audioMixer.SetFloat(musicVolumeKey, musicVolume); } }
    public AudioSource ButtonPressedSound { get { return buttonPressedSound; } private set { } }

    public AudioManager(AudioMixer audioMixer, AudioSource buttonPressedSound)
    {
        this.audioMixer = audioMixer;
        this.buttonPressedSound = buttonPressedSound;

        if (PlayerPrefs.HasKey(wasGameOpenedBefore))
        {
            masterVolume = PlayerPrefs.GetFloat(masterVolumeKey);
            sfxVolume = PlayerPrefs.GetFloat(sfxVolumeKey);
            musicVolume = PlayerPrefs.GetFloat(musicVolumeKey);
        }
        else
        {
            masterVolume = initialMasterVolume;
            sfxVolume = initialSfxVolume;
            musicVolume = initialMusicVolume;

            PlayerPrefs.SetInt(wasGameOpenedBefore, 1);

            SaveConfig();
        }

        UpdateAudioMixerValues();
    }

    private void UpdateAudioMixerValues()
    {
        audioMixer.SetFloat(masterVolumeKey, masterVolume);
        audioMixer.SetFloat(sfxVolumeKey, sfxVolume);
        audioMixer.SetFloat(musicVolumeKey, musicVolume);
    }

    private void SaveConfig()
    {
        PlayerPrefs.SetFloat(masterVolumeKey, masterVolume);
        PlayerPrefs.SetFloat(sfxVolumeKey, sfxVolume);
        PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);

        PlayerPrefs.Save();
    }

    ~AudioManager()
    {
        SaveConfig();
    }
}

