using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	// Script Helper
	[SerializeField] CameraController sc_CameraController;
	[SerializeField] HighScoreManager sc_HighScoreManager;
	
	[SerializeField] GameObject Backdrop;
	[SerializeField] GameObject GameOverText;
	[SerializeField] GameObject EnterInitials;
	[SerializeField] GameObject MenuButtons;
	
	public void GameOverTransition()
	{
		GameOverText.SetActive(true);
		StartCoroutine( GameOverMenu());
	}
	
	IEnumerator GameOverMenu()
	{
		// Wait for animation of GameOver to finish
		yield return new WaitForSeconds(GameOverText.animation.clip.length);
				
		// Rotate camera around to back of boat
		sc_CameraController.CameraRotate(10, -180, 3.0f);
		
		// Dim background
		yield return StartCoroutine( FadeBackdrop() );
		
		// Check to see if score made leaderboards
		if (sc_HighScoreManager.CheckScore())
		{
			// Activate button
			EnterInitials.SetActive(true);		
			
			// Wait user entered initials then proceed
			while (!sc_HighScoreManager.isTextCompleted)
				yield return null;	
		}
		else
			Destroy(EnterInitials);
		
		// Activate the menu buttons
		MenuButtons.SetActive(true);
	}
	
	IEnumerator FadeBackdrop()
	{
		// Set backdrops color
		Backdrop.renderer.material.color = Color.clear;
		Color Trans = new Color(1,1,1,0.667f);
		
		Backdrop.SetActive(true);
		
		float time = 0;
		while (time < 1)
		{
			// One second time
			time += Time.deltaTime;
			Backdrop.renderer.material.color = Color.Lerp( Color.clear, Trans, time);
			yield return null;
		}	
	}
}
