using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFXSource : ScriptableObject
{
    public AudioClip[] audioClips;

    public void PlayRandomClip()
    {
        audioClips[audioClips.Length()]
    }
}
