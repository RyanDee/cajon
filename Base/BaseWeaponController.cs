using UnityEngine;
using System.Collections;

public class BaseWeaponController : MonoBehaviour {

	public GameObject[] weapons;

	public int SelectedWeaponSlot;
	public int LastSelectedWeaponsSlot;

	public Vector3 OffseWeaponsSpawnPosition;

	public Transform ForceParent;

	private ArrayList _weaponsSlots;
	private ArrayList _weaponsScript;
	private BaseWeaponScript _tmpWeapon;
	private Vector3 _tmpVector3;
	private Quaternion _tmpProtation;
	private GameObject _tmpGameObject;

	private Transform iTransform;
	private int _ownerNum;

	public bool UseForceVectorDirection;
	public Vector3 ForceVector;
	private Vector3 TheDir;

	public void Start()
	{
		// Default to the firsst weapons slot
		SelectedWeaponSlot = 0;
		LastSelectedWeaponsSlot = -1;

		// Initialize weapon list Arraylist
		_weaponsSlots = new ArrayList();

		// initialize weapon scripts ArrayList
		_weaponsScript = new ArrayList();

		// Cache a reference to the transform (looking up a transform each step can be expensive
		// so this is importance)
		iTransform = transform;

		if(ForceParent == null)
		{
			ForceParent = iTransform;
		}

		// Rather than look up the transform position and rotation of the player each interatction of this loop below,
		// we cache them first into temporaty variables
		_tmpVector3 = ForceParent.position;
		_tmpProtation = ForceParent.rotation;

		// We instantiate all of the weapons and hide them so that we can active and use them when we needed.
		for(int i=0; i < weapons.Length; i++)
		{
			// Instantiate the item from the weapon controller script is attched to, to be the parent of the weapon
			// so that the weapon will move around wwith the player

			// Note: If you need projectiles to be on a different player from the main gameObject, set the layer of the forceParent
			// objeect to the layer you want projecttiles to be on

			_tmpGameObject.transform.parent = ForceParent;
			_tmpGameObject.layer = ForceParent.gameObject.layer;
			_tmpGameObject.transform.position = ForceParent.position;
			_tmpGameObject.transform.rotation = ForceParent.rotation;

			// Store a reference to the gameObject in an ArrayList
			_weaponsSlots.Add(_tmpGameObject);

			// Grab a reference to the weapon script attached to the weapon and store the refence in an ArrayList
			_tmpWeapon = _tmpGameObject.GetComponent<BaseWeaponScript>();
			_weaponsScript.Add(_tmpWeapon);

			// Disable the weapon
			_tmpGameObject.SetActive(false);
		}

		// Now we set the default selected weapon to visible
		SetWeaponSlot(0);

	}

	public void SetOwner (int aNum)
	{
		// Used to indentify the object firing, if required
		_ownerNum = aNum;
	}

	public virtual void SetWeaponSlot (int slotNum)
	{
		// If the selected weapon is already this one, drop out!
		if (slotNum == LastSelectedWeaponsSlot)
		{
			return;
		}

		// Disable the current weapon
		DisableCurrentWeapon();

		// Set out current weapon to the one passed in
		SelectedWeaponSlot = slotNum;

		// Make sure sensible value are getting passed in
		if(SelectedWeaponSlot < 0)
		{
			SelectedWeaponSlot = _weaponsSlots.Count - 1;
		}

		// Make sure that the weapon slot isn't higher then the total number of weapons in out list
		if(SelectedWeaponSlot > _weaponsSlots.Count -1)
		{
			SelectedWeaponSlot = _weaponsSlots.Count - 1;
		}

		// We store this selected slot to use to prevent duplicates weaponslot setting
		LastSelectedWeaponsSlot = SelectedWeaponSlot;

		// Enable the newly selected weapon
		EnableCurrentWeapon();
	}

	public virtual void NextWeaponsSlot (bool shouldLoop)
	{
		// Disable the current weapons
		DisableCurrentWeapon();

		// Next slot
		SelectedWeaponSlot++;

		// Make sure that the slot isn't higher than the total number of weapons in out list
		if(SelectedWeaponSlot == _weaponsScript.Count)
		{
			if(shouldLoop)
			{
				SelectedWeaponSlot = 0;
			}
			else
			{
				SelectedWeaponSlot = _weaponsScript.Count - 1;
			}
		}

		// We store this selected slot to use to prevent duplicate weapon slot setting

		LastSelectedWeaponsSlot = SelectedWeaponSlot;

		// Enable the newly selected weapn
		EnableCurrentWeapon();
	}

	public virtual void PrevWeaponSlot (bool shouldLoop)
	{
		// Disable the current weapons
		DisableCurrentWeapon();

		// Prev slot
		SelectedWeaponSlot--;

		// Make sure that the slot is a Sensible number
		if(SelectedWeaponSlot < 0)
		{
			if(shouldLoop)
			{
				SelectedWeaponSlot = _weaponsScript.Count - 1;
			}
			else
			{
				SelectedWeaponSlot = 0;
			}
		}

		// We store this selected slot to use to prevent duplicate weapon slot setting
		LastSelectedWeaponsSlot = SelectedWeaponSlot;

		// Enable the newly selected weapn
		EnableCurrentWeapon();
	}

	public virtual void DisableCurrentWeapon()
	{
		if(_weaponsScript.Count == 0)
		{
			return;
		}

		// Grab reference to currently selected weapon script
		_tmpWeapon = (BaseWeaponScript) _weaponsScript[SelectedWeaponSlot];

		// now tell the script to disable itself
		_tmpWeapon.Disable();

		// Grab reference to the weapon's gameObject and disable the, too

		_tmpGameObject = (GameObject)_weaponsSlots[SelectedWeaponSlot];

		_tmpGameObject.SetActive(false);

	}

	public virtual void EnableCurrentWeapon()
	{
		if(_weaponsScript.Count == 0)
			return;

		// Grab refrence to currently selected weapon
		_tmpWeapon = (BaseWeaponScript)_weaponsScript[SelectedWeaponSlot];

		// Now tell the script to enable itselft
		_tmpWeapon.Enable();

		_tmpGameObject = (GameObject)_weaponsSlots[SelectedWeaponSlot];

		_tmpGameObject.SetActive(false);
	}

	public virtual void Fire()
	{
		if(_weaponsScript == null || _weaponsScript.Count == 0)
		{
			return;
		}

		// Find the weapon in the currently selected slot
		_tmpWeapon = (BaseWeaponScript) _weaponsScript[SelectedWeaponSlot];

		TheDir = iTransform.forward;

		if(UseForceVectorDirection)
			TheDir = ForceVector;

		// fire the projectiles
		_tmpWeapon.Fire(TheDir, _ownerNum);

	}

}
