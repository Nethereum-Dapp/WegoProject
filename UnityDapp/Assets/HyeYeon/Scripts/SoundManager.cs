using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] audioClip = new AudioClip[4];

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioManager(int index)
    {
        audioSource.clip = audioClip[index];
        audioSource.Play();
        audioSource.loop = true;

    }
}
