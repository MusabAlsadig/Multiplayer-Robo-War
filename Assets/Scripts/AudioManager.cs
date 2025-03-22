using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource uiClickSound, uiReturnSound;

    private readonly Dictionary<SoundType, AudioSource> sounds = new Dictionary<SoundType, AudioSource>();

    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        sounds[SoundType.Click] = uiClickSound;
        sounds[SoundType.Return] = uiReturnSound;
    }

    public void PlaySound(SoundType soundType)
    {
        sounds[soundType].Play();
    }

}

public enum SoundType
{
    Click, Return
}
