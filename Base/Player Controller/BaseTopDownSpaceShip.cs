using UnityEngine;
using System.Collections;

public class BaseTopDownSpaceShip : ExtendCustomMonoBehavior {

	private Quaternion targetPostion;

	private float thePos;
	private float moveXAmount;
	private float moveZAmount;

	public float moveXSpeed = 40f;
	public float moveZSpeed = 15f;
	public float limitX = 15f;
	public float limitZ = 15f;

	private float orginZ;

	[System.NonSerialized]
	public Keyboard_Input default_input;

	public float horizontal_input;
	public float vertical_input;

	public virtual void Start(){
		//We are overrriding Start() so as not to call Init, as we want to the game controller to do this in this game.
		didInit = false;
		this.Init();
	}

	public virtual void Init(){
		
		//cache refs to our trasform and gameObject
		iTransform = transform;
		iGameobject = gameObject;
		iRigid = gameObject.GetComponent<Rigidbody>();

		//default keyboard input
		default_input = iGameobject.AddComponent<Keyboard_Input>();

		//grab the starting Z position to use as a base line for position limiting
		orginZ = transform.localPosition.z;

		//set a flag so that our Update function knows when we are to use
		didInit = true;

	}

	public virtual void GameStart()
	{
		//lET'S START GAME
		canControl = true;
	}

	public virtual void GetInput()
	{
		//this is just a 'default' function that (if needs be) should be overridden in the glue code
		horizontal_input = default_input.GetHorizontal();
		vertical_input = default_input.GetVertical();
	}

	public virtual void Update()
	{
		UpdateShip();
	}

	public virtual void UpdateShip()
	{
		//don't do anything untial Init() has been run
		if(!didInit){
			return;
		}

		//check to see if we're supposed to be controlling the player before moving it
		if(!canControl)
			return;
		
		GetInput();

		//calculate movement amouts for X and Z axis
		moveXAmount = horizontal_input * Time.deltaTime * moveXSpeed;
		moveZAmount = vertical_input * Time.deltaTime * moveZSpeed;

		Vector3 tmpRotation = iTransform.eulerAngles;
		tmpRotation.z = horizontal_input * -30f;
		iTransform.eulerAngles = tmpRotation;

		//move out transform to its updated position
		iTransform.localPosition += new Vector3(moveXAmount, 0, moveZAmount);

		if(iTransform.localPosition.x <= -limitX || iTransform.localPosition.x >= limitX)
		{
			thePos = Mathf.Clamp(iTransform.localPosition.x, -limitX, limitX);
			iTransform.localPosition = new Vector3(thePos, iTransform.localPosition.y, iTransform.localPosition.z);
		};

		if(iTransform.localPosition.z <= orginZ || iTransform.localPosition.z >= limitZ)
		{
			thePos = Mathf.Clamp(iTransform.localPosition.z, orginZ, limitZ);
			iTransform.localPosition = new Vector3(iTransform.localPosition.x,iTransform.localPosition.y, thePos);
		}
			
	}

}
