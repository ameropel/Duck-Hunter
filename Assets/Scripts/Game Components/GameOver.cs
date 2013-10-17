using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	// Script Helper
	[SerializeField] AudioManager     sc_AudioManager;
	[SerializeField] CameraController sc_CameraController;
	[SerializeField] GameScore 		  sc_GameScore;
	[SerializeField] GunController    sc_GunController;
	[SerializeField] HighScoreManager sc_HighScoreManager;
	
	[SerializeField] GameObject GameHud;
	GameObject Backdrop, GameOverText, GameStats,
			   EnterInitials, MenuButtons, Continue_btn, EndGameMenu;
	[SerializeField] TextMesh BonusScore;
	int bonusIncrement = 100;
	TextMesh players_score;	
	
	[SerializeField] Material Mark_X;
	[SerializeField] Material Mark_Check;
	
	void Start()
	{
		// Get all gameObject Variables
		Backdrop = transform.FindChild("Backdrop").gameObject;
		GameOverText = transform.FindChild("GameOver").gameObject;
		GameStats = transform.FindChild("GameStatistics").gameObject;
		EnterInitials = transform.FindChild("EnterInitials").gameObject;
		MenuButtons = transform.FindChild("MenuButtons").gameObject;
		EndGameMenu = transform.FindChild("EndGameMenu").gameObject;
		Continue_btn = transform.FindChild("Continue").gameObject;
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
		
		// Display End Game menu
		yield return StartCoroutine( Display_EndGameMenu() );
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
		players_score = Variables.transform.FindChild("score").GetComponent<TextMesh>();
		
		// Set text for geese and ducks hit to appropriate variables
		// Increment Ducks hit text to final number, takes two seconds, play duck sound if hits > 0
		if (sc_GameScore.Ducks_Hit > 0)
			sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.DUCK_CALL_1);
		yield return StartCoroutine( Increment_Number(sc_GameScore.Ducks_Hit, 2, ducks_hit) );
		
		// Increment Geese hit text to final number, takes two seconds, play goose sound if hits > 0
		if (sc_GameScore.Geese_Hit > 0)
			sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.GOOSE_CALL_1);	
		yield return StartCoroutine( Increment_Number(sc_GameScore.Geese_Hit, 2, geese_hit) );
		
		// At same time change users streak and accuracy
		// Set users best streak
		StartCoroutine( Increment_Number(sc_GameScore.Best_Streak, 2, streak) );
		// Determine the users accuracy on hitting ducks
		
		float acc = 0;
		if (sc_GunController.Shots_Fired != 0)
		{
			acc = ((float)sc_GameScore.Ducks_Hit / (float)sc_GunController.Shots_Fired);
			acc = (ScriptHelper.RoundValue(acc, .01f) * 100 );
		}
	
		// Set users accuracy
		StartCoroutine( Accuracy_NumberChange(acc, 3, 0.1f, accuracy) );
		
		
		// Increment score text to final number, takes three seconds
		yield return StartCoroutine( Increment_Number(sc_GameScore.PlayerScore, 3, players_score) );
		
		// If score is 0 wait till accuracy is finished
		if (sc_GameScore.PlayerScore == 0)
			yield return new WaitForSeconds(3.0f);
		
		if (acc != 0)
		{
			// Instantiate bonus points and wait till finish
			yield return StartCoroutine( Display_BonusPoints( (int)(acc * bonusIncrement), Variables) );
		}
		
		// Activate Continue button and wait till it is clicked
		Continue_btn.SetActive(true);
		while(!Continue_btn.GetComponent<btn_Continue_ToEnd>().ButtonClicked)
			yield return null;
		
		// Wait till fade of statistics is completed
		yield return new WaitForSeconds(GameStats.animation["endgame_menu_hide_stats"].length);
				
		yield return null;	
	}
	
	// Increments number slowly to finalNumber based pn time. Start from 0 ends at finalNumber
	IEnumerator Increment_Number(int finalNumber, float timeToChange, TextMesh textChange)
	{
		if (finalNumber != 0)
		{
			float temp_number = 0;
			float time = 0;
			while (time < 1)
			{
				// Calc time with delay
				time += Time.deltaTime / timeToChange;
					
				// Lerp time calc, one goes down, one goes up
				temp_number = Mathf.Lerp(0, finalNumber, time);
					
				// Set text values to appropriate number, convert float to int so no decimals are displayed
				textChange.text = Mathf.CeilToInt(temp_number).ToString();
				yield return null;
			}
		}
		
		textChange.text = finalNumber.ToString();
	}
	
	// Display random set numbers for timeToChange ever timeBetweenRandom. 
	// Once timeToChange is up display finalNumber.
	IEnumerator Accuracy_NumberChange(float finalNumber, float timeToChange, float timeBetweenRandom, TextMesh textChange)
	{	
		// Give random number to accuracy
		textChange.text = Random.Range(0, 100).ToString() + "%";
		
		float overalTime = 0;
		float betweenTime = 0;
		while (overalTime < 1)
		{
			overalTime += Time.deltaTime / timeToChange;
			betweenTime += Time.deltaTime / timeBetweenRandom;
			
			// Give accuracy a new number if betweenTime is up and reset betweenTime back to 0
			if (betweenTime >= 1)
			{
				textChange.text = Random.Range(0, 100).ToString() + "%";
				betweenTime = 0;
			}
			
			yield return null;	
		}
				
		// Give accuracy text its final number
		textChange.text = finalNumber.ToString() + "%";
	}
	
	IEnumerator Display_BonusPoints(int bonus, GameObject parent)
	{
		// Instantiate deduction display
		TextMesh points = Instantiate(BonusScore) as TextMesh;
		
		// Change guiText for deduction
		points.text = "+" + bonus.ToString();
		
		// Add points to hud gameObject so scene looks less messy
		points.transform.parent = parent.transform;
		
		yield return new WaitForSeconds(points.gameObject.animation.clip.length);
		
		// Change players score to the one with the bonus
		sc_GameScore.PlayerScore += bonus;
		players_score.text = sc_GameScore.PlayerScore.ToString();
	}
	
	IEnumerator Display_EndGameMenu()
	{
		EndGameMenu.SetActive(true);
		GameObject leaderboardAnswer = EndGameMenu.transform.Find("Variables/leaderboard_answer").gameObject;
		GameObject highscoreAnswer = EndGameMenu.transform.Find("Variables/highscore_answer").gameObject;
		
		// Delay to display leaderboard decision
		yield return new WaitForSeconds(2.0f);
		
		// Check to see if score made leaderboards
		if (sc_HighScoreManager.makeLeaderBoards(sc_GameScore.PlayerScore))
		{
			SetAnswer(true, leaderboardAnswer);
			sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.CELEBRATION_TADA);
			
			// Delay to display high score decision
			yield return new WaitForSeconds(2.0f);
			
			if (sc_HighScoreManager.isHighScore(sc_GameScore.PlayerScore))
			{
				SetAnswer(true, highscoreAnswer);
				EnterInitials.transform.FindChild("sentence").GetComponent<TextMesh>().text = "You Got A";
				EnterInitials.transform.FindChild("sentence_2").GetComponent<TextMesh>().text = "High Score!";
				sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.CELEBRATION_TADA);
			}
			else
			{
				SetAnswer(false, highscoreAnswer);
				EnterInitials.transform.FindChild("sentence").GetComponent<TextMesh>().text = "You Made The";
				EnterInitials.transform.FindChild("sentence_2").GetComponent<TextMesh>().text = "Leaderboards!";
				sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.CROWD_AWW);
			}
			
			// Activate button
			EnterInitials.SetActive(true);	
			
			// Wait user entered initials then proceed
			while (!sc_HighScoreManager.isTextCompleted)
				yield return null;	
		}
		else
		{	
			SetAnswer(false, leaderboardAnswer);
			GameObject scribble = EndGameMenu.transform.Find("Subjects/scribble").gameObject;
			// Scribble out high score
			scribble.animation.Play();
			
			sc_AudioManager.PlayAudioClip((int)AudioManager.SoundClips.CROWD_AWW);
		}
		
		MenuButtons.SetActive(true);
		
		yield return null;
	}
	
	void SetAnswer(bool good, GameObject obj)
	{		
		if (!good)
			obj.renderer.material = Mark_X;
		else
			obj.renderer.material = Mark_Check;
		
		// Set alpha to 1
		obj.renderer.material.color = Color.white;
	}
	
}
