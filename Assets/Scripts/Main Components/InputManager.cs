using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{	
	[SerializeField] GameController sc_GameController;
	[SerializeField] GameScore		sc_GameScore;
	
	void Update()
	{
		ApplicationInput();	
	}
	
	void ApplicationInput ()
	{
		// Android Back Button
		if (Input.GetKeyDown(KeyCode.Escape))
		{
		  	ScriptHelper.DebugString("Back Button");
		}
		
		// Android Menu Button
		if (Input.GetKeyDown(KeyCode.Menu))
		{
			// Test to add score
			//sc_GameScore.ChangeScore(GameScore.ObjectHit.DUCK);
			ScriptHelper.DebugString("Menu Button");
		}
		
		// Android Home Button
		if (Input.GetKeyDown(KeyCode.Home))
		{
			sc_GameController.PauseGame(true);
		}
	}
}
