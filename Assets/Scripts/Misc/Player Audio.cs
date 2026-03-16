using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip[] soundEffects;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandom()
    {
        if (soundEffects.Length == 0) return;

        int index = Random.Range(0, soundEffects.Length);
        audioSource.clip = soundEffects[index];
        audioSource.Play();
    }
}
