using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    private LineRenderer laser;
    private GameObject aim;
    private GameObject parentweapon;

    public LayerMask masksToAvoid;

    public float maxLaserLen = 200f;

    public int lengthOfLineRenderer = 2;

    public bool drawTrail = false;
    public float timeToFade = 0.1f;
    public float timeToTrailing = 1;

    public GameObject trail;
    private GameObject trailBeam;

    float currentTime;

    Vector3 trailDirection;

    RaycastHit2D hit;

    Vector3 hitPosition;
    bool hitPosSetted = false;

    void Start()
    {
        aim = GameObject.Find("Aim");
        
        laser = GetComponent<LineRenderer>();
        parentweapon = GameObject.Find("Blaster");
        laser.material = new Material(Shader.Find("Particles/Additive"));
        
        transform.localScale = new Vector3(1f, 0.02f, 1f);
        transform.localPosition = new Vector3(1f, 0f, -1f);
        laser.useWorldSpace = true;
        laser.SetVertexCount(lengthOfLineRenderer);

        currentTime = Time.time;

        if (drawTrail && trailBeam == null)
        {
            trailBeam = Instantiate(trail, transform.position, laser.transform.rotation) as GameObject;
        }

        trailDirection = parentweapon.transform.position;


        trailBeam.transform.position = trailDirection;
    }

	// Update is called once per frame
	void Update () {
        
        currentTime -= Time.time;
        if (!hitPosSetted)
        {
            laser.SetPosition(0, parentweapon.transform.position);
        
            transform.localPosition = new Vector3(parentweapon.transform.position.x, parentweapon.transform.position.y, -1f);
        }

        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(aim.transform.position));
        Vector3 dirr = ray.direction;
        dirr.Set(ray.direction.x, ray.direction.y, 0f);
        
        
        hit = Physics2D.Raycast(parentweapon.transform.position, dirr, maxLaserLen, masksToAvoid);

        if (hit.collider != null)
        {


            if(!hitPosSetted)
            {
                hitPosition = hit.point;

                laser.SetPosition(1, hit.point);
                trailBeam.transform.position = hitPosition;
                hitPosSetted = true;
            }
            

        } else
        {
            float drawTo = aim.transform.position.x;
            //drawTo = drawTo < 0 ? drawTo - 100 : drawTo + 100;
            laser.SetPosition(1, new Vector3(drawTo, aim.transform.position.y, 0f));
        }

    }
}
