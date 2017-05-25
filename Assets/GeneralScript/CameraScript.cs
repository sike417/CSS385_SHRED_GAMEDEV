using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private enum CamState
    {
        Following,
        Stop
    };
    //public int mCycles;
    public float offsetX;
    public GameObject mHero;
    private CamState camerastate;
    private Camera cam;
    private float zoomVal;
    private float INITZOOM;
    public float minZoom;
    public float maxZoom;
    public float zoomSmoothing;
    // Use this for initialization
    void Start()
    {
        INITZOOM = Camera.main.orthographicSize;
        zoomVal = INITZOOM;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (camerastate == CamState.Following)
        {
//            Vector2 camPos = this.transform.position;
            Vector2 heroPos = mHero.transform.position;
            transform.position = new Vector3(heroPos.x + offsetX, heroPos.y, -10);
            cam = GetComponent<Camera>();
            if (cam.orthographicSize < zoomVal)
            {
                cam.orthographicSize += zoomSmoothing;
            }
            if (cam.orthographicSize >= zoomVal)
            {
                cam.orthographicSize -= zoomSmoothing;
            }
        }
//        if (Input.GetKey("z"))
//            cam.orthographicSize = 50;
//        if (Input.GetKey("v"))
//            cam.orthographicSize -= 1;


    }
    public void setMaxZoomValue(float val)
    {
        float newZoom;
        newZoom = Mathf.Clamp(val, minZoom, maxZoom);
        zoomVal = newZoom;
        
    }
    public void StartCam()
    {
        camerastate = CamState.Following;
    }
    public void StopCam()
    {
        camerastate = CamState.Stop;
    }
}
