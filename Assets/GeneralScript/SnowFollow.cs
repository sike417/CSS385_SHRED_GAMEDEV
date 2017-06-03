using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFollow : MonoBehaviour {

	// Use this for initialization
    public float offsetX;
    public float offsetY;
    public GameObject mHero;

	void Start ()
	{
	    offsetX = transform.position.x;
	    offsetY = transform.position.y;
        mHero = GameObject.Find("snow_boarder");
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var heroPos = mHero.transform.position;
        transform.position = new Vector3(heroPos.x + offsetX, heroPos.y + offsetY, -10);

    }
}
