using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    private AudioSource bgmSource;

    private AudioSource effectSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialize
    public void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        effectSound = gameObject.AddComponent<AudioSource>();
    }

    public void Start()
    {
        //Init();
        //PlayBGM("bgm1", true);
    }

    // Play BGM
    public void PlayBGM(string bgmName, bool isLoop)
    {
        // Load BGM clip
        bgmSource.clip = Resources.Load<AudioClip>("Sounds/BGM/" + bgmName);

        bgmSource.loop = isLoop;

        bgmSource.Play();
    }

    // Play Effect
    public void PlayEffect(string effectName)
    {
        // Load effect clip
        effectSound.clip = Resources.Load<AudioClip>("Sounds/" + effectName);

        AudioSource.PlayClipAtPoint(effectSound.clip, transform.position);
    }

    public void stopBgmSource()
    {
        bgmSource.Stop();
    }

    public void toggleBgmSource()
    {
        bgmSource.mute = !bgmSource.mute;
    }

    public void toggleEffect()
    {
        effectSound.mute = !bgmSource.mute;
    }

    public void bgmSourceVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void effectSoundVolume(float volume)
    {
        effectSound.volume = volume;
    }
}
