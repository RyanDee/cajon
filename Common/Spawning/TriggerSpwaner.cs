using UnityEngine;
using System.Collections;

/*
 * The TriggerSpawner.cs script instantiates an object when onother object enters the trigger to withch the script is attached.
 * One such use for this script would be to spwan an enemy when player reaches a particular area in a level
 * */

public class TriggerSpwaner : MonoBehaviour {

	public GameObject ObjectToSpawnOnTrigger;
	public Vector3 offsetPosition;
	public bool onlySpawnOnce;
	public int layerToCauseTriggerHit = 13;

	private Transform iTransform;

	void Start()
	{
		Vector3 tempPos = transform.position;
		tempPos.y = Camera.main.transform.position.y;
		transform.position = tempPos;
		//cache Transform
		iTransform = transform;
	}

	void OnTriggerEnter(Collider other)
	{
		// make sure that the player or the object entering our trigger is the once to cause the boss to spwan
		if(other.gameObject.layer != layerToCauseTriggerHit)
			return;

		// instantiate the ojbect to spawn on trigger enter
		Instantiate(ObjectToSpawnOnTrigger, iTransform.position + offsetPosition, Quaternion.identity);

		// if we are to only spawn once, destroy this gameobject after spawn occurs
		if(onlySpawnOnce)
			Destroy (gameObject);
	}

}
