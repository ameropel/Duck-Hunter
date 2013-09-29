using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour 
{
	// Script Holder
	[SerializeField] GameController sc_GameController;
	
	// Duck Types
	[SerializeField] Transform DuckWave;
	
	
	void Start()
	{
		StartCoroutine( StartGame() );
	}
	
	IEnumerator StartGame()
	{
		while (sc_GameController.GameState != GameController.GameStatus.PLAYING)
			yield return null;
		
		// Activate ducks
		foreach (Transform wave in DuckWave)
			wave.gameObject.SetActive(true);
	}
	
	void Update()
	{
		if (sc_GameController.GameState != GameController.GameStatus.PLAYING)
			return;	
	}
}
