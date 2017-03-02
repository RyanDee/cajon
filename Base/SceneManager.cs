using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	/*
	 * This class is used to add some common variables to MonoBehavior,
	 * rather than constantly repeating the same declaration in every class.
	*/

	public string[] levelNames;
	public int gameLevelNum;

	public void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void LoadLevel( string sceneName )
	{
		Application.LoadLevel(sceneName);
	}

	public void GoNextLevel()
	{
		if(gameLevelNum >= levelNames.Length)
			gameLevelNum = 0;

		LoadLevel(gameLevelNum);
		gameLevelNum++;
	}

	public void LoadLevel(int indexNum)
	{
		LoadLevel(levelNames[indexNum]);
	}

	public void ResetGame()
	{
		gameLevelNum = 0;
	}

}
