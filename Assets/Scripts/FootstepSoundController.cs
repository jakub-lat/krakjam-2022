using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FootstepSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] footsteps = null;

    private List<AudioClip> notUsedClips = new List<AudioClip>();

    private AudioSource source;

    protected void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        if (FirstPersonController.Current.Controller.velocity.magnitude > 2f && !source.isPlaying && FirstPersonController.Current.Controller.isGrounded)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.volume = Random.Range(0.5f, 0.9f);
            source.PlayOneShot(GetRandomSoundFromRange());

        }
    }

    public AudioClip GetRandomSoundFromRange()
    {
        if (notUsedClips.Count <= 0)
        {
            notUsedClips = new List<AudioClip>(footsteps);
        }

        AudioClip footstepSound = notUsedClips[Random.Range(0, notUsedClips.Count)];
        notUsedClips.Remove(footstepSound);
        return footstepSound;
    }
}
