using JetBrains.Annotations;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("BGM")]
    public AudioClip BGMclip;
    public float BGMVol;
    public AudioSource BGMsource;

    [Header("SFX")]
    public AudioClip SFXclip;
    public float SFXVol;
    public int channel;
    public AudioSource[] SFXsource;
    private int chIndex;

    private void Awake()
    {
        instance = this;
        SoundReset();
    }

    void SoundReset()
    {
        //BGM reset
        GameObject bgmObject = new GameObject("BGMsource");
        bgmObject.transform.parent = transform;
        BGMsource = bgmObject.GetComponent<AudioSource>();
        BGMsource.playOnAwake = false;
        BGMsource.loop = true;
        BGMsource.volume = BGMVol;
        BGMsource.clip = BGMclip;

        //SFX reset
        GameObject sfxObject = new GameObject("SFXsource");
        sfxObject.transform.parent = transform;
        SFXsource = new AudioSource[channel];

        for (int i = 0; i < SFXsource.Length; i++)
        {
            SFXsource[i] = sfxObject.AddComponent<AudioSource>();
            SFXsource[i].playOnAwake = false;
            SFXsource[i].volume = SFXVol;
        }
    }

    public void PlayBGM(bool isPlay)
    {
        if (isPlay)
        {
            BGMsource.Play();
        }
        else
        {
            BGMsource.Stop();
        }
    }
}
