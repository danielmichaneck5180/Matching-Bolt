﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip match1;
    public AudioClip match2;
    public AudioClip giggle;
    public AudioClip shoot;
    public AudioClip happy;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        AudioClip clip = match1;

        switch (sound)
        {
            case "Match1":
                clip = match1;
                break;

            case "Match2":
                clip = match2;
                break;

            case "Giggle":
                clip = giggle;
                break;

            case "Shoot":
                clip = shoot;
                break;

            case "Happy":
                clip = happy;
                break;

            default:
                clip = match1;
                break;
        }

        source.PlayOneShot(clip);
    }
}
