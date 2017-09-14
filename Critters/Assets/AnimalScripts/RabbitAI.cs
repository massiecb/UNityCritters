using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour {
    // Use this for initialization
    public bool canSeePredator = false;
    public float RADIUS = 0.5f;
	public float RUN_SPEED = 9f;
	public float WALK_SPEED = 3f;
	public float MAX_TURN = 90f;
	public float WAIT_TIME = 3f;
	public Vector3 groundNormal;

    private Animator ani;
	private Transform predator;
	private Rigidbody physics;
	private List<Vector3> grassLocations;
	void Start () {
        ani = gameObject.GetComponent<Animator>();
		physics = GetComponent<Rigidbody> ();

    }
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
		if (state.IsName ("Idle")) {
			IdleHandler ();
		}
		if (state.IsName("Running")) {
			SeePredatorHandler ();
        }
		
	}


	private void IdleHandler () {		
		physics.velocity = Vector3.zero;
		Debug.Log ("Sniffing in the air");
		if (Time.time > WAIT_TIME) {
			WAIT_TIME = WAIT_TIME + Time.time;
			ani.SetBool ("CanSeeFood", true);
		} else {
			//Vector3 targetSpeed = FindClosestGrass () * WALK_SPEED;
			//physics.velocity = Vector3.ProjectOnPlane (targetSpeed, groundNormal);
		}

    }

	private void SeePredatorHandler(){
		Debug.Log ("running");
		physics.velocity = -predator.transform.position;
		Vector3 targetSpeed = physics.velocity * RUN_SPEED;
		physics.velocity = Vector3.ProjectOnPlane (targetSpeed, groundNormal);
	}

    private void OnTriggerEnter (Collider target) {
        if (target.gameObject.tag != "Terrain") {
			if (target.gameObject.tag == "Predator" && LOSClear (target.transform)) {
				predator = target.transform;
				ani.SetBool ("CanSeePredator", true);
			}
        }
    }

    private void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Predator"){
			ani.SetBool("CanSeePredator", false);
		}
    }

	private void OnCollisionStay(Collision c){
		switch (c.gameObject.tag) {
		case "Terrain":
			groundNormal = Vector3.zero;
			foreach (ContactPoint pt in c.contacts)
				groundNormal += pt.normal;
			groundNormal.Normalize ();
			break;
		}
	}

	private bool LOSClear (Transform other) {
		RaycastHit firstTarget;
		Vector3 delta = transform.position - other.position;
		if (!Physics.SphereCast(transform.position, RADIUS, transform.forward, out firstTarget, delta.magnitude - RADIUS))
			return true;
		return firstTarget.collider.gameObject.tag == other.gameObject.tag;
	}

	private Vector3 FindClosestGrass(){
		grassLocations = GameObject.Find ("GrassLocations").GetComponent<GrassLocation> ().grasses;
		Vector3 closest = new Vector3 ();
		closest = grassLocations [0];
		foreach (Vector3 location in grassLocations){
			if (Vector3.Distance(location, transform.position) < Vector3.Distance(closest, transform.position)) {
				closest = location;
			}
		}
		return closest;
	}
}
