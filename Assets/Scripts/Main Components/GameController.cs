using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class GameController : MonoBehaviour 
{	
	// Script Holder
	#if UNITY_ANDROID && !UNITY_EDITOR
	Android_Compass	sc_Android_Compass;
	#endif
	[SerializeField] GameCountDown sc_GameCountDown;
	
	[SerializeField] GameObject PauseMenu;
	
	[HideInInspector] public enum GameStatus
	{	LOADING = 0, PLAYING, PAUSED, QUIT, GAMEOVER	};
	[HideInInspector] public GameStatus GameState;
	[HideInInspector] public delegate void PauseGameplay();
	[HideInInspector] public delegate void UnPauseGameplay();
	[HideInInspector] public static event PauseGameplay Gameplay_Pause;
	[HideInInspector] public static event UnPauseGameplay Gameplay_UnPause;
	
	void Awake()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		// Find main android game component. This gameobject has all android related scripts attached to it.
		GameObject Android_Component = GameObject.FindGameObjectWithTag("Android Component").gameObject;
		
		// Attach scripts
		sc_Android_Compass = Android_Component.GetComponent<Android_Compass>();
		#endif
	}
	
	void Start () 
	{
		StartCoroutine( GameStartUp() );
	}
	
	IEnumerator GameStartUp()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		// Make games status to be loading
		GameState = GameStatus.LOADING;
		
		// Wait until android compass is active
		while(!sc_Android_Compass.CompassLoaded)
			yield return null;
		
		// Wait for game start button and counter to be completed
		while(!sc_GameCountDown.GameCounterCompleted)
			yield return null;
		
		// Change game status to playing
		GameState = GameStatus.PLAYING;
		#else
		
		// Change game status to playing
		GameState = GameStatus.PLAYING;
		yield return null;
		#endif
	}
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	// When App is interrupted, ex. Home button
	void OnApplicationPause()
	{
		PauseGame(true);
	}
	
	#endif
	void OnApplicationQuit()
	{
		GameState = GameStatus.QUIT;
		PlayerPrefs.Flush();
	}
	
	public void PauseGame(bool pauseGame)
	{
		if (pauseGame)
		{
			GameState = GameStatus.PAUSED;
			if (Gameplay_Pause != null)
				Gameplay_Pause();
		}
		else
		{
			GameState = GameStatus.PLAYING;
			if (Gameplay_UnPause != null)
				Gameplay_UnPause();
		}
		
		PauseMenu.SetActive(pauseGame);
	}
}