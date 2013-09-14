using UnityEngine;
using System.Collections;

public class btn_Fire_Shotgun : Button 
{
	// Script holder
	[SerializeField] GunController sc_GunController;
	
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		sc_GunController.FireShotgun();
	}
}
