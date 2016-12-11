using UnityEngine;
using System.Collections;

public class HandsTrack : MonoBehaviour {

    private GameObject aim;
    private GameObject player;

    public GameObject weapon;

    // Use this for initialization
    void Start () {
        aim = GameObject.Find("Aim");
        player = GameObject.Find("Zet");
        
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        float deltaX = transform.position.x - mouseWorldPosition.x;
        float deltaY = transform.position.y - mouseWorldPosition.y;

        float diffAngle = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;

        if (diffAngle > 60 && diffAngle < 120) return;

        float lessThanPers = mouseWorldPosition.x - player.transform.position.x;

        if (lessThanPers>0.01f)
        {
            diffAngle -= 180;
        }

        transform.rotation = Quaternion.AngleAxis(diffAngle, new Vector3(0, 0, 1));
    }
}
