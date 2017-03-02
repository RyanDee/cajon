using UnityEngine;
using System.Collections;

public class MouseInput : BaseInputController {

	private Vector2 _prevMousePos;
	private Vector2 _mouseDelta;

	private float speedX = 0.05F;
	private float speedY = 0.1F;

	public void Start()
	{
		_prevMousePos = Input.mousePosition;
	}

	public override void CheckInput()
	{
		// get input from vertical and horizontal axis and store in vert and horz , so _
		// we don't hove to access them every time we need to relay input data out

		// calculate a percentage amount to use per pixel;
		float scalerX = 100f / Screen.width;
		float scalerY = 100f / Screen.height;

		// calculate and use deltas
		float mouseDeltaY = Input.mousePosition.y - _prevMousePos.y;
		float mouseDeltaX = Input.mousePosition.x - _prevMousePos.x;

		// scale base on screen size
		horz += (mouseDeltaX * speedX) * scalerX;
		vert += (mouseDeltaY * speedY) * scalerY;

		// store this mouse position for the next time we're here
		_prevMousePos = Input.mousePosition;

		// setup some Boolean values for up, down, left and right
		Up = (vert > 0);
		Down = (vert < 0);
		Left = (horz < 0);
		Right = (horz > 0);

		//get fire-action button
		Fire1 = Input.GetButton("Fire1");
	}

	public void LateUpdate()
	{
		// check inputs each LateUpdate() ready for the next tick.
		CheckInput();
	}

}
