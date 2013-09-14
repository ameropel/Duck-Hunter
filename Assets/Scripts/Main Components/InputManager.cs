using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{	
	void Update()
	{
		ApplicationInput();	
	}
	
	void ApplicationInput ()
	{
		// Android Back Button
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("Back Button");
			Application.Quit();
		}
		
		// Android Menu Button
		if (Input.GetKeyDown(KeyCode.Menu))
		{
			Debug.Log("Menu Button");
		}
		
		// Android Home Button
		if (Input.GetKeyDown(KeyCode.Home))
		{	
		}
	}
	
	void OnGUI()
	{	
		// Make button background colors transparent
		//GUI.backgroundColor = new Color(1,1,1,0);
		
		
	}
	
	// When App is interrupted, ex. Home button
	void OnApplicationPause()
	{
	}
}
