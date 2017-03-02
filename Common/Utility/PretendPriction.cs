using UnityEngine;
using System.Collections;

public class PretendPriction : MonoBehaviour {

	private Rigidbody iRigid;
	private Transform iTransform;
	private float iMass;
	private float slideSpeed;
	private Vector3 velocity;
	private Vector3 flatVelo;
	private Vector3 iRight;
	private Vector3 tmpVec3;

	public float theGrip = 100f;

	// Use this for initialization
	void Start () {
		//cache some refereneces to our rigidbody, mass and transform
		iRigid = GetComponent<Rigidbody>();
		iMass = iRigid.mass;
		iTransform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//grab the values we need to calculate grip
		iRight = iTransform.right;

		//calculate flat velocity
		velocity = iRigid.velocity;
		flatVelo.x  = velocity.x;
		flatVelo.y  = 0;
		flatVelo.z  = velocity.z;

		//calculate how much we are sliding
		slideSpeed = Vector3.Dot(iRight,flatVelo);

		//build a new vector to compensate for the sliding
		tmpVec3 = iRight * (slideSpeed * iMass * iMass * theGrip);

		//apply the correctional force to the rigidbody
		iRigid.AddForce(tmpVec3 * Time.deltaTime);

	}
}
