using UnityEngine;
using System.Collections;

public class btn_LoadScene: Button 
{
	public enum GameScenes
	{	Menu = 0, Time_Trial, Target_Practice, Quit	}
	public GameScenes SceneToLoad;
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		
		if (SceneToLoad == GameScenes.Quit)
			Application.Quit();
		else
			Application.LoadLevel((int)SceneToLoad);	
	}
}
