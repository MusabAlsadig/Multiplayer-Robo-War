using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundPlayer : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private float minPitch;
    [SerializeField, Range(1, 2)]
    private float maxPitch;
    [SerializeField]
    private bool loop;
    [SerializeField]
    private float rate = 10f;

    [SerializeField]
    private AudioSource audioSource;

    private float time;

    private void Update()
    {
        if (loop)
        {
            time += rate * Time.deltaTime;
        }

        if (time > 1)
        {
            time -= 1;
            Play();
        }
    }

    public void Play()
    {
        audioSource.Stop();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }
}
