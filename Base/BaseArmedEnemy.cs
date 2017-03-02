using UnityEngine;
using System.Collections;
using AIAttackState;

public class BaseArmedEnemy : ExtendCustomMonoBehavior {

	[System.NonSerialized]
	public bool DoFire;

	public bool OnlyFireWhenOnScreen;

	public int PoitsValue = 50;
	public int ThisEnemyStrength = 1;
	public bool ThisGameObjectShouldFire;

	// We use a renderer to test whether or not the ship is on screen
	public Renderer rendererToTestAgainst;

	public StandardWeaponSlotController weaponControl;

	public GameObject mesh_parentGO;

	private bool _canFire;

	public float FireDelayTime = 1f;

	public BasePlayerManager iPlayerManager;
	public BaseUserManager iDatamanager;

	public bool isBoss = false;

	public int tempInit;

	// Default action is to attack nothing
	public AIAttackStates currentAttackState = AIAttackStates.random_fire;

	public string tagOfTargetsToShootAt;

	public void Start()
	{
		// Now call our script-specific init function
		InitThis();
	}

	public void InitThis()
	{
		// Cache our transform
		iTransform = transform;

		// Cache our gameobject
		iGameobject = gameObject;

		if(weaponControl == null)
		{
			// Try to find weapon controller on this gameObject
			weaponControl = iGameobject.GetComponent<StandardWeaponSlotController>();
		}

		if(rendererToTestAgainst == null)
		{
			// We need to find out whether or not we are on the screen, so let's try and find one in our children
			// if we don't already have one set in the editor
			rendererToTestAgainst = iGameobject.GetComponentInChildren<Renderer>();
		}

		// If a player manager is not set in the editor, let's try to find one
		if(iPlayerManager == null)
		{
			iPlayerManager = iGameobject.AddComponent<BasePlayerManager>();
		}

		iDatamanager = iPlayerManager.DataManager;
		iDatamanager.SetName("Enemy");
		iDatamanager.SetHealth(ThisEnemyStrength);

		_canFire = true;
		didInit = true;
	}

	private RaycastHit rayHit;

	public virtual void Update()
	{
		// If we are not allowed to control the weapon, we drop out here
		if(!canControl) return;

		if(ThisGameObjectShouldFire)
		{
			// We use doFire to determine whether or not to fire right now
			DoFire = false;

			// CanFire is used to control a delay between firing
			if(_canFire)
			{
				if(currentAttackState == AIAttackStates.random_fire)
				{
					// If the random numver is over x, fire
					if(Random.Range(0,100) > 98)
					{
						DoFire = true;
					}
				}
				else if (currentAttackState == AIAttackStates.look_and_destroy)
				{
					if(Physics.Raycast(iTransform.position, iTransform.forward, out rayHit))
					{
						// Is it an opponent to be shot at?
						if(rayHit.transform.CompareTag(tagOfTargetsToShootAt))
						{
							// We have a match on the tag, so let's shoot at it
							DoFire = true;
						}
					}
				}
				else
				{
					DoFire = true;
				}
			}
		}
		if(DoFire)
		{
			// We only want to fire if we are on screen, visible on the main camera
			if(OnlyFireWhenOnScreen && !rendererToTestAgainst.isVisible)
			{
				DoFire = false;
				return;
			}

			// Tell weapon controler to fire, if we have a weapon controller
			if(weaponControl != null)
			{
				// Tell weapons to fire
				weaponControl.Fire();
			}

			// Set a flag to disable firing temporarily (providing a delay between firing)
			_canFire = false;

			// Invoke a function call in <FireDelayTime> to reset canFire back to true, allowing another
			// firing session
			Invoke("ResetFire", FireDelayTime);
		}
	}

	public void ResetFire()
	{
		_canFire = true;
	}

}
