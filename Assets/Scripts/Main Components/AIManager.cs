using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour 
{
	// Script Holder
	[SerializeField] GameController sc_GameController;
	
	// Duck Types
	[SerializeField] GameObject Duck;
	[SerializeField] GameObject DuckWave_3_Units;
	
	
	void Start()
	{
		
	}
	
	void Update()
	{
		if (sc_GameController.GameState != GameController.GameStatus.PLAYING)
			return;
			
		
	}
}
