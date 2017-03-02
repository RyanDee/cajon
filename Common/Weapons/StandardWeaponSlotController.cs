using UnityEngine;
using System.Collections;

public class StandardWeaponSlotController : BaseWeaponController {

	public bool allowWeaponSwitchKeys= true;

	public void Update()
	{
		if(!allowWeaponSwitchKeys)
			return;

		// Do weapon selection / switching slots
		// ---------------------------------------

		if(Input.GetKey("1"))
		{
			SetWeaponSlot(0);
		}

		if(Input.GetKey("2"))
		{
			SetWeaponSlot(1);
		}

		if(Input.GetKey("3"))
		{
			SetWeaponSlot(2);
		}

		if(Input.GetKey("4"))
		{
			SetWeaponSlot(3);
		}

		if(Input.GetKey("5"))
		{
			SetWeaponSlot(4);
		}

		if(Input.GetKey("6"))
		{
			SetWeaponSlot(5);
		}

		if(Input.GetKey("7"))
		{
			SetWeaponSlot(6);
		}

		if(Input.GetKey("8"))
		{
			SetWeaponSlot(7);
		}

		if(Input.GetKey("9"))
		{
			SetWeaponSlot(8);
		}	
	}

}