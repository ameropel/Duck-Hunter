using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	// Script Holder
	#if UNITY_ANDROID && !UNITY_EDITOR
	Android_Compass	sc_Android_Compass;
	#endif
	[SerializeField] GameCountDown sc_GameCountDown;
	
	[HideInInspector] public enum GameStatus
	{	LOADING = 0, PLAYING, PAUSED	};
	[HideInInspector] public GameStatus GameState;
	
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
		// Make games status to be loading
		GameState = GameStatus.LOADING;
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		// Wait until android compass is active
		while(!sc_Android_Compass.CompassLoaded)
			yield return null;
		#endif
		
		// Wait for game start button and counter to be completed
		while(!sc_GameCountDown.GameCounterCompleted)
			yield return null;
		
		// Change game status to playing
		GameState = GameStatus.PLAYING;
	}
}
