using UnityEngine;
using System.Collections;

public class BaseTopDown : ExtendCustomMonoBehavior {

	public AnimationClip IdleAnimation;
	public AnimationClip WalkAnimation;

	public float WalkMaxAnimationSpeed = 0.75f;
	public float RunMaxAnimationSpeed = 1.0f;

	// When did the user start walking  (Used for going into run after a while)
	private float _walkTimeStart = 0.0f;

	// Wee've made the following variable public so the we can use an animation on 
	// a different gameObject if needed

	public Animation iAnimation;

	enum CharacterStates {
		Idle = 0,
		Walking = 1,
		Running = 2
	}
			
	private CharacterStates _characterState;

	// The speed when walking
	public float walkSpeed = 2.0f;

	// after runAfterSeconds of walking we run with runSpeed
	public float runSpeed = 4.0f;

	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	public float runAfterSeconds = 3.0f;

	// The current move direction in x-z
	private Vector3 _moveDirection = Vector3.zero;

	// The current vertical speed
	private float verticalSpeed = 0.0f;

	// The current x-z move speed
	public float moveSpeed = 0.0f;

	// The last collision flags returned from controller.Move
	private CollisionFlags _collisionFlags;

	public BasePlayerManager iPlayerController;

	[System.NonSerialized]
	public Keyboard_Input default_input;

	public float vert;
	public float horz;

	private CharacterController _controller;

	//========================================================
	void Awake ()
	{
		// we need to do this before anything happens to the script or object, so it happens in Awake
		// if you need to add specific set-up, consider adding it to the Init() function instead to keep this function
		// limited only to things we need to do before anything else happens

		_moveDirection = transform.TransformDirection(Vector3.forward);

		//if _animation has not been set up in the inspector, we will try to find it on the current gameObject
		if(iAnimation == null)
		{
			iAnimation = GetComponent<Animation>();
		}

		if(!iAnimation)
		{
			Debug.Log("The character you would like to control doesn't have animation. Moving have might look weird.");
		}
		if(!IdleAnimation)
		{
			iAnimation = null;
			Debug.Log("No idle animation found. Turning off animation.");
		}
		if(!WalkAnimation)
		{
			iAnimation = null;
			Debug.Log("No walk animation found. Turning og animations");
		}

		_controller = GetComponent<CharacterController>() ;
	}

	public virtual void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		// cache te usual suspects
		iRigid = GetComponent<Rigidbody>();
		iGameobject = gameObject;
		iTransform = transform;

		// add default keyboard input
		default_input = iGameobject.AddComponent<Keyboard_Input>();

		// cache a reference to the player controller
		iPlayerController = iGameobject.GetComponent<BasePlayerManager>();
		if(iPlayerController!=null)
		{
			iPlayerController.Init();
		}
	}

	public void SetUserInput(bool setInput)
	{
		canControl = setInput;
	}

	public virtual void GetInput()
	{
		horz = Mathf.Clamp(default_input.GetHorizontal(), -1, 1);
		vert = Mathf.Clamp(default_input.GetVertical(), -1, 1);
	}

	public virtual void LateUpdate()
	{
		//we check for input in LateUpdate because Unity recommends this
		if(canControl)
		{
			GetInput();
		}
	}

	public bool moveDirectionally;

	private Vector3 _targetDirection;
	private float _curSmooth;
	private float _targetSpeed;
	private float _curSpeed;
	private Vector3 _forward;
	private Vector3 _right;

	void UpdateSmoothedMovementDirction()
	{
		if(moveDirectionally)
		{
			UpdateDirectionalMovement();
		}
		else
		{
			UpdateRotationMovement();
		}
	}
		
	void UpdateDirectionalMovement()
	{
		// find target direction
		_targetDirection = horz * Vector3.right;
		_targetDirection += vert * Vector3.forward;
		if(_targetDirection != Vector3.zero)
		{
			_moveDirection = Vector3.RotateTowards(_moveDirection, _targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
			_moveDirection = _moveDirection.normalized;
		}

		// Smooth the speed based on the current target direction
		_curSmooth = speedSmoothing * Time.deltaTime;

		// Chose target speed
		// We want to support analog input but make sure you can't walk faster diagonaly than just forward or slidways
		_targetSpeed = Mathf.Min(_targetDirection.magnitude, 1.0f);

		_characterState = CharacterStates.Idle;

		//cecide on animation state and adjust move speeds
		if(Time.time - runAfterSeconds > _walkTimeStart)
		{
			_targetSpeed *= runSpeed;
			_characterState = CharacterStates.Running;
		}
		else
		{
			_targetSpeed *= walkSpeed;
			_characterState = CharacterStates.Walking;
		}

		moveSpeed = Mathf.Lerp(moveSpeed, _targetSpeed, _curSmooth);

		// Reset walk time start when we slow down
		if(moveSpeed < walkSpeed * 0.3f)
		{
			_walkTimeStart = Time.time;
		}

		// Calculate actual motion
		Vector3 movement = _moveDirection * moveSpeed;
		movement *= Time.deltaTime;

		// Move the controller
		_collisionFlags = _controller.Move(movement);

		// Set rotation to the move direction
		iTransform.rotation = Quaternion.LookRotation(_moveDirection);
	}

	void UpdateRotationMovement()
	{
		// This character movement is based on the code in the Unity help file for CharacterController.SimpleMove
		iTransform.Rotate(0, horz * rotateSpeed * Time.deltaTime, 0);
		_curSpeed = moveSpeed * vert;
		_controller.SimpleMove(iTransform.forward * _curSpeed);

		// Target direction (the max we want to move, used for calculating target speed)
		_targetDirection = vert * iTransform.forward;

		// Smooth the  speed based on the current target direction
		float curSmooth = speedSmoothing * Time.deltaTime;

		// Choose target speed
		// We want to supoert analog input but make sure you can't walk faseter diagonally than just forward or sideways.
		_targetSpeed = Mathf.Min(_targetDirection.magnitude, 1.0f);

		_characterState = CharacterStates.Idle;

		// decide on animation state and adjust move speed
		if(Time.time - runAfterSeconds > _walkTimeStart)
		{
			_targetSpeed *= runSpeed;
			_characterState = CharacterStates.Running;
		}
		else
		{
			_targetSpeed *= walkSpeed;
			_characterState = CharacterStates.Walking;
		}

		moveSpeed = Mathf.Lerp(moveSpeed, _targetSpeed, curSmooth);

		// Reset walk time start when we slow down
		if(moveSpeed < walkSpeed * 0.3f)
		{
			_walkTimeStart = Time.time;
		}
	}

	void Update ()
	{
		if(!canControl)
		{
			// kill all input if not cotrollable.
			Input.ResetInputAxes();
		}

		UpdateSmoothedMovementDirction();

		//Animation
		if(!iAnimation)
		{
			if(_controller.velocity.sqrMagnitude < 0.1f)
			{
				iAnimation.CrossFade(IdleAnimation.name);
			}
			else
			{
				if(_characterState == CharacterStates.Running)
				{
					iAnimation[WalkAnimation.name].speed = Mathf.Clamp(_controller.velocity.magnitude, 0.0f, RunMaxAnimationSpeed);
					iAnimation.CrossFade(WalkAnimation.name);
				}
				else if(_characterState == CharacterStates.Walking)
				{
					iAnimation[WalkAnimation.name].speed = Mathf.Clamp(_controller.velocity.magnitude, 0.0f, WalkMaxAnimationSpeed);
				}
			}
		}
	}

	public float GetSpeed ()
	{
		return moveSpeed;
	}

	public Vector3 GetDirection ()
	{
		return _moveDirection;
	}

	public bool IsMoving ()
	{
		return Mathf.Abs(vert) + Mathf.Abs(horz) > 0.5f;
	}

	public void Reset ()
	{
		gameObject.tag = "Player";
	}


}
