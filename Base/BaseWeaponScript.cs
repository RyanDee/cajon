using UnityEngine;
using System.Collections;

/* 
 * 
*/
public class BaseWeaponScript : MonoBehaviour {

	[System.NonSerialized]
	public bool CanFire;

	public int Ammo = 100;
	public int MaxAmmo = 100;

	public bool IsInfiniteAmmo;
	public GameObject ProjectileGO;
	public Collider ParentCollider;

	private Vector3 _fireVector;

	[System.NonSerialized]
	public Transform iTransform;

	private int _layer;

	public Vector3 SpawnPosOffset;
	public float ForWardOffset = 1.5f;
	public float ReloadTime = 0.2f;
	public float ProjectileSpeed = 10f;
	public bool InheritVelocity;

	[System.NonSerialized]
	public Transform TheProjectile;

	private GameObject _theProjectileGO;
	private bool _isLoaded;
	private ProjectileController _theProjectileController;

	public virtual void Start()
	{
		Init();
	}

	public virtual void Init()
	{
		// Cache the transform
		iTransform = transform;

		// Cache the layer (we'll set all projectiles to avoid this layer in collision so that
		// things don't shoot themseleves)
		_layer = gameObject.layer;

		// Load the weapon
		Reloaded();
	}

	public virtual void Enable()
	{
		// Drop out if firing is enable
		if(CanFire) return;

		// Enable weapon (do things like show the weapons mesh etc.)
		CanFire = true;
	}

	public virtual void Disable()
	{
		// Drop down if firing is disable
		if(!CanFire) return;

		// Hide weapon
		CanFire = false;
	}

	public virtual void Reloaded()
	{
		// The isLoaded var tells us if this weapon is loaded and ready to fire
		_isLoaded = true;
	}

	public virtual void SetCollider(Collider collider)
	{
		ParentCollider = collider;
	}


	public virtual void Fire(Vector3 direction, int ownerID)
	{
		// Be sure to check canFire so that the weapon can be enabled or disabled as required
		if(!CanFire) return;

		// If the weapon is not loaded, drop out
		if(!_isLoaded) return;

		// If we're out of ammo and we don't have infinite ammo, drop out
		if(Ammo <=0 && !IsInfiniteAmmo) return;

		// Decrease ammo
		Ammo--;

		// Generate the actual projectile
		FireProjectile(direction, ownerID);


	}

	public virtual void FireProjectile( Vector3 fireDirection, int ownerID)
	{
		// Make our first projectile
		TheProjectile = MakeProjectile(ownerID);

		// Direct the projectile toward the direction of fire
		TheProjectile.LookAt(TheProjectile.position + fireDirection);

		// Sdd some force to move our projectile
		TheProjectile.GetComponent<Rigidbody>().velocity = fireDirection * ProjectileSpeed;

	}

	public virtual Transform MakeProjectile(int ownerID)
	{
		// Create a projectile
		TheProjectile = SpawnController.Instance.Spawn(ProjectileGO, iTransform.position + SpawnPosOffset + (iTransform.forward * ForWardOffset), iTransform.rotation);
		_theProjectileGO = TheProjectile.gameObject;
		_theProjectileGO.layer = _layer;

		// Grab a ref to the projectile's controller so we can pass on some information about it
		_theProjectileController.SetOwnerType(ownerID);

		Physics.IgnoreLayerCollision(iTransform.gameObject.layer, _layer);

		// Note: Maker sure that the parentCollider is a collision mesh which represents the firing object or a collision mesh likely to be
		// hit by a projectile as it is being fired from the vehicle.
		// One limitation with this system is that it only reliably support a signle collision mesh

		if(ParentCollider != null)
		{
			// Diasble collision between 'us' and our projectile so as not to hit ourselves with it
			Physics.IgnoreCollision(TheProjectile.GetComponent<Collider>(), ParentCollider);
		}

		// Return this projetile in case we want to do something else to it
		return TheProjectile;
	}
}
