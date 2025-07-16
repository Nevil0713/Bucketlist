using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource bgm_player;
    AudioSource sfx_player;

    public AudioClip[] audio_clips;
    public SoundManager instance;

    void Awake()
    {
        instance = this;

        bgm_player = GameObject.Find("BGM_M").GetComponent<AudioSource>();
        sfx_player = GameObject.Find("SFX_M").GetComponent<AudioSource>();
    }

    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Clique": index = 0; break;
            case "TestSFX": index = 1; break;
        }

        sfx_player.clip = audio_clips[index];
        sfx_player.Play();
    }

    public void PlayMusic(string type)
    {
        int index = 0;

        switch (type)
        {
            case "TestBGM": index = 0; break;
        }

        bgm_player.clip = audio_clips[index];
        bgm_player.Play();
    }

    public void Start()
    {
        PlayMusic("TestBGM");
    }
}
