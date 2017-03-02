using UnityEngine;
using System.Collections;

public class Keyboard_Input : BaseInputController 
{
	public override void CheckInput()
	{
		/* Get input data from vertical and horizontal axis and store them internally in vert
		 * and horz so we don't have to access them every time we need to relay input data out
		 */

		// Overide with your code to deal with input
		horz = Input.GetAxis("Horizontal");
		vert = Input.GetAxis("Vertical");

		// Setup some boolean values for up, down, left and right
		Up 	=	(vert > 0);
		Down 	=	(vert < 0 );
		Left	=	(horz < 0);
		Right	=	(horz >0 );

		//get fire/action buttons
		Fire1 = Input.GetButton("Fire1");
		ShouldRespawn = Input.GetButton("Fire3");
	}

	public void LateUpdate()
	{
		// check inputs each LateUpdate() ready for the next tick CheckInput();
		CheckInput();
	}
}
