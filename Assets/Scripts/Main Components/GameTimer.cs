using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour 
{
	// Script Holder
	[SerializeField] GameController sc_GameController;
	[SerializeField] GUIText[] TimerText;				// Countdown text
	
	[HideInInspector] public float CountDownTimer;	// Countdown clock for game over
	[HideInInspector] public bool  GameTime_Ended;	// Boolean used if countdown timer reached zero
	[HideInInspector] public float GameplayTime;	// GameTime, used for any even dealing with time.
	[HideInInspector] public float GameSpeed = 1;	// Game speed. If greater than 1 game speed is reduced.
	const float MAX_GAMEPLAY_TIME = 300;			// Time is in second, default is 300 (5 minutes) 
	
	void Start()
	{
		// Set countdown timer to max amount
		CountDownTimer = MAX_GAMEPLAY_TIME;	
		
		// Set Gameplay Timer
		GameplayTime = (Time.deltaTime / GameSpeed);
		
		// Change gameplay text
		foreach(GUIText text in TimerText)
			text.text = TimeConvert(CountDownTimer);
	}
	
	void Update()
	{
		// Set Gameplay Timer
		GameplayTime = (Time.deltaTime / GameSpeed);
		
		// If game is not playing do not update game time
		if (sc_GameController.GameState != GameController.GameStatus.PLAYING)
			return;
	
		// If game time is up, ignore rest of function
		if (GameTime_Ended)
			return;
		
		// Check if GameTimer time is up
		if (CountDownTimer <= 0)
		{
			// If time is less then or equal to zero,
			// call end game function and ignore rest of function
			EndGame();
			return;
		}
			
		// Subtract Time from GameTimer
		CountDownTimer -= GameplayTime;
		
		// Change gameplay text
		foreach(GUIText text in TimerText)
			text.text = TimeConvert(CountDownTimer);
	}
	
	string TimeConvert(float timer)
	{
		// Calculate minutes and seconds left in game
		float minutes = Mathf.Floor(timer / 60);
		float seconds = Mathf.Floor(timer % 60);
		
		return (minutes.ToString("0") + ":" + seconds.ToString("00"));
	}
	
	void EndGame()
	{
		// GameTimer has ended
		GameTime_Ended = true;
	}
}
