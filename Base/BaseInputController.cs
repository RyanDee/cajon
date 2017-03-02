using UnityEngine;
using System.Collections;

public class BaseInputController : MonoBehaviour {

	/*
	 * The input controller provide input for use by the PlayerController
	*/

	// Direction
	public bool Up;
	public bool Down;
	public bool Left;
	public bool Right;

	public bool Fire1;

	// Weapon slot
	public bool Slot1;
	public bool Slot2;
	public bool Slot3;
	public bool Slot4;
	public bool Slot5;
	public bool Slot6;
	public bool Slot7;
	public bool Slot8;
	public bool Slot9;

	public float vert;
	public float horz;
	public bool ShouldRespawn;

	public Vector3 tmpVect3;
	private Vector3 zeroVector = Vector3.zero;

	public virtual void CheckInput()
	{
		// Overide with your code to deal with input
		horz = Input.GetAxis("Horizontal");
		vert = Input.GetAxis("Vertical");
	}

	public virtual float GetHorizontal()
	{
		return horz;
	}
		
	public virtual float GetVertical()
	{
		return vert;
	}

	public virtual bool GetFire()
	{
		return Fire1;
	}

	public bool GetRespawn()
	{
		return ShouldRespawn;
	}

	public virtual Vector3 GetMovementDirectionVector()
	{
		// temp vector for movement dir gets set to the value of an otherwise _
		// unused vector that always has the value of 0,0,0
		tmpVect3 = Vector3.zero;

		// if we're going left or right, set the velocity vector's x to our horizontal input value
		if(Left || Right)
		{
			tmpVect3.x = horz;
		}

		// if we're going up or down, set the velocity vector's x to our vertical input value
		if(Up || Down)
		{
			tmpVect3.y = vert;
		}

		return tmpVect3;
	}

	public virtual void LateUpdate()
	{
		CheckInput();
	}

}
