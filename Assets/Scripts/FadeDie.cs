using UnityEngine;
using System.Collections;

public class FadeDie : MonoBehaviour {

    public float timeToDie = 2;

    public GameObject wreck;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(timeToDie);

        DestroyMe();
	}
	
	void DestroyMe() {
        if(wreck != null)
        {
            GameObject wreckClone = Instantiate(wreck, transform.position, transform.rotation) as GameObject;
        }
        Destroy(gameObject);
        
	}
}
