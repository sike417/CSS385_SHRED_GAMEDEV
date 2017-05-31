using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    public AudioClip[] audioClips;
    private AudioSource BGM;
    private AudioClip lastPlayed;
    private int trackNum = 0;
    private float volume = 0.0f;
    private int playCount = 0;

    // Use this for initialization
    void Start() {
        BGM = GetComponent<AudioSource>();
        BGM.loop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!BGM.isPlaying && playCount == 1)
        {
            BGM.clip = GetRandomClip(lastPlayed);
            BGM.volume = volume;
            BGM.Play();
            lastPlayed = BGM.clip;
            playCount = 0;
        } else if (!BGM.isPlaying)
		{
		    BGM.volume = volume;
		    BGM.Play();
		    playCount += 1;
		}
       else 
        {
            if (BGM.clip.length - BGM.time < 5f && playCount == 1)
                FadeOut();
        }
        FadeIn();
    }

    AudioClip GetRandomClip(AudioClip lastPlayed)
    {
        AudioClip newClip;
        do {
            newClip = audioClips[Random.Range(0, audioClips.Length)];
        }
        while (newClip == lastPlayed);

        return newClip;
    }

    void FadeIn()
    {
        if(volume < 0.1f)
        {
            volume += 0.05f * Time.deltaTime;
            BGM.volume = volume;
        }
    }

    void FadeOut()
    {
        if(volume > 0f)
        {
            volume -= 0.02f * Time.deltaTime;
            BGM.volume = volume;
        }
    }
}
