using UnityEngine;
using System.Collections;

public class btn_Restart : Button 
{
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		Application.LoadLevel(0);
	}
}
