using UnityEngine;
using System.Collections;

public class btn_EnterInitials : Button 
{
	[SerializeField] HighScoreManager sc_HighScoreManager;
	
	public override void Start() 
	{
		base.Start();
	}
	
	public override void PerfromTransition()
	{
		base.PerfromTransition();
		gameObject.collider.enabled = false;
		
		// Open textbar so user can enter their name
		sc_HighScoreManager.AddPlayersName();
		
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
