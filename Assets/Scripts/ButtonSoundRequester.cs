using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundRequester : MonoBehaviour
{
    [SerializeField]
    private SoundType soundType;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlaySound(soundType);
    }
}
