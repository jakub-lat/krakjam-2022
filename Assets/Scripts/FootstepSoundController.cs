using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] footsteps = null;

    private List<AudioClip> notUsedClips = null;

    /*
    public void PlayRandomSoundFromRange()
    {
        if (notUsedClips.Count <= 0)
        {
            
            return;
        }

        AudioClip footstepSound = footsteps[Random.Range(0, footsteps.Length)];
        notUsedClips.Add(footstepSound);
    }
    */
}
