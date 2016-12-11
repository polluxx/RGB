using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {

    public float maxDistance = 100f;
    public LayerMask masksToAvoid;
    RaycastHit2D hit;

    Vector3 hitPosition;

    GameObject aim;

    // Use this for initialization
    void Start () {
        aim = GameObject.Find("Aim");

    }
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(aim.transform.position));
        Vector3 dirr = ray.direction;
        dirr.Set(ray.direction.x, ray.direction.y, 0f);


        hit = Physics2D.Raycast(transform.position, dirr, maxDistance, masksToAvoid);

        if (hit.collider != null)
        {
            //Debug.Log(hit.collider);
        }

    }
}
