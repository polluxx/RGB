using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    Animator parentAnimator;

    public GameObject force;
    public GameObject instrument;
    //public GameObject trail;

    private GameObject beam;
    private GameObject chain;
    private GameObject[] chains;

    private GameObject parentItem;
    //private GameObject beamTrail;

    // Use this for initialization
    void Start () {
        parentAnimator = gameObject.GetComponentInParent<Animator>();
        parentItem = GameObject.Find("ChainStart");
    }

    void Fire()
    {
        if (parentAnimator != null) parentAnimator.SetTrigger("fire");
        if (beam == null)
        {
            beam = Instantiate(force, force.transform.position, Quaternion.identity) as GameObject;
            //beamTrail = Instantiate(trail, beam.transform.position, beam.transform.rotation) as GameObject;
        }
    }

    void Stack()
    {
        if (parentAnimator != null) parentAnimator.SetTrigger("fire");
        if (chain == null)
        {
            //chain = Instantiate(instrument, new Vector3(0f, 0, -1f), Quaternion.identity) as GameObject;
            //chain.transform.parent = transform;
            //chain.transform.localPosition = new Vector3(0.2f, -0.3f, -1f);
            //HingeJoint2D hinge = chain.GetComponent<HingeJoint2D>();
            //hinge.connectedBody = GetComponent<Rigidbody2D>();
            instrument.SetActive(true);
        }
    }
}
