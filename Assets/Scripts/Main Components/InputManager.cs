using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{	
	[SerializeField] GameController sc_GameController;
	
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
			sc_GameController.PauseGame(true);
		}
		
		// Android Menu Button
		if (Input.GetKeyDown(KeyCode.Menu))
		{
			ScriptHelper.DebugString("Menu Button");
		}
		
		// Android Home Button
		if (Input.GetKeyDown(KeyCode.Home))
		{	
		}
	}
}
