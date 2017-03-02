using UnityEngine;
using System.Collections;

public class CameraThirdPerson : BaseCameraController {

	/* CameraTarget: Base Class
	 * SetTarget()
	*/

	public Transform iTransform;

	public float Distance = 20.0f;
	public float Height = 5.0f;
	public float HeightDamping = 2.0f;

	public float LookAtHeight = 0.0f;
	public float RotationSnapTime = 0.3f;
	public float DistanceSnapTime = 0.1f;

	public Vector3 LookAtAdjustVector;

	private float _usedDistance;

	private float _wantedRotationAngle;
	private float _wantedHeight;

	private float _currentRotationAngle;
	private float _currentHeight;

	private Quaternion _currentRotation;
	private Vector3 _wantedPosition;

	private float _yVelocity = 0.0f;
	private float _zVelocity = 0.0f;

	void Start()
	{
		if (iTransform == null)
		{
			iTransform = transform;
		}	
	}

	void LateUpdate()
	{
		if (CameraTarget == null) return;	
			
		_wantedHeight = CameraTarget.position.y + Height;

		_currentHeight = iTransform.position.y;

		_wantedRotationAngle = CameraTarget.eulerAngles.y;

		_currentRotationAngle = iTransform.eulerAngles.y;

		_currentRotationAngle = Mathf.SmoothDampAngle(_currentRotationAngle, _wantedRotationAngle, ref _yVelocity, RotationSnapTime);

		_currentHeight = Mathf.Lerp(_currentHeight, _wantedHeight, HeightDamping * Time.deltaTime);

		_wantedPosition = CameraTarget.position;

		_wantedPosition.y = _currentHeight;

		_usedDistance = Mathf.SmoothDampAngle(_usedDistance, Distance, ref _zVelocity, DistanceSnapTime);

		_wantedPosition += Quaternion.Euler(0, _currentRotationAngle, 0) * new Vector3(0, 0, - _usedDistance);

		iTransform.position = _wantedPosition;

		iTransform.LookAt(CameraTarget.position + LookAtAdjustVector);

	}

}
