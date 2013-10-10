using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	// Script Helper
	[SerializeField] GameObject GameScore;
	
	[SerializeField] GameObject Backdrop;
	[SerializeField] GameObject GameOverText;
	
	public void GameOverTransition()
	{
		GameOverText.SetActive(true);
		StartCoroutine( GameOverMenu());
	}
	
	IEnumerator GameOverMenu()
	{
		// Wait for animation of GameOver to finish
		yield return new WaitForSeconds(GameOverText.animation.clip.length);
		
		float time = 0;
		Backdrop.renderer.material.color = Color.clear;
		Color Trans = new Color(1,1,1,0.667f);
		Backdrop.SetActive(true);
		while (time < 1)
		{
			// One second time
			time += Time.deltaTime;
			Backdrop.renderer.material.color = Color.Lerp( Color.clear, Trans, time);
			yield return null;
		}
		
		GameScore.GetComponent<HighScoreManager>().SaveScore();
	}
}
