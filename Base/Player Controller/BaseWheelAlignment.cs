using UnityEngine;
using System.Collections;

public class BaseWheelAlignment : MonoBehaviour {

	// Define the variable used in the script, the Corresponding collider is the wheel collider
	// at the position of the visible wheel, the slip prefab is the prefab instantiated when wht wheels slide,
	// the rotation value used to rotate the wheel around its axle.

	public WheelCollider CorrespondingCollider;
	public GameObject SlipPrefab;
	public float SlipAmountForTireSmoke = 50f;

	private float RotationValue = 0.0f;
	private Transform iTransform;
	private Quaternion zeroRotation;
	private Transform colliderTransform;
	private float SuspendsionDistance;

	void Start()
	{
		// Cache some commonly used things
		iTransform = transform;
		zeroRotation = Quaternion.identity;
		colliderTransform = CorrespondingCollider.transform;
	}

	void Update()
	{
		// Define a hit point for the raycase collision
		RaycastHit hit;
		// Find the collider's center point, you need to do this because the center of the collider might not actually 
		// be the real position it the transform's off.
		Vector3 ColliderCenterPoint = colliderTransform.TransformPoint(CorrespondingCollider.center);

		// Now cast a ray out from the wheel collider's center the distance of the suspension, if it hit something, then use
		// the "hit", variable's data to find where the wheel hit, if it didn't, then set the wheel to be fully extended along
		// the suspension
		if (Physics.Raycast(ColliderCenterPoint, -colliderTransform.up, out hit, CorrespondingCollider.suspensionDistance + CorrespondingCollider.radius))
		{
			iTransform.position = hit.point + (colliderTransform.up * CorrespondingCollider.radius);
		}
		else
		{
			iTransform.position = ColliderCenterPoint - (colliderTransform.up * CorrespondingCollider.suspensionDistance);
		}

		// Now set the wheel rotation to the rotation of the collider comined with a new rotation value.
		// This new value is the roatation around the axle, and the rotation from steering input.
		iTransform.rotation = colliderTransform.rotation * Quaternion.Euler(RotationValue, CorrespondingCollider.steerAngle, 0);

		// Increase the rotation value by the rotation speed (in degrees per second)
		RotationValue += CorrespondingCollider.rpm * (360/60)* Time.deltaTime;

		// Define a whiell hit object, this stores all of the data from the wheel collider and will allow us to determine the slip
		// of the tire.

		WheelHit correspondingGroundHit = new WheelHit();
		CorrespondingCollider.GetGroundHit(out correspondingGroundHit);

		// If the slip of the tire is greater than 2.0f, and the slip prefab exists, create an instance of it on the ground a zero ratation.
		if(Mathf.Abs(correspondingGroundHit.sidewaysSlip) > SlipAmountForTireSmoke){
			if(SlipPrefab){
				SpawnController.Instance.Spawn(SlipPrefab, correspondingGroundHit.point, zeroRotation);
			}
		}
	}

}
