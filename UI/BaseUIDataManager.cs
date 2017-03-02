using UnityEngine;
using System.Collections;

public class BaseUIDataManager : MonoBehaviour {

	// The actual UI drawing is done by a script deriving from this one
	public int player_score;
	public int player_lives;
	public int player_highscore;

	public string gamePrefsName = "DafaultGame";

	public void UpdateScoreP1(int score)
	{
		player_score = score;
		if(player_score > player_highscore)
		{
			player_highscore = player_score;
		}
	}

	public void UpdateScore(int score)
	{
		player_score = score;
	}

	public void UpdateLives(int alifeNum)
	{
		player_lives = alifeNum;
	}

	public void LoadHighScore()
	{
		if(PlayerPrefs.HasKey(gamePrefsName + "_highScore"))
		{
			player_highscore = PlayerPrefs.GetInt(gamePrefsName+"_highScore");
		}
	}

	public void SaveHighScore()
	{
		// As we know that the game is over, let's save out the high score too
		PlayerPrefs.SetInt(gamePrefsName +"_hightSore", player_highscore);
	}
}
