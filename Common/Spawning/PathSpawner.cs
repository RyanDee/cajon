using UnityEngine;
using System.Collections;
/*
public class PathSpawner : MonoBehaviour {

	public WaypointsController waypointControl;

	// should we start spawing based on distance from the camera?private
	// if distanceBased is false,, we will need to call this class elsewhere, to spawn
	public bool distanceBasedSpawnStart;

	// if we're using distance based spawning, at what distance should we start?
	public float distanceFromCameraToSpawnAt = 35f;

	// if the distanceBasedSpwanStart is false, we can have tha pasth spwaner just start spawing automatically
	public float shouldAutoStartSpawningOnLoad;

	public float timeBetweenSpawns = 1;
	public int totalAmountToSpawn = 10;
	public bool shouldReversePath;

	public GameObject[] spawnObjectPrefabs;

	private int totalSpawnObjects;

	private Transform iTransform;

	private int spawnCounter = 0;
	private int currentObjectNum;
	private Transform cameraTransform;
	private bool spawning;

	public bool shouldSetSpeed;
	public float speedToSet;
	public bool shouldSetSmoothing;
	public float smoothingToSet;
	public bool shouldSetRotateSpeed;
	public float rotateToSet;

	public bool didInit;

	void Start()
	{
		Init();
	}

	void Init()
	{
		//cache ref to our transform
		iTransform = transform;

		//cache ref to the camera
		Camera mainCam = Camera.main;

		if(mainCam == null)
			return;

		cameraTransform = mainCam.transform;

		//tell waypoint controller if we want to reverse the path or not
		//waypointControl.SetReverseMode(shouldReversePath);

		totalSpawnObjects = spawnObjectPrefabs.Length;

		if(shouldAutoStartSpawningOnLoad)
			StartWave(totalAmountToSpawn, timeBetweenSpawns);
		
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0,0,1,0.5f);
		Gizmos.DrawCube(transform.position, new Vector3(200, 0, distanceFromCameraToSpawnAt));
	}

	public void Update()
	{
		float aDist = Mathf.Abs(iTransform.position.z - cameraTransform.position.z);
		if(distanceBasedSpawnStart && !spawning && aDist <distanceFromCameraToSpawnAt>)
		{
			StartWave(totalAmountToSpawn, timeBetweenSpawns);
			spawning = true;
		}
	}

	public void StartWave(int HowMany, float timeBetweenSpawns)
	{
		spawnCounter = 0;
		totalAmountToSpawn = HowMany;

		//reset
		currentObjectNum = 0;

		CancelInvoke("doSpawn");

		InvokeRepeating("doSpawn", timeBetweenSpawns, timeBetweenSpawns);

	}

	public void doSpawn()
	{
		SpawnObject();
	}

	public void SpawnObject()
	{
		if(spawnCounter > totalAmountToSpawn)
		{
			// tell your script that the wave is finished
			CancelInvoke("doSpawn");
			this.enabled = false;
			return;
		}

		// create an object

		GameObject tmpObject = SpawnController.Instance.SpawnGameObject(spawnObjectPrefabs[currentObjectNum],iTransform.position, Quaternion.identity);

		// tell object to reverse its pathfinding, if required;
		tmpObject.SendMessage("SetReversePath",shouldReversePath, SendMessageOptions.DontRequireReceiver);

		// tell spawned object to use this waypoint controller
		tmpObject.SendMessage("SetWayController", waypointControl, SendMessageOptions.DontRequireReceiver);

		// tell object to use this spedd (againt with no required receiver just in case)
		if(shouldSetSpeed)
			tmpObject.SendMessage("SetSpeed", speedToSet, SendMessageOptions.DontRequireReceiver);

		// tell object to use this speed again with no required receiver just in case
		if(shouldSetSmoothing)
			tmpObject.SendMessage("SetPathSmoothingRate", smoothingToSet, SendMessageOptions.DontRequireReceiver);

		// tell object to use this speed (again with no required receiver just in case)
		if(shouldSetRotateSpeed)
			tmpObject.SendMessage("SetRotateSpeed", rotateToSet, SendMessageOptions.DontRequireReceiver);

		// increase the 'how many objects we have spawned' counter
		spawnCounter++ ;

		// increse the 'which object to spawn' counter
		currentObjectNum++ ;

		// check to see if we've reached the end of the spawn objects array
		if(currentObjectNum > totalSpawnObjects -1)
			currentObjectNum = 0;


	}

}

*/