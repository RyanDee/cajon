using UnityEngine;
using System.Collections;

public class BaseVehicle : ExtendCustomMonoBehavior {

	public WheelCollider FrontWheelLeft;
	public WheelCollider FrontWheelRight;
	public WheelCollider rearWheelLeft;
	public WheelCollider rearWheelRight;

	public float SteerMax = 30f;
	public float AccelMaxx = 5000f;
	public float brakeMax = 5000f;
	public float Steer = 0f;
	public float Motor = 0f;
	public float Brake = 0f;
	public float ISpeed;
	public bool isLocked;

	[System.NonSerialized]
	public Vector3 Velocity;

	[System.NonSerialized]
	public Vector3 FlatVelocity;

	public BasePlayerManager PlayerController;

	[System.NonSerialized]
	public Keyboard_Input default_input;

	public AudioSource engineSoundSource;

	public virtual void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		// Cache the usual suspects
		iRigid = GetComponent<Rigidbody>();
		iGameobject = gameObject;
		iTransform = transform;

		// Add default keyboard input
		default_input = iGameobject.GetComponent<Keyboard_Input>();

		// Cache a reference to player controller
		PlayerController = iGameobject.GetComponent<BasePlayerManager>();

		// Call base class init
		PlayerController.Init();

		// With this simple vehicle code, we set the center of mass low to try to keep the car from toppling over
		iRigid.centerOfMass = new Vector3(0, -4f, 0);

		// See if we cant find an engine sound source, if we need to
		if(engineSoundSource == null)
		{
			engineSoundSource = iGameobject.GetComponent<AudioSource>();
		}
	}

	public void SetUserInput(bool setInput)
	{
		canControl = setInput;
	}

	public void SetLock(bool lockState)
	{
		isLocked = lockState;
	}

	public virtual void LateUpdate()
	{
		// We check for input in LateUpdate because Unity recommends this
		if(canControl)
		{
			GetInput();
		}

		// Update the audio
		UpdateEngineAudio();
	}

	public virtual void FixedUpdate()
	{
		UpdatePhysics();
	}

	public virtual void UpdateEngineAudio()
	{
		// This is just 'made up' multiplayer value applied to ISpeed
		engineSoundSource.pitch = 0.5f + (Mathf.Abs(ISpeed)* 0.005f);
	}

	public virtual void UpdatePhysics()
	{
		CheckLock();
	}

	public void CheckLock()
	{
		if(isLocked)
		{
			// Control is locked out and we shuold be stopped
			Steer = 0;
			Brake = 0;
			Motor = 0;

			// Hold our rigidbody in place (but allow the Y to move so the car may drop to the ground 
			// if it is not exacly matched to the terrain)
			Vector3 tempV = iRigid.velocity;
			tempV.x = 0;
			tempV.z = 0;
			iRigid.velocity = tempV;
		}
	}

	public virtual void GetInput()
	{
		// Calculate steering amout
		Steer = Mathf.Clamp(default_input.GetHorizontal(), -1, 1);

		// How much Accelerator
		Motor = Mathf.Clamp(default_input.GetVertical(), 0, 1);

		// How much Brake
		Brake = -1 * Mathf.Clamp(default_input.GetVertical(), -1, 0);
	}
}
