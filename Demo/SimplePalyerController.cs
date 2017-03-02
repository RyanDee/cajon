using UnityEngine;
using System.Collections;

public class SimplePalyerController : BaseTopDownSpaceShip {

	public BasePlayerManager iPlayerManager;
	public BaseUserManager iDataManager;

	// Use this for initialization
	public override void Start () {
		base.Init();
		this.Init();
		GameStart();
	}

	public override void Init()
	{
		if(iPlayerManager == null)
		{
			iPlayerManager = iGameobject.GetComponent<BasePlayerManager>();
		}

		iDataManager = iPlayerManager.DataManager;
		iDataManager.SetName("Player");
		iDataManager.SetHealth(3);

		didInit = true;
	}

	// Update is called once per frame
	public override void Update () {
		UpdateShip();

		if(!didInit) return;

		// check to see if we're supposed to be controlling the player before checking for firing
		if(!canControl) return;
	}

	public override void GetInput()
	{
		// we're overriding the default input function to add in the ability to fire
		horizontal_input = default_input.GetHorizontal();
		vertical_input = default_input.GetVertical();
	}

	void OnCollisionEnter(Collision collider)
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		
	}

	void PlayerFinish()
	{
		//Deal wit the end of the game for this player
	}

	public void ScoredPoints (int howMany)
	{
		iDataManager.AddScore(howMany);
	}

}
