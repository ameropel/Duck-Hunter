using UnityEngine;
using System.Collections;

public class GameScore : MonoBehaviour 
{
	// Script holder
	[SerializeField] GameTimer     sc_GameTimer;
	
	[SerializeField] TextMesh ScoreText;	// Score text
	[SerializeField] TextMesh ComboText;	// Combo text
	[SerializeField] TextMesh DeductText;	// Prefab to deduct points
	
	int duck_score = 1000;		// Value given when hitting a duck
	int goose_score = -1000;	// Value given when hitting a goose
	int miss_shot_score = -10;	// Value given when missing a shot
	int combo_value = 1;		// Value multiplied to positive points
	float combo_timer = 3.0f;	// Time till combo resets to zero
	
	// Statistics
	[HideInInspector] public int Ducks_Hit = 0;
	[HideInInspector] public int Geese_Hit = 0;
	[HideInInspector] public int Best_Streak = 0;
	[HideInInspector] public int Current_Streak = 0;
	
	[HideInInspector] public int PlayerScore = 0;	// Score player currently has
	
	[HideInInspector] public enum ObjectHit
	{	MISS = 0, DUCK, GOOSE 	}
	
	void Start()
	{
		// Change gameplay text for score
		ScoreText.text = PlayerScore.ToString();	
		
		// Change gameplay text for combo
		ComboText.text = "";
	}
	
	public void ChangeScore(ObjectHit hit)
	{
		switch(hit)
		{
			// Player shot and missed target
			case ObjectHit.MISS:
				DeductPoints(miss_shot_score);
				break;	
			
			// Player shot and hit a duck
			case ObjectHit.DUCK:
				// Take note that user hit a duck
				Ducks_Hit++;
			
				PlayerScore += (duck_score * combo_value);	// Increase score with combo (if any)
				StopCoroutine( "ComboTimer" );		// If timer is running stop combo timer
				StartCoroutine( "ComboTimer" );		// Reset combo timer back to intial countdown
				Edit_ComboText();
				combo_value++;		// Increase combo value by 1
				
				// Calculate the users streak
				Current_Streak++;
				if (Best_Streak < Current_Streak)
					Best_Streak = Current_Streak;
				break;
			
			// Player shot and hit a goose
			case ObjectHit.GOOSE:
				// Take note that user hit a goose
				Geese_Hit++;
				DeductPoints(goose_score);
				break;	
		}
		
		Edit_ScoreText();
	}
	
	void DeductPoints(int deduction)
	{
		combo_value = 1;				// Reset combo
		Current_Streak = 0;				// Reset current streak to 0
		Edit_ComboText();
		StopCoroutine( "ComboTimer" );	// If timer is running stop combo timer
		PlayerScore += deduction;		// Deduct points from overall score
		Display_Deduction(deduction);
	}
	
	void Display_Deduction(int deduction)
	{
		// Instantiate deduction display
		TextMesh points = Instantiate(DeductText) as TextMesh;
		
		// Change guiText for deduction
		points.text = deduction.ToString();
		
		// Add points to hud gameObject so scene looks less messy
		points.transform.parent = ComboText.transform.parent;
	}
	
	void Edit_ComboText()
	{
		// Change gameplay text for combo
		if (combo_value == 1)
			ComboText.text = "";
		else
			ComboText.text = ("x" + combo_value).ToString();
	}
	
	void Edit_ScoreText()
	{
		// Change gameplay text for score
		ScoreText.text = PlayerScore.ToString();	
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
