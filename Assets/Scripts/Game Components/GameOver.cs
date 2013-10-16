using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	// Script Helper
	[SerializeField] CameraController sc_CameraController;
	[SerializeField] GameScore 		  sc_GameScore;
	[SerializeField] GunController    sc_GunController;
	[SerializeField] HighScoreManager sc_HighScoreManager;
	
	[SerializeField] GameObject GameHud;
	GameObject Backdrop, GameOverText, GameStats, EnterInitials, MenuButtons;
	
	void Start()
	{
		// Get all gameObject Variables
		Backdrop = transform.FindChild("Backdrop").gameObject;
		GameOverText = transform.FindChild("GameOver").gameObject;
		GameStats = transform.FindChild("GameStatistics").gameObject;
		EnterInitials = transform.FindChild("EnterInitials").gameObject;
		MenuButtons = transform.FindChild("MenuButtons").gameObject;
	}
	
	public void GameOverTransition()
	{
		GameOverText.SetActive(true);
		StartCoroutine( GameOverMenu());
	}
	
	IEnumerator GameOverMenu()
	{
		// Wait for animation of GameOver to finish
		yield return new WaitForSeconds(GameOverText.animation.clip.length);
				
		// Rotate camera around to back of boat
		sc_CameraController.CameraRotate(10, -180, 3.0f);
		
		// Dim background
		yield return StartCoroutine( FadeBackdrop() );
		
		// Display game stats
		yield return StartCoroutine( GameStatistics() );
		
		// Check to see if score made leaderboards
		if (sc_HighScoreManager.CheckScore())
		{
			// Activate button
			EnterInitials.SetActive(true);		
			
			// Wait user entered initials then proceed
			while (!sc_HighScoreManager.isTextCompleted)
				yield return null;	
		}
		else
			Destroy(EnterInitials);
		
		// Activate the menu buttons
		MenuButtons.SetActive(true);
	}
	
	IEnumerator FadeBackdrop()
	{
		// Play hide hud details
		GameHud.animation.Play();
		
		// Set backdrops color
		Backdrop.renderer.material.color = Color.clear;
		Color Trans = new Color(1,1,1,0.667f);
		
		Backdrop.SetActive(true);
		
		float time = 0;
		while (time < 1)
		{
			// One second time
			time += Time.deltaTime;
			Backdrop.renderer.material.color = Color.Lerp( Color.clear, Trans, time);
			yield return null;
		}	
	}
	
	IEnumerator GameStatistics()
	{
		// Pull up game stats
		GameStats.SetActive(true);
		
		// Wait till animation stops playing then proceed with stats
		yield return new WaitForSeconds(GameStats.animation.clip.length + 1.0f); 
		
		// Get Statistic variables, textmeshes
		GameObject Variables = GameStats.transform.FindChild("Variables").gameObject;
		TextMesh ducks_hit = Variables.transform.FindChild("ducks_hit").GetComponent<TextMesh>();
		TextMesh geese_hit = Variables.transform.FindChild("geese_hit").GetComponent<TextMesh>();
		TextMesh accuracy = Variables.transform.FindChild("accuracy").GetComponent<TextMesh>();
		TextMesh streak = Variables.transform.FindChild("streak").GetComponent<TextMesh>();
		TextMesh score = Variables.transform.FindChild("score").GetComponent<TextMesh>();
		
		// Set text for geese and ducks hit to appropriate variables
		ducks_hit.text = sc_GameScore.Ducks_Hit.ToString();
		geese_hit.text = sc_GameScore.Geese_Hit.ToString();
		
		// Set users best streak
		streak.text = sc_GameScore.Best_Streak.ToString();
		
		// Determine the users accuracy on hitting ducks
		float acc = (float)sc_GameScore.Ducks_Hit / (float)sc_GunController.Shots_Fired;
		accuracy.text = ( (ScriptHelper.RoundValue(acc, .01f) * 100 ).ToString() + "%");
		score.text = sc_GameScore.PlayerScore.ToString();
		
		yield return null;	
	}
	
}
