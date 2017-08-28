using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour {
    // Use this for initialization
    public bool canSeePredator = false;
    public float RADIUS = 0.5f;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter (Collider target) {
        if (target.gameObject.tag == "Predator")
            canSeePredator = true;
    }

    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Predator")
            canSeePredator = false;
    }

    private bool LOSClear (Transform other) {
        RaycastHit firstTarget;
        Vector3 delta = transform.position - other.position;
        if (!Physics.SphereCast(transform.position, RADIUS, transform.forward, out firstTarget, delta.magnitude - RADIUS))
            return true;
        return firstTarget.collider.gameObject.tag == other.gameObject.tag;
    }
}
