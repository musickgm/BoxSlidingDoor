using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip releaseBallClip;
    public AudioClip selectBallClip;

    public void PlayAudioClip(AudioClip _clip, Vector3 location)
    {
        if (_clip == null)
        {
            return;
        }

        GameObject temporaryAudioHost = new GameObject("TempAudio");
        temporaryAudioHost.transform.position = location;
        AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
        audioSource.clip = _clip;

        audioSource.Play();

        Destroy(temporaryAudioHost, _clip.length);
    }

    public void ReleaseBallSound()
    {
        Vector3 location;
        if(Box.ballClone != null)
        {
            location = Box.ballClone.transform.position;
        }
        else
        {
            location = new Vector3(0, 0, 0);
        }
        PlayAudioClip(releaseBallClip, location);
    }

    public void SelectBallSound()
    {
        Vector3 location;
        if (Box.ballClone != null)
        {
            location = Box.ballClone.transform.position;
        }
        else
        {
            location = new Vector3(0, 0, 0);
        }
        PlayAudioClip(selectBallClip, location);
    }
}
