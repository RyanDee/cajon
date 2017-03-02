using UnityEngine;
using System.Collections;

public class SpawnController : ScriptableObject {

	private ArrayList playerTransforms;
	private ArrayList playerGameObjects;

	private Transform tempTransform;
	private GameObject tempGameObject;

	private GameObject[] playerPrefabList;
	private Vector3[] startPositions;
	private Quaternion[] startRotations;

	// singleton structure based on AngryAnt's fantastic wiki entry over at:
	// http://wiki.unity3d.com/index.php/Singleton

	private static SpawnController instance;

	public SpawnController()
	{
		// Hàm này sẽ dược gọi khi 1 Instance của SpawnController được tạo ra,
		// Đầu tiên, chúng ta kiểm tra 1 thể hiện đã có hay chưa
		if(instance != null)
		{
			Debug.LogWarning("Đã có Instance của SpawnController này!");
			return;
		}

		//Khi không có 1 thể hiện nào tồn tại, thì chúng có thể tạo thển hiện 1 cách an toàn
		instance = this;
	}

	public static SpawnController Instance
	{
		// Theo cách thông thường, Setter và Getter sẽ là cách để truy xuất dữ liệu trong Singleton của class này
		get
		{
			// the other script is trying to access an instance of this script,
			// so we need to see if an instance allready exists
			if(instance == null)
			{
				// no instance exists yet, so we go ahead CREATE ONE
				ScriptableObject.CreateInstance<SpawnController>();	// new SpawnController
			}
			// now we pass the reference to this instance back to the other script
			// so it can communicate with other.
			return instance;
		}
	}

	public void Restart()
	{
		playerTransforms = new ArrayList();
		playerGameObjects = new ArrayList();
	}

	public void SetUpPlayers (GameObject[] playerPrefabs, Vector3[] playerStartPositions, Quaternion[] playerStartRotations, Transform theParentObj, int totalPlayer)
	{
		// we pass in everthing need to spawn players and take care of spawning players in this class _
		// so that we don't have to replicate this code in every game controller

		playerPrefabList = playerPrefabs;
		startPositions = playerStartPositions;
		startRotations = playerStartRotations;

		// call the function to take care of spawning all the players and putting them in the right places

	}

	public void CreatePlayer (Transform theParent, int totalPlayer)
	{
		playerTransforms = new ArrayList();
		playerGameObjects = new ArrayList();

		for(int i=0; i < totalPlayer; i++)
		{

			tempTransform = Spawn(playerPrefabList[i], startPositions[i], startRotations[i]);

			// if we have passed in an object to parent the players to, set the parent
			if(theParent != null){
				tempTransform.parent = theParent;
				tempTransform.localPosition = startPositions[i];
			}

			// add this transform into our list of player transforms
			playerTransforms.Add(tempTransform);

			// add its gameobject into our list of player gameobjects (cache them separately)
			playerGameObjects.Add(tempTransform.gameObject);
		}
			
	}

	public GameObject GetPlayerGameObject(int indexNum)
	{
		return (GameObject)playerGameObjects[indexNum];
	}

	public Transform GetPlayerTransform (int indexNum)
	{
		return (Transform)playerTransforms[indexNum];
	}

	public Transform Spawn(GameObject anGameObject, Vector3 anPosition, Quaternion aRotation)
	{
		// instanciate the object
		tempGameObject = (GameObject)Instantiate(anGameObject, anPosition, aRotation);
		tempTransform = tempGameObject.transform;
		//return the GameObject to whatever was calling
		return tempTransform;
	}

	// here we just provide a convenient function to return the spwan objects gameobject rather than its transform
	public GameObject SpawnGameObject(GameObject anGameObject, Vector3 aPosition, Quaternion aRotation)
	{
		tempGameObject = (GameObject)Instantiate(anGameObject,aPosition, aRotation);
		tempTransform = tempGameObject.transform;

		//return the object to whatever was calling
		return tempGameObject;
	}

	public ArrayList GetAllSpawnedPlayers()
	{
		return playerTransforms;
	}


}
