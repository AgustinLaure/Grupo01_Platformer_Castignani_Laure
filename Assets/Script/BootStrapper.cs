using UnityEngine;
using UnityEngine.Audio;

public static class BootStrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        AudioMixer audioMixer = Resources.Load<AudioMixer>("AudioMixer");

        GameObject audioManagerObject = new GameObject("[AudioManager]");
        Object.DontDestroyOnLoad(audioManagerObject);

        AudioSource audioSource = audioManagerObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("click_sound");

        ServiceLocator.Instance.AddService(new AudioManager(audioMixer, audioSource));
    }
}
