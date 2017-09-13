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
	public Vector3 groundNormal;

    private Animator ani;
	private Transform predator;
	private Rigidbody physics;
	void Start () {
        ani = gameObject.GetComponent<Animator>();
		physics = GetComponent<Rigidbody> ();

    }
	
	// Update is called once per frame
	void Update () {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Idle"))
            IdleHandler();
        if (canSeePredator) {
            Debug.Log("Can see ");
			SeePredatorHandler ();
        }
		
	}

    private void IdleHandler () {
		Debug.Log ("Sniffing in the air");
		physics.velocity = Vector3.zero;
    }

	private void SeePredatorHandler(){
		physics.velocity = -predator.transform.position;
		Vector3 targetSpeed = physics.velocity * RUN_SPEED;
		physics.velocity = Vector3.ProjectOnPlane (targetSpeed, groundNormal);
	}

    private void OnTriggerEnter (Collider target) {
        if (target.gameObject.tag != "Terrain") {
			if (target.gameObject.tag == "Predator" && LOSClear (target.transform)) {
				canSeePredator = true;
				predator = target.transform;
				ani.SetBool ("canSeePredator", true);
			}
        }
    }

    private void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Predator"){
			canSeePredator = false;
			ani.SetBool("canSeePredator", false);
		}
    }

    private bool LOSClear (Transform other) {
        RaycastHit firstTarget;
        Vector3 delta = transform.position - other.position;
        if (!Physics.SphereCast(transform.position, RADIUS, transform.forward, out firstTarget, delta.magnitude - RADIUS))
            return true;
        return firstTarget.collider.gameObject.tag == other.gameObject.tag;
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
}
