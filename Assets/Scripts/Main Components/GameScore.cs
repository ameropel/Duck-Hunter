using UnityEngine;
using System.Collections;

public class GameScore : MonoBehaviour 
{
	// Script holder
	[SerializeField] GameTimer sc_GameTimer;
	
	[SerializeField] GUIText[] ScoreText;	// Score text
	[SerializeField] GUIText[] ComboText;	// Combo text
	
	int duck_score = 1000;		// Value given when hitting a duck
	int goose_score = -1000;	// Value given when hitting a goose
	int miss_shot_score = -10;	// Value given when missing a shot
	int combo_value = 1;		// Value multiplied to positive points
	float combo_timer = 3.0f;	// Time till combo resets to zero
	
	[HideInInspector] public int PlayerScore = 0;	// Score player currently has
	
	[HideInInspector] public enum ObjectHit
	{	MISS = 0, DUCK, GOOSE 	}
	
	void Start()
	{
		// Change gameplay text for score
		foreach(GUIText text in ScoreText)
			text.text = PlayerScore.ToString();	
		
		// Change gameplay text for combo
		foreach(GUIText text in ComboText)
			text.text = "";
	}
	
	public void ChangeScore(ObjectHit hit)
	{
		switch(hit)
		{
			// Player shot and missed target
			case ObjectHit.MISS:
				combo_value = 1;				// Reset combo
				Edit_ComboText();
				StopCoroutine( "ComboTimer" );	// If timer is running stop combo timer
				PlayerScore += miss_shot_score;	// Deduct miss shot score from overall score
				break;	
			// Player shot and hit a duck
			case ObjectHit.DUCK:
				PlayerScore += (duck_score * combo_value);	// Increase score with combo (if any)
				StopCoroutine( "ComboTimer" );		// If timer is running stop combo timer
				StartCoroutine( "ComboTimer" );		// Reset combo timer back to intial countdown
				Edit_ComboText();
				combo_value++;		// Increase combo value by 1
				break;
			// Player shot and hit a goose
			case ObjectHit.GOOSE:
				combo_value = 1;				// Reset combo
				Edit_ComboText();
				StopCoroutine( "ComboTimer" );	// If timer is running stop combo timer
				PlayerScore += goose_score;		// Deduct goose_score from overall score
				break;	
		}
		
		Edit_ScoreText();
	}
	
	void Edit_ComboText()
	{
		// Change gameplay text for combo
		foreach(GUIText text in ComboText)
		{
			if (combo_value == 1)
				text.text = "";
			else
				text.text = ("x" + combo_value).ToString();
		}
	}
	
	void Edit_ScoreText()
	{
		// Change gameplay text for score
		foreach(GUIText text in ScoreText)
			text.text = PlayerScore.ToString();	
	}
	
	IEnumerator ComboTimer()
	{
		float time = 0;
		while (time < 1)
		{
			// Calculate timer for combo
			time += sc_GameTimer.Game_deltaTime / combo_timer;
			yield return null;
		}
		
		// Change combo value back to 1
		combo_value = 1;
		Edit_ComboText();
	}
	
}
