using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    public AudioClip snowboard, grind, speedBoost;
    private AudioSource boardSrc, grindSrc, speedBoostSrc;
    private playerBehavior pb;

    // Use this for initialization
    void Start()
    {
        boardSrc = GameObject.Find("snow_board_sound").GetComponent<AudioSource>();
        grindSrc = GameObject.Find("grind_sound").GetComponent<AudioSource>();

        pb = GetComponent<playerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pb.isOnGround)
            boardSrc.Stop();

        boardSrc.volume = pb.velocity * 20 / pb.maxSpeed;
        boardSrc.pitch = pb.velocity * 5 / pb.maxSpeed;
        if (pb.isOnGround && !boardSrc.isPlaying)
        {
            boardSrc.clip = snowboard;
            boardSrc.Play();
        }

        if (!pb.attachedToRail)
            grindSrc.Stop();

        if (pb.attachedToRail && !grindSrc.isPlaying)
        {
            grindSrc.volume = 0.1f;
            grindSrc.clip = grind;
            grindSrc.Play();
        }
    }       
}
