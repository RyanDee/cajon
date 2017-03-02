using UnityEngine;
using System.Collections;

public class ExtendCustomMonoBehavior : MonoBehaviour {

	/*
	 * This class is used to add some common variables to MonoBehavior,
	 * rather than constantly repeating the same declaration in every class.
	*/

	public Transform iTransform;
	public GameObject iGameobject;
	public Rigidbody iRigid;

	public bool didInit;
	public bool canControl;

	public int id;

	[System.NonSerialized]
	public Vector3 tempVector;

	[System.NonSerialized]
	public Transform tempTransform;

	public virtual void SetID (int anID)
	{
		id = anID;
	}

}
