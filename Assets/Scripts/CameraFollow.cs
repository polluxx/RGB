using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    private Vector2 velocity;

    public float smTimeX;
    public float smTimeY;

    public Vector3 minCamPos;
    public Vector3 maxCamPos;

    public bool boundaries;

    public GameObject tracker;

    private float zoom, initialZoom;

	// Use this for initialization
	void Start () {
        tracker = GameObject.Find("Zet");
        zoom = initialZoom = GetComponent<Camera>().fieldOfView;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        Vector3 transPos = transform.position; 

        float posX = Mathf.SmoothDamp(transPos.x, tracker.transform.position.x, ref velocity.x, smTimeX);
        float posY = Mathf.SmoothDamp(transPos.y, tracker.transform.position.y, ref velocity.y, smTimeY);
        
        if(boundaries)
        {
            if(minCamPos.x != 0 || maxCamPos.x != 0) posX = Mathf.Clamp(transPos.x, minCamPos.x, maxCamPos.x);
            posY = Mathf.Clamp(transPos.y, minCamPos.y, maxCamPos.y);
        }

        transform.position = new Vector3(posX, posY, transPos.z);
    }

    void Update()
    {
        //zoom = GetComponent<Camera>().fieldOfView;
        if (Input.GetKey(KeyCode.R))
        {
            if (zoom < 179) zoom += 0.5f;
            GetComponent<Camera>().fieldOfView = zoom;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            GetComponent<Camera>().fieldOfView  = zoom = initialZoom;
        }
    }


}
