using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {

    public GameObject aim;

	// Update is called once per frame
	void Update () {

        if(aim == null)
        {
            aim = Instantiate(aim, new Vector3(2f, 0f, 0f), Quaternion.identity) as GameObject;
        }

        //Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

        aim.transform.localPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0f);
    }
}
