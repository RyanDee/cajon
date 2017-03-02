using UnityEngine;
using System.Collections;
using AIState;

[AddComponentMenu("Base/AI Controller")]
public class BaseAIController : ExtendCustomMonoBehavior {

	// AI states are defined in the AIStates namespace

	private Transform _proxyTarget;
	private Vector3 _relativeTarget;
	private float _targetAngle;
	private RaycastHit _hit;
	private Transform _tempTransform;
	private Vector3 _tempDirVector;

	public float Horz;
	public float Vert;

	private int _obstacleHitType;

	// Editor changeable / visible
	public bool IsStationary;

	public AIStates CurrentAIState;

	public float PatrolSpeed = 5f ;
	public float PatrolTurnSpeed = 10f;
	public float WallAvoidDistance = 40f;

	public Transform FollowTarget;

	public float ModelRotateSpeed = 15f;

	public int FollowTargetMaxTurnAngle = 120;

	public float MinChaseDistance = 2f;
	public float MaxChaseDistance = 10f;
	public float VisionHeightOffset = 1f;

	[System.NonSerialized]
	public Vector3 MoveDirection;

	// Waypoint following related variables
	public WaypointsController iWayController;

	public int currentWaypointNum;

	[System.NonSerialized]
	public Transform currentWaypointTransform;

	private int _totalWaypoints;
	private Vector3 _nodePosition;
	private Vector3 _iPosition;
	private Vector3 _diff;
	private float _currentWayDist;

	[System.NonSerialized]
	public bool ReachedLastWaypoint;
	private Vector3 _moveVec;
	private Vector3 _targetMoveVec;
	private float _distanceToChaseTarget;

	public float WaypointDistance = 5f;
	public float moveSpeed = 30f;
	public float pathSmoothing = 2f;
	public bool ShouldReversePathFollowing;
	public bool LoopPath;
	public bool DestroyAtEndOfWaypoints;

	public bool FaceWaypoints;
	public bool StartAtFirstWaypoint;
	[System.NonSerialized]
	public bool IsRespawning;

	private int ObstacleFinderResult;
	public Transform RotateTransform;

	[System.NonSerialized]
	public Vector3 RelativeWaypointPosition;

	public bool AIControlled;

	public void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		// Cache ref to GameObject
		iGameobject = gameObject;

		// Cache ref to transform
		iTransform = transform;

		// RotateTransform may be set if the object we rotate is different to the main transform
		if(RotateTransform == null)
		{
			RotateTransform = iTransform;
		}

		// Cache a ref to our rigidbody
		iRigid = iTransform.GetComponent<Rigidbody>();

		didInit = true;
	}

	public void SetAIControl(bool state)
	{
		AIControlled = state;
	}

	// Set up AI parameters
	#region Set Up AI parameters
	public void SetPatrolSpeed(float num)
	{
		PatrolSpeed = num;
	}

	public void SetPatrolTurnSpeed(float num)
	{
		PatrolTurnSpeed = num;
	}

	public void SetWallAvoidDistance(float num)
	{
		WallAvoidDistance = num;
	}

	public void SetMoveSpeed(float num)
	{
		moveSpeed = num;
	}

	public void SetMinChaseDistance(float num)
	{
		MinChaseDistance = num;
	}

	public void SetMaxChaseDistance(float num)
	{
		MaxChaseDistance = num;
	}

	public void SetPathSmoothing(float num)
	{
		pathSmoothing = num;
	}
	#endregion

	public virtual void SetAIState(AIStates newState)
	{
		// Update AI state
		CurrentAIState = newState;
	}

	public virtual void SetChaseTarget(Transform rTransform)
	{
		// Set a target for this AI to chase, if required
		FollowTarget = rTransform;
	}

	public virtual void Update()
	{
		// Make sure we have initialized before doing anything
		if(!didInit)
		{
			Init();
		}

		// Check to see if we're supposed to be controlling the player
		if(!AIControlled)
		{
			return;
		}

		UpdateAI();
	}

	public virtual void UpdateAI()
	{
		// Reset our inputs
		Horz = 0;
		Vert = 0;

		int obstacleFinderResult = IsObstacleAhead();

		switch(CurrentAIState)
		{
		case AIStates.moving_looking_for_target:
			// Looking for chase target
			if(FollowTarget != null)
			{
				LookAroundFor(FollowTarget);

			}
			// The AvoidWalls function looks to see if there's anything in front. If there is, it
			// will automatically change the value of moveDirection before we do the actual move
			if(obstacleFinderResult == 1)
			{
				// Go LEFT
				SetAIState(AIStates.stopped_turning_left);
			}
			else if(obstacleFinderResult == 2)
			{
				// Go RIGHT
				SetAIState(AIStates.stopped_turning_right);
			}
			else if(obstacleFinderResult == 3)
			{
				// BACK UP
				SetAIState(AIStates.backing_up_looking_for_target);
			}

			// All clear! head forward
			MoveForward();
			break;
		case AIStates.chasing_target:
			// Chasing in case mode, we point toward the target and go right at it!
			// Quick check to make sure that we have a target (if not, we drop back to patrol mode)
			if(FollowTarget == null)
			{
				SetAIState(AIStates.moving_looking_for_target);
			}

			// The TurnTowardTarget function does just that, so to chase we just throw it the current target
			TurnTowardTarget(FollowTarget);

			// Find the distance between us and the chase target to see if it is within range
			_distanceToChaseTarget = Vector3.Distance(iTransform.position, FollowTarget.position);

			// Check the range
			if(_distanceToChaseTarget > MinChaseDistance)
			{
				// Keep charging forward
				MoveForward();
			}

			// Here we do a quick check to test the distance between AI and target.
			// If it's higher than our MaxChaseDistance variable, we drop out og chase mode and go back to patrolling
			if(_distanceToChaseTarget > MaxChaseDistance || CanSee(FollowTarget) == false)
			{
				SetAIState (AIStates.moving_looking_for_target);
			}
			break;
		case AIStates.backing_up_looking_for_target:
			// Look for chase target
			if (FollowTarget != null)
			{
				LookAroundFor(FollowTarget);
			}

			MoveBack();

			if(obstacleFinderResult == 0)
			{
				// Backup - let's randomize whether to go left or right
				if(Random.Range(0,100) > 50)
				{
					SetAIState(AIStates.stopped_turning_left);
				}
				else
				{
					SetAIState(AIStates.stopped_turning_right);
				}
			}
			break;
		case AIStates.stopped_turning_left:
			// Look for chase target
			if(FollowTarget != null)
			{
				LookAroundFor(FollowTarget);
			}

			// Stopped, turning left
			TurnLeft();

			if(obstacleFinderResult == 0)
			{
				SetAIState(AIStates.moving_looking_for_target);
			}
			break;
		case AIStates.stopped_turning_right:
			// Look for chase target
			if(FollowTarget != null)
			{
				LookAroundFor(FollowTarget);
			}

			// Stooped, turning right
			TurnRight();

			// Check results from looking, to see if path ahead is clear
			if(obstacleFinderResult == 0)
			{
				SetAIState(AIStates.moving_looking_for_target);
			}
			break;
		case AIStates.paused_looking_for_target:
			// Standing still, with looking for chase target 
			// Look for chasing target
			if(FollowTarget != null)
			{
				LookAroundFor(FollowTarget);
			}
			break;
		case AIStates.translate_along_waypoint_path:
			// Following waypoints (moving toward them, not pointing at them) at the speed of moveSpeed
			// Make sure we have been initialized before trying to accsss waypoints
			if(!didInit && ReachedLastWaypoint) 
			{
				return;
			}

			UpdateWaypoints();

			// Move the ship
			if(!IsStationary)
			{
				_targetMoveVec = Vector3.Normalize(currentWaypointTransform.position - iTransform.position);
				_moveVec = Vector3.Lerp(_moveVec, _targetMoveVec, Time.deltaTime * pathSmoothing);
				iTransform.Translate(_moveVec * moveSpeed * Time.deltaTime);
				MoveForward();
				if(FaceWaypoints)
				{
					TurnTowardTarget(currentWaypointTransform);
				}
			}
			break;
		case AIStates.steer_to_waypoint:
			// Make sure we have been inintialized before trying to access waypoints
			if(!didInit && !ReachedLastWaypoint)
			{
				return;
			}

			UpdateWaypoints();

			if(currentWaypointTransform == null)
			{
				return;
			}

			// Now we just find the relative position of the waypoint from the car transform, 
			// that we can determine how far to the left and right the waupoints is.
			RelativeWaypointPosition = transform.InverseTransformPoint(currentWaypointTransform.position);

			// By driving the horz position by the magnitude we et a decimal percentage of the turn angle that
			// we can use to drive the wheels
			Horz = (RelativeWaypointPosition.x/RelativeWaypointPosition.magnitude);

			// Now we do the same for torque, but make sure that it doesn't apply any engine torque when going around a sharp turn...
			if(Mathf.Abs(Horz) < 0.5f)
			{
				Vert = RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude - Mathf.Abs(Horz);
			}
			else
			{
				NoMove();
			}
			break;
		case AIStates.steer_to_target:
			// Make sure we have been initialized before trying to access waypoints
			if(!didInit)
			{
				return;
			}

			if(FollowTarget != null)
			{
				// It may be possible that this function gets called before a target has been setup, so
				// we catch any nulls here
				return;
			}

			// Now we just find the relative position of the waypoint from the car transform, that way we can determine how far to the left
			// and right the waypoint is.
			RelativeWaypointPosition = transform.InverseTransformPoint(FollowTarget.position);

			// By driving the horz position by the magnitude we et a decimal percentage of the turn angle that
			// we can use to drive the wheels
			Horz = (RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude);


			// If we're outside of the minimum chase distance drive
			if(Vector3.Distance(FollowTarget.position, iTransform.position) > MinChaseDistance)
			{
				MoveForward();
			}
			else
			{
				NoMove();
			}

			if(FollowTarget != null)
				LookAroundFor(FollowTarget);

			// The AvoidWall function looks to see if there's anything in front. If there is, 
			// it will automatically change the value of moveDirection before we do the actual move
			if(obstacleFinderResult == 1)
			{
				TurnLeft();
			}
			else if(obstacleFinderResult == 2)
			{
				TurnRight();
			}
			else if (obstacleFinderResult == 3)
			{
				MoveBack();
			}
			break;
		case AIStates.paused_no_target:
			// paused no target
			break;
		default:
			// idle (do nothing)
			break;

		}
	}

	public virtual void TurnLeft()
	{
		Horz = -1;
	}

	public virtual void TurnRight()
	{
		Horz = 1;
	}

	public virtual void MoveForward()
	{
		Vert = 1;
	}

	public virtual void MoveBack()
	{
		Vert = -1;
	}

	public virtual void NoMove()
	{
		Vert = 0;
	}
	public virtual void LookAroundFor(Transform refTransform)
	{
		// Here we do a quick check to test the distance between AI and target. If it's higher 
		// than our MaxChaseDistance variable, we drop out of chase mode and go back to patrolling
		if (Vector3.Distance(iTransform.position, refTransform.position ) < MaxChaseDistance)
		{
			// Check to see if the target is visible before
			if(CanSee(FollowTarget) == true)
			{
				// Set our state to chase the target
				SetAIState(AIStates.chasing_target);
			}
		}
	}

	private int _obstacleFinding;

	public virtual int IsObstacleAhead()
	{
		_obstacleHitType = 0;

		// Quick check to make sure that iTransform has been set
		if(iTransform == null)
		{
			return 0;
		}

		// Draw this raycast so we can see what it is doing
		Debug.DrawRay(iTransform.position,((iTransform.forward + (iTransform.right * 0.5f))* WallAvoidDistance));
		Debug.DrawRay(iTransform.position,((iTransform.forward + (iTransform.right * -0.5f))* WallAvoidDistance));

		// Case a ray out forward from our AI and put the 'result' into the variable named hit
		if(Physics.Raycast(iTransform.position, iTransform.forward + (iTransform.right * 0.5f), out _hit, WallAvoidDistance))
		{
			// Obstacle 
			// It's a left hit, so it's a type 1 right now (though it could change when we check on the other side)
			_obstacleHitType = 1;
		}

		if(Physics.Raycast(iTransform.position, iTransform.forward + (iTransform.right * -0.5f), out _hit, WallAvoidDistance))
		{
			if(_obstacleHitType == 0)
			{
				// If we haven't hit anything yet, this is a type 2
				_obstacleHitType = 2;
			}
			else
			{
				// If we have hits on both left and right raycasts, it's a type 3
				_obstacleHitType = 3;
			}
		}
		return _obstacleHitType;
	}

	public void TurnTowardTarget(Transform refTarget)
	{
		if(refTarget == null)
		{
			return;
		}

		// Calculate the target position relative to the target of this transform's coordinate system.
		// eg a positive x value means the target is to to the right of the car, a positive z means the
		// target is in front of the car
		_relativeTarget = RotateTransform.InverseTransformPoint(refTarget.position);

		// Calculate the target angle
		_targetAngle = Mathf.Atan2(_relativeTarget.x, _relativeTarget.z);

		// Atan returns the angle in radians, convert to degrees
		_targetAngle *= Mathf.Rad2Deg;

		// The wheels should have a maxinum rotation angle
		_targetAngle  = Mathf.Clamp(_targetAngle, - FollowTargetMaxTurnAngle - _targetAngle, FollowTargetMaxTurnAngle);

		// Turn towards the target at the rate of model Rotate Speed
		RotateTransform.Rotate(0, _targetAngle * ModelRotateSpeed * Time.deltaTime, 0);
	}

	public bool CanSee(Transform refTarget)
	{
		// First, let's get a vector to use for raycasting bu subtracting the target position
		// from our AI position
		tempVector = Vector3.Normalize(refTarget.position - iTransform.position);

		// Let's have a debug line to check the distane between the to manually, in case you run into trouble!
		Debug.DrawLine(iTransform.position, refTarget.position);

		// Cast a ray from our AI, out toward the target passed in (use the tempDirVec magnitude as the distance to cast
		if(Physics.Raycast(iTransform.position + (VisionHeightOffset * iTransform.up), _tempDirVector, out _hit, MaxChaseDistance))
		{
			// Check to see if we hit the target
			if(_hit.transform.gameObject.layer == refTarget.gameObject.layer)
			{
				return true;
			}
		}

		// Nothing found, so return false;
		return false;
	}

	public void SetWayController( WaypointsController wayControl)
	{
		iWayController = wayControl;
		wayControl = null;

		// Grab total waypoint
		_totalWaypoints = iWayController.GetTotal();

		// Make sure that if you use SetRerversePath to set shouldReversePathFollowing that you call
		// SetReversePath for the first time Before SetWayController, otherwise it won't set the first
		// waypoint correcly
		if(ShouldReversePathFollowing)
		{
			currentWaypointNum = _totalWaypoints - 1;
		}
		else
		{
			currentWaypointNum = 0;
		}

		Init();

		// Get the first waypoint from the waypoint controller
		currentWaypointTransform = iWayController.GetWaypoint(currentWaypointNum);

		if(StartAtFirstWaypoint)
		{
			// Position at the currentwaypointTransform position
			iTransform.position = currentWaypointTransform.position;
		}
	}

	public void SetReversePath(bool shouldRev)
	{
		ShouldReversePathFollowing = shouldRev;
	}

	public void SetSpeed(float speed)
	{
		moveSpeed = speed;
	}

	public void SetPathSmoothingRate(float rate)
	{
		pathSmoothing = rate;
	}

	public void SetRotateSpeed(float rate)
	{
		ModelRotateSpeed = rate;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (transform.position, WaypointDistance);
	}

	void UpdateWaypoints()
	{
		// If we dont't have a waypoint controller, we sefely drop out
		if(iWayController == null)
		{
			return;
		}

		if(ReachedLastWaypoint && DestroyAtEndOfWaypoints)
		{
			// Self Destroy
			Destroy(gameObject);
			return;
		}
		else if( ReachedLastWaypoint)
		{
			currentWaypointNum = 0;
			ReachedLastWaypoint = false;
		}

		// Because of the oder that scripts run and are inititalzed, it is possible for this function to bell called
		// befor we have actually finished running the waypoints initialization, wich means we need to drop out to avoid
		// doing anything silly or before it breaks the game.
		if(_totalWaypoints == 0)
		{
			// Grab total waypoints
			_totalWaypoints = iWayController.GetTotal();
			return;
		}

		if(currentWaypointTransform == null)
		{
			// Grab our transform reference from the waypoint controller
			currentWaypointTransform = iWayController.GetWaypoint(currentWaypointNum);
		}

		// Now we check to see if we are close enough to the current waypoints to advance on to the next one
		_iPosition = iTransform.position;
		_iPosition.y = 0;

		// Get waypoint position and 'flatten' it
		_nodePosition = currentWaypointTransform.position;
		_nodePosition.y = 0;

		// Cehck distancefrom this to the waypoint

		_currentWayDist = Vector3.Distance(_nodePosition, _iPosition);

		if(_currentWayDist < WaypointDistance)
		{
			// We are close to the curent node, so let's move on to the next one
			if(ShouldReversePathFollowing)
			{
				currentWaypointNum --;
				// Now check to see if we have been all the way around
				if(currentWaypointNum < 0)
				{
					// Just in case it gets referenced before we are destroyed, let's keep it to a safe index number
					currentWaypointNum = 0;

					// Completed the route
					ReachedLastWaypoint = true;

					// If we are set to loop, reset the currentwaypointnum to 0
					if(LoopPath)
					{
						currentWaypointNum = _totalWaypoints;

						// The route keeps going in a loop, so we don't want reachedLastWaypoint to ever become true
						ReachedLastWaypoint = false;
					}

					return;
				}
			}
			else
			{
				currentWaypointNum++;

				// Now check to see if we have benn all the way around
				if(currentWaypointNum > _totalWaypoints)
				{
					// Completed the route
					ReachedLastWaypoint = true;

					// If we are set to loop, reset the currentwaypointnum to 0
					if(LoopPath)
					{
						currentWaypointNum = 0;

						// The route keeps going in a loop, so we don't want reachedLastWaypoint to ever become true
						ReachedLastWaypoint = false;
					}

					return;
				}
			}

			// Grab our transform refernce from the waypoint controller
			currentWaypointTransform  = iWayController.GetWaypoint(currentWaypointNum);
		}
	}

	public float GetHorizontal()
	{
		return Horz;
	}

	public float GetVertical()
	{
		return Vert;
	}
}

