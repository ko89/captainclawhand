using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFXSource : ScriptableObject
{
    public AudioClip[] audioClips;
    public float volume0 = 1;
    public float volume1 = 1;

    public float pitch0 = 1;
    public float pitch1 = 1;


    public void PlayOneShot(AudioSource source)
    {
        var clip = audioClips[Random.Range(0, audioClips.Length)];
        var volume = Random.Range(volume0, volume1);
        var pitch = Random.Range(pitch0, pitch1);

        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
    }
}
