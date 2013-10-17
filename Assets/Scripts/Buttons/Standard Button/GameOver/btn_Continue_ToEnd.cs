using UnityEngine;
using System.Collections;

public class btn_Continue_ToEnd : Button 
{	
	[HideInInspector] public bool ButtonClicked;
	[SerializeField] GameObject GameStatistics;
	[SerializeField] string endgame_menu_hide_stats;
	
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		
		// Register that the button was hit
		ButtonClicked = true;
		
		// Play the animation where all game compenents, except score, fade out
		GameStatistics.animation.Play(endgame_menu_hide_stats);
		
		// Turn off collider
		gameObject.collider.enabled = false;
		
		// Get animation name
		string clip_name = gameObject.animation.clip.name;
		
		// Reverse animation
		gameObject.animation[clip_name].speed = -1.0f;
		gameObject.animation[clip_name].normalizedTime = 1.0f;
		
		// Play animation
		gameObject.animation.Play();
		
		// Destroy gameObject once animation is completed
		Destroy(gameObject, gameObject.animation[clip_name].length);
	}
}
