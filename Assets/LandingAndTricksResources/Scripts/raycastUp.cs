using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastUp : MonoBehaviour {

    public Vector2 point;
    public bool grindable;
    public Transform gTransform;
    public float railY;
    private playerBehavior parent;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<playerBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 10f, parent.groundLayer);

        Debug.DrawRay(transform.position, -Vector3.up * 10f, Color.green);
        if (hit)
        {
            point = hit.point;
            if (hit.collider.gameObject.tag == "Grindable")
            {
                grindable = true;
                gTransform = hit.transform;
                //var rotation = hit.collider.gameObject.transform.right;
//                railY = hit.collider.gameObject.GetComponent<BoxCollider2D>().
                railY = hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider2D>().size.y/2;
            }
            else
            {
                grindable = false;
                gTransform = null;
            }
        }
        else
        {
            grindable = false;
            gTransform = null;
        }
    }
}
