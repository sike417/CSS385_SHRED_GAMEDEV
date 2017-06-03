using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathColliderScript : MonoBehaviour {
    private playerBehavior pb;
    private SpriteRenderer sb;
    public bool isVisible;
    // Use this for initialization
    void Start () {
        sb = GetComponent<SpriteRenderer>();
        if (!isVisible)
            sb.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            pb = (playerBehavior)other.gameObject.GetComponent(typeof(playerBehavior));
            pb.CrashPlayer();
        }
    }
}
