using UnityEngine;
using System.Collections;

public class SingleAxisKeyboardInput : BaseInputController {

	public override void CheckInput ()
	{
		// get input date from vertical and horizontal axis and store them internally _
		// in vert and horz so we dont't have to accsss them every time we need to relay 
		// input data out
		horz = Input.GetAxis("Horizontal");
		Left = (horz < 0);
		Right = (horz > 0);
		Fire1 = Input.GetButton("Fire1");
	}
}
