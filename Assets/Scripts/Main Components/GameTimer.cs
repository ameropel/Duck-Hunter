using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour 
{
	// Script Holder
	[SerializeField] AudioManager   sc_AudioManager;
	[SerializeField] GameController sc_GameController;
	[SerializeField] GameOver       sc_GameOver;
	
	[SerializeField] TextMesh TimerText;			// Countdown text
	[HideInInspector] public float CountDownTimer;	// Countdown clock for game over
	[HideInInspector] public bool  GameTime_Ended;	// Boolean used if countdown timer reached zero
	[HideInInspector] public float Game_deltaTime;	// GameTime, used for any even dealing with time.
	[HideInInspector] public float GameSpeed = 1;	// Game speed. If greater than 1 game speed is reduced.
	[HideInInspector] public const float MAX_GAMEPLAY_TIME = 300;	// Time is in second, default is 300 (5 minutes) 
	
	void Start()
	{
		// Set countdown timer to max amount
		CountDownTimer = MAX_GAMEPLAY_TIME;	
		
		// Set Gameplay Timer
		Game_deltaTime = (Time.deltaTime / GameSpeed);
		
		// Change gameplay text
		TimerText.text = TimeConvert(CountDownTimer);
	}
	
	void Update()
	{		
		// Set Gameplay Timer
		Game_deltaTime = (Time.deltaTime / GameSpeed);
		
		// If game is not playing do not update game time
		if (sc_GameController.GameState != GameController.GameStatus.PLAYING)
			return;
	
		// If game time is up, ignore rest of function
		if (GameTime_Ended)
			return;
		
		// Check if GameTimer time is up
		if (CountDownTimer <= 1)
		{
			// If time is less then or equal to zero,
			// call end game function and ignore rest of function
			EndGame();
			return;
		}
			
		// Subtract Time from GameTimer
		CountDownTimer -= Game_deltaTime;
		
		// Change gameplay text
		TimerText.text = TimeConvert(CountDownTimer);
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
		sc_GameController.GameState = GameController.GameStatus.GAMEOVER;
		sc_GameOver.GameOverTransition();
	}
	
	public void SlowGameplayDown(float speed)
	{
		// Value can not be less then 1. If so game will speed up
		// This function is only meant to slow down the game, not speed up
		if (speed < 1)
			return;
		
		// Slow down game by speed amount
		GameSpeed += speed;
		
		// Slow down audio (pitch decrease) by speed amount
		sc_AudioManager.ChangePitch(speed);
		
		// Debug current game speed
		ScriptHelper.DebugString("Game Speed = " + (1 - speed));
	}
}
