using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayZoom : MonoBehaviour {

    Ray ray;
    RaycastHit2D hitinfo;
    Vector2 boundSize; 
    private playerBehavior pb;
    private Camera cam;
    private CameraScript cs;
    // Use this for initialization
    void Start () {
        ray = new Ray(transform.position, -1 * Vector3.up);
        boundSize= GetComponent<Collider2D>().bounds.size;
        pb = (playerBehavior)GetComponent(typeof(playerBehavior));
        cam = Camera.main;
        cs = (CameraScript)cam.GetComponent(typeof(CameraScript));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!pb.isOnGround)
        {
            
            Vector3 yOffset = new Vector3(0f, boundSize.y, 0f);
            for (int i = -225; i >= -315; i--)
            {
                float angle = Mathf.Deg2Rad * i;
                Vector2 Direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))*-1;
                RaycastHit2D temphitinfo=Physics2D.Raycast(transform.position - yOffset, Direction);
                hitinfo = Physics2D.Raycast(transform.position - yOffset,Direction,500);
                if(i==-225)
                {
                    hitinfo = temphitinfo;
                }
                if(temphitinfo.distance>hitinfo.distance)
                {
                    hitinfo = temphitinfo;
                }
                Debug.DrawRay(transform.position - yOffset, Direction*10,Color.white,1);
                
            }
            //Debug.Log(hitinfo.distance);
            cs.setMaxZoomValue(hitinfo.distance*1.5f);
        }
	}
}
