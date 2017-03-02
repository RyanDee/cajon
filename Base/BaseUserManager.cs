using UnityEngine;
using System.Collections;

/*
 * The user manager is a script object made by the player manager to store player properties
 * - Player name
 * - Current score
 * - Highest score
 * - Level
 * - Health
 * - Whether or not this player has finished the game
 * Contains function to manipulate data
 * 
 */

public class BaseUserManager : MonoBehaviour {

	/*
	 * Gameplay specific data
	 * we keep these private and provide methods to modify them
	 * instead, just to prevent any accidental corruption of
	 * invalid data coming in
	*/

	private int score;
	private int hightScore;
	private int level;
	private int health;
	private bool isFinished;

	public string playerName = "An Cajon Player Name";

	public virtual void GetDefaultData()
	{
		playerName = "AN CAJON player Name";
		score = 0;
		level = 1;
		health = 3;
		hightScore = 0;
		isFinished = false;
	}

	public string GetName ()
	{
		return playerName;
	}
		
	public void SetName (string name)
	{
		playerName = name;
	}

	public int GetLevel()
	{
		return level;
	}

	public void SetLevel(int num)
	{
		level = num	;
	}

	public int GetHighScore()
	{
		return hightScore;
	}

	public int GetScore()
	{
		return score;
	}

	public virtual void AddScore( int amount)
	{
		score += amount;
	}

	public void LostScore(int num)
	{
		score -= num;
	}

	public void SetScore(int num)
	{
		score = num;
	}

	public int GetHealth()
	{
		return health;
	}

	public void AddHeath(int num)
	{
		health += num;
	}

	public void ReduceHealth(int num)
	{
		health -= num;
	}

	public void SetHealth(int num)
	{
		health = num;
	}

	public bool GetIsFinish()
	{
		return isFinished;
	}

	public void SetIsFinished(bool ival)
	{
		isFinished = ival;
	}

}
