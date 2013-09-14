using UnityEngine;
using System.Collections;

public class btn_hold_Zoom_Weapon : MouseHold 
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
		sc_GunController.Zoom_In = true;
	}
}
