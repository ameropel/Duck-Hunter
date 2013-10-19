using UnityEngine;
using System.Collections;

public class btn_Pause_Resume : Button 
{
	// Script Holder
	[SerializeField] GameController sc_GameController;
	
	public enum GameplayButton
	{	Pause_Game = 0, Resume_Game	}
	public GameplayButton ButtonAction;
	
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		if (ButtonAction == GameplayButton.Pause_Game)
			sc_GameController.PauseGame(true);
		else if (ButtonAction == GameplayButton.Resume_Game)
			sc_GameController.PauseGame(false);
	}
}
