using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        None,
    }

    public enum BGMType
    {
        Test,
    }

    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] sfxList;
    [SerializeField] private AudioClip[] bgmList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySFX(SoundType type, float volume = 1f)
    {
        if ((int)type >= instance.sfxList.Length) return;

        AudioClip clip = instance.sfxList[(int)type];
        if (clip != null)
        {
            instance.sfxSource.PlayOneShot(clip, volume);
        }
    }

    public static void PlayBGM(BGMType type, float volume = 1f)
    {
        if ((int)type >= instance.bgmList.Length) return;

        AudioClip clip = instance.bgmList[(int)type];
        if (clip != null && instance.bgmSource.clip != clip)
        {
            instance.bgmSource.clip = clip;
            instance.bgmSource.volume = volume;
            instance.bgmSource.loop = true;
            instance.bgmSource.Play();
        }
    }

    public static void StopBGM()
    {
        instance.bgmSource.Stop();
    }

    public static void SetBGMVolume(float volume)
    {
        instance.bgmSource.volume = volume;
    }

    public static void SetSFXVolume(float volume)
    {
        instance.sfxSource.volume = volume;
    }
}