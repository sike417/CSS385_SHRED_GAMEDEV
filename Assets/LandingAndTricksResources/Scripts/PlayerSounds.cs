using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSounds : MonoBehaviour {

    public AudioClip snowboard, grind, collisionSound, speedBoost;
    private AudioSource boardSrc, grindSrc, collisionSrc, speedBoostSrc;
    private playerBehavior pb;
    private bool hasCrashed = false;

    // Use this for initialization
    void Start()
    {
        boardSrc = GameObject.Find("snow_board_sound").GetComponent<AudioSource>();
        grindSrc = GameObject.Find("grind_sound").GetComponent<AudioSource>();
        speedBoostSrc = GameObject.Find("speed_boost_sound").GetComponent<AudioSource>();
        collisionSrc = GameObject.Find("collision_sound").GetComponent<AudioSource>();

        pb = GetComponent<playerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pb.isOnGround || pb.getState() != "Live" )
            boardSrc.Stop();
        
        boardSrc.volume = pb.velocity * 30 / pb.maxSpeed;
        boardSrc.pitch = pb.velocity * 5 / pb.maxSpeed;

        if (pb.isOnGround && !boardSrc.isPlaying && pb.getState() == "Live")
        {
            boardSrc.clip = snowboard;
            boardSrc.Play();
        }

//        if (pb.getState() == 2.ToString())
//        {
            
//        }

        if (!pb.attachedToRail)
            grindSrc.Stop();

        if (pb.attachedToRail && !grindSrc.isPlaying)
        {
            grindSrc.volume = 0.1f;
            grindSrc.clip = grind;
            grindSrc.Play();
        }

        if (pb.getState() == "Die" && hasCrashed == false)
        {
            collisionSrc.volume = 0.3f;
            collisionSrc.clip = collisionSound;
            collisionSrc.Play();
            hasCrashed = true;
        }

        if (!Input.GetKey("left shift") || pb.boost < 0)
            speedBoostSrc.Stop();

        if (Input.GetKeyDown("left shift") && pb.boost > 0)
        {
            speedBoostSrc.volume = 0.1f;
            speedBoostSrc.clip = speedBoost;
            speedBoostSrc.Play();
        }
    }

    public void Retry()
    {
        hasCrashed = false;
    }
}
