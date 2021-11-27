using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtCurrentPosition : MonoBehaviour
{   
    public void PlaySound(AudioClip soundClip)
    {
        AudioSource.PlayClipAtPoint(soundClip, transform.position, 1f);
    }
}
