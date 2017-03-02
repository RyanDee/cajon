using UnityEngine;
using System.Collections;

/* Topdown camera is a basic target following system from a top-down perspective */

public class CameraTopDown : MonoBehaviour {

	public Transform FollowTarget;
	public Vector3 TargetOffset;
	public float MoveSpeed = 2F;

	private Transform iTransform;

	void Start()
	{
		iTransform = transform;
	}

	public void SetTarget(Transform target)
	{
		this.FollowTarget = target;
	}

	void LateUpdate()
	{
		if(FollowTarget != null)
		{
			iTransform.position = Vector3.Lerp(
						iTransform.position, 
						FollowTarget.position + TargetOffset,
						MoveSpeed * Time.deltaTime
						);
		}
	}

}
