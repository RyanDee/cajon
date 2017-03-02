using UnityEngine;
using System.Collections;

/**
 * Acts like glue among the input manager, the user manager, and the game-specific player controller.
 * */

public class BasePlayerManager : MonoBehaviour {

	public bool didInit;

	/*
	 * The user manager and AI controllers are publically accessible so that _
	 * our invidual control scripts can access thhem easily.
	*/

	public BaseUserManager DataManager;

	// Note that we initialize on Awake in this class so that it is _
	// ready for other classes to access our details when they initialize on Start

	public virtual void Awake()
	{
	
		didInit = false;

		// Rather than clutter up the start(), func, we call Init to do any start up specifics
		Init();

	}

	public virtual void Init()
	{
		DataManager = gameObject.GetComponent<BaseUserManager>();

		if(DataManager == null)
		{
			DataManager = gameObject.AddComponent<BaseUserManager>();
		}
		didInit = true;
	}

	public virtual void GameFinished()
	{
		DataManager.SetIsFinished(true);
	}

	public virtual void GameStart()
	{
		DataManager.SetIsFinished(false);
	}
}
