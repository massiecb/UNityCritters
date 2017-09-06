using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour {
    // Use this for initialization
    public bool canSeePredator = false;
    public float RADIUS = 0.5f;

    private Animator ani;
	void Start () {
        ani = gameObject.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Idle"))
            IdleHandler();
        if (canSeePredator) {
            Debug.Log("Can see ");
        }
		
	}

    private void IdleHandler () {

    }

    private void OnTriggerEnter (Collider target) {
        if (target.gameObject.tag != "Terrain") {
            if (target.gameObject.tag == "Predator" && LOSClear(target.transform))
                canSeePredator = true;
        }
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
