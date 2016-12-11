using UnityEngine;
using System.Collections;

public class FlahLight : MonoBehaviour {

    private GameObject aim;

	// Use this for initialization
	void Start () {
        aim = GameObject.Find("Aim");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(aim.transform);
    }
}
