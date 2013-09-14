using UnityEngine;
using System.Collections;

public class btn_Start_Game : Button 
{
	[HideInInspector] public bool GameStart;
	
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{		
		base.PerfromTransition();
		GameStart = true;
	}
}
