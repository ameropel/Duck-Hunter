using UnityEngine;
using System.Collections;

public class btn_Resume_Game : Button 
{

	// Script Holder
	[SerializeField] GameController sc_GameController;
	
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		sc_GameController.PauseGame(false);
	}
}
