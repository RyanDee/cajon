using UnityEngine;
using System.Collections;

/*
 * BaseGameController
 * This class acts as a hub for active game activity and place where scripts that may not directly connected can send message.
 * 1. Tracking game state
 * 		a. Storeing the game state
 * 		b. Providing function for changing the game state
 * 		c. Chainging the game state based on calls to function such as EndGames(), StartGame(), PauseGame()
 * 2. Create any player objects
 * 3. Setting up the cammera (any specifics such as telling the camera which player to follow, etc.)
 * 4. Communicating between session manager, user manager, and game action (such as score, etc.)
 * 5. Handling any communication between scripts that we would like to keep as standalone and not have to record to talk directly to each other
 * 
 */

public class BaseGameController : MonoBehaviour {

	bool paused;

	public GameObject ExplosionPrefab;

	public virtual void PlayerLostLife () {}

	public virtual void SpawnPlayer () {}

	public virtual void Respawn () {}

	public virtual void StartGame () {}

	public void Explode (Vector3 aPosition)
	{
		// Instantiate an explosion at the position passed into this function
		Instantiate(ExplosionPrefab, aPosition, Quaternion.identity);
	}

	// Deal with enemy destroy
	public virtual void  EnemyDestroyed (Vector3 aPosition, int pointsValue, int hitByID) {}

	public virtual void BossDestroyed() {}

	public virtual void RestartGameButtonPressed ()
	{
		Application.LoadLevel(Application.loadedLevelName);
	}

	public bool Paused
	{
		get
		{
			return paused;
		}
		set
		{
			paused = value;
			if (paused)
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}
}
