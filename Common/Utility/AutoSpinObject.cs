using UnityEngine;
using System.Collections;

public class AutoSpinObject : MonoBehaviour {

	public Vector3 SpinVector = new Vector3(1, 0, 0);
	private Transform iTransform;

	void Start()
	{
		iTransform = transform;
	}

	void Update()
	{
		iTransform.Rotate(SpinVector * Time.deltaTime);
	}
}
