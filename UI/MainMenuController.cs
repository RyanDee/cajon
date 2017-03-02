using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public int WhichMenu = 0;
	public GUISkin memuSkin;
	public string GameDisplayeName = " - Default game name -";
	public string GamePrefsName = "DefaultGame";

	public string SingleGameStartScene;
	public string CoopGameStartScene;

	public float DefaultWidth = 720;
	public float DefaultHeight = 480;

	public float AudioSFXSliderValue;
	public float AudioMusicSliderValue;

	public float GraphicsSliderValue;
	private int DetailLevels = 6;

	void Start()
	{
		// Set up default options, if they have been saved out to prefs already
		if(PlayerPrefs.HasKey(GamePrefsName + "_SFXVol"))
		{
			AudioSFXSliderValue = PlayerPrefs.GetFloat(GamePrefsName+"_SFXVol");
		}
		else
		{
			AudioSFXSliderValue = 1;
		}

		if(PlayerPrefs.HasKey(GamePrefsName+"_MusicVol"))
		{
			AudioMusicSliderValue = PlayerPrefs.GetFloat(GamePrefsName + "_MuicVol");
		}
		else
		{
			AudioMusicSliderValue = 1;
		}

		if(PlayerPrefs.HasKey(GamePrefsName + "_GraphicDetail"))
		{
			GraphicsSliderValue = PlayerPrefs.GetFloat(GamePrefsName + "_GraphicDetail");
		}
		else
		{
			string[] names = QualitySettings.names;
			DetailLevels = names.Length;
			GraphicsSliderValue = DetailLevels;
		}

		// Set the quality setting
		QualitySettings.SetQualityLevel((int)GraphicsSliderValue, true);
	}

	void OnGUI()
	{
		float resX = Screen.width / DefaultWidth;
		float resY = Screen.width / DefaultHeight;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resX, resY, 1));

		// Set the GUI skin to use our custom menu skin

		switch (WhichMenu)
		{
		case 0:
			GUI.BeginGroup(new Rect (DefaultWidth /2 - 150, DefaultHeight/2 - 250, 500, 500));

			// All rectangle are now adjusted to the group. (0,0) is the topleft corner of the group.
			GUI.Label(new Rect (0, 50, 300, 50), GameDisplayeName, "textarea");

			if(GUI.Button(new Rect(0,200,300,40),"START SINGLE","button"))
			{
				PlayerPrefs.SetInt("totalPlayers", 1);
				LoadLevel(SingleGameStartScene);
			}
			if(CoopGameStartScene != "")
			{
				if(GUI.Button(new Rect(0,250,300,40),"START CO-OP"))
				{
					PlayerPrefs.SetInt("totalPlayer",2);
					LoadLevel(CoopGameStartScene);
				}	

				if(GUI.Button(new Rect(0, 300, 300,40), "OPTIONS"))
				{
					ShowOptionMenu();	
				}
			}
			else
			{
				if(GUI.Button(new Rect(0, 250, 300,40), "OPTIONS"))
				{
					ShowOptionMenu();	
				}
			}

			if(GUI.Button(new Rect(0, 400, 300, 40), "EXIT"))
			{
				ConfirmExitGame();
			}

			// End the group we started above. This is very importance to remember
			GUI.EndGroup();
			break;
		case 1:
			GUI.BeginGroup(new Rect (DefaultWidth /2 - 150, DefaultHeight/2 - 250, 500, 500));
			GUI.Label(new Rect(0, 250, 300, 50), "OPTIONS","textarea");
			if(GUI.Button(new Rect(0,250,300,40),"AUDIO OPTION"))
			{
				ShowAudioOptionMenu();
			}
			if(GUI.Button(new Rect(0, 300 ,300,40),"GRAPHIC OPTIONS"))
			{
				ShowGraphicsOptionsMenu();
			}
			if(GUI.Button(new Rect(0, 300 ,300,40),"Back to main menu"))
			{
				GoMainMenu();
			}
			GUI.EndGroup();
			break;
		case 2:
			GUI.BeginGroup(new Rect (DefaultWidth /2 - 150, DefaultHeight/2 - 250, 500, 500));
			GUI.Label(new Rect(0, 250, 300, 50), "Are you sure want to exit?","textarea");
			if(GUI.Button(new Rect(0, 250 , 300, 40),"Yes, Quit Please!"))
			{
				ExitGame();
			}
			if(GUI.Button(new Rect(0, 300 ,300,40),"No, Don't Quit"))
			{
				GoMainMenu();
			}
			GUI.EndGroup();
			break;
		case 3:
			// Audio Option
			GUI.BeginGroup(new Rect (DefaultWidth /2 - 150, DefaultHeight/2 - 250, 500, 500));

			GUI.Label(new Rect(0, 250, 300, 50), "Audio Option", "textarea");

			GUI.Label(new Rect(0, 250, 300, 50), "SFX VOLUME:");
			AudioSFXSliderValue = GUI.HorizontalSlider(new Rect(0, 200, 300, 50), AudioSFXSliderValue, 0.0f, 1f);

			GUI.Label(new Rect(0, 250, 300, 50), "MUSIC VOLUME:");
			AudioMusicSliderValue = GUI.HorizontalSlider(new Rect(0, 300, 300, 50), AudioMusicSliderValue, 0.0f, 1f);


			if(GUI.Button(new Rect(0, 400 , 300, 40),"Back to Option Menu"))
			{
				SaveOptionPrefs();
				ShowOptionMenu();
			}

			GUI.EndGroup();
			break;
		case 4:
			// Graphic options
			GUI.BeginGroup(new Rect (DefaultWidth /2 - 150, DefaultHeight/2 - 250, 500, 500));

			GUI.Label(new Rect(0, 250, 300, 50), "Graphic Option", "textarea");

			GUI.Label(new Rect(0, 250, 300, 50), "SFX VOLUME:");

			GUI.Label(new Rect(0, 170, 300, 20), "Graphic quanlity:");
			GraphicsSliderValue = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(0, 200, 300, 50), GraphicsSliderValue, 0, DetailLevels));


			if(GUI.Button(new Rect(0, 400 , 300, 40),"Back to Option Menu"))
			{
				SaveOptionPrefs();
				ShowOptionMenu();
			}

			GUI.EndGroup();
			break;
		}
	}

	void LoadLevel(string whichLevel)
	{
		Application.LoadLevel(whichLevel);
	}

	void GoMainMenu()
	{
		WhichMenu = 0;
	}

	void ShowOptionMenu()
	{
		WhichMenu = 1;
	}

	void ShowAudioOptionMenu()
	{
		WhichMenu = 3;
	}

	void ShowGraphicsOptionsMenu()
	{
		WhichMenu = 4;
	}

	void SaveOptionPrefs()
	{
		PlayerPrefs.SetFloat(GamePrefsName + "_SFXVol", AudioSFXSliderValue);
		PlayerPrefs.SetFloat(GamePrefsName + "_MusicVol", AudioMusicSliderValue);
		PlayerPrefs.SetFloat(GamePrefsName + "_GraphicsDetail", GraphicsSliderValue);

		// Set the quality setting
		QualitySettings.SetQualityLevel((int)GraphicsSliderValue, true);
	}

	void ConfirmExitGame()
	{
		WhichMenu = 2;
	}

	void ExitGame()
	{
		Application.Quit();
	}

}
