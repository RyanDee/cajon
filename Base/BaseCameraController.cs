using UnityEngine;
using System.Collections;

/* BaseCameraController 
 * Lớp cơ sở cho các lớp camera khác
 */

[AddComponentMenu("Base/Camera Controller")]
public class BaseCameraController : MonoBehaviour {

	/* Set target by Inspector Windows */
	public Transform CameraTarget;

	/* Set target by code */
	public virtual void SetTarget(Transform target)
	{
		CameraTarget = target;
	}

}